using ApiGateway.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.json")
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

//builder.Services.AddSwaggerForOcelot(Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
}); // Try create swagger with all apis but I can't do it

app.UseAuthentication();

app.UseOcelot().Wait();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
