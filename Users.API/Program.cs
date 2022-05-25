using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Core.Users;
using Users.DataAccess;
using Users.Users.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("MyPolicy",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200",
                                   "https://localhost:4200")
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
            });
    })
    .AddDbContext<UserContext>(options => //.AddSqlite<MyDbContext>(builder.Configuration.GetConnectionString("Default"))
        options.UseInMemoryDatabase(databaseName: "inventory"))
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserContext>();

builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,//true,
            ValidateAudience = false,//true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapPost("/token", async (AuthenticateRequestDto request, UserManager<User> userManager) =>
{
    var user = await userManager.FindByNameAsync(request.UserName);

    if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
    {
        return Results.Forbid();
    }

    var roles = await userManager.GetRolesAsync(user);
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Sid, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
    };

    foreach (var role in roles)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    var tokenDescriptor = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(720),
        signingCredentials: credentials);

    var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

    return Results.Ok(new
    {
        AccessToken = token
    });
});
app.MapGet("/me", (IHttpContextAccessor contextAccessor) =>
{
    var user = contextAccessor.HttpContext.User;

    return Results.Ok(new
    {
        Claims = user.Claims.Select(s => new
        {
            s.Type,
            s.Value
        }).ToList(),
        user.Identity.Name,
        user.Identity.IsAuthenticated,
        user.Identity.AuthenticationType
    });
})
.RequireAuthorization();

await SeedData();

app.Run();



async Task SeedData()
{
    var scopeFactory = app!.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<UserContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    context.Database.EnsureCreated();

    if (!userManager.Users.Any())
    {
        logger.LogInformation("Creando usuario de prueba");

        var newUser = new User
        {
            Email = "test@demo.com",
            FirstName = "Test",
            LastName = "User",
            UserName = "test.demo"
        };

        await userManager.CreateAsync(newUser, "P@ss.W0rd");
        await roleManager.CreateAsync(new IdentityRole
        {
            Name = "Admin"
        });
        await roleManager.CreateAsync(new IdentityRole
        {
            Name = "User"
        });

        await userManager.AddToRoleAsync(newUser, "Admin");
        await userManager.AddToRoleAsync(newUser, "User");
    }
}