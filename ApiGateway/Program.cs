using ApiGateway;
using ApiGateway.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net;
using System.Text;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.Development.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
                        ClockSkew = new System.TimeSpan(0)
                    };
                });
builder.Services.AddOcelot(configuration)
    //.AddDelegatingHandler<RemoveEncodingDelegatingHandler>(true)
    .AddDelegatingHandler<BlackListHandler>()
    //.AddSingletonDefinedAggregator<UsersPostsAggregator>();
    ;
builder.Services.AddSwaggerForOcelot(configuration);
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}); // Try create swagger with all apis but I can't do it

app.UseAuthentication();

//var configuration = new OcelotPipelineConfiguration
//{
    //AuthorisationMiddleware
        //= async (downStreamContext, next) =>
        //await OcelotJwtMiddleware.CreateAuthorizationFilter(downStreamContext, next)
//};

var config= new OcelotPipelineConfiguration
{
    AuthorizationMiddleware = async (ctx, next) =>
    {
        if (OcelotJwtMiddleware.Authorize(ctx))
        {
            await next.Invoke();
        }
        else
        {
            new DownstreamResponse(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("Unauthorized", Encoding.UTF8, "application/json")
            });
        }

    }
};

app.UseOcelot(config).Wait();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
