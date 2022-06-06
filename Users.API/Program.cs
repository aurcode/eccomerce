using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.ApplicationServices.Users;
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

builder.Services.AddTransient<UserManager<User>>();
builder.Services.AddTransient<IUsersAppService, UsersAppService>();
builder.Services.Configure<Users.Core.JwtConfig>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddControllers()
    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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

        var newAdmin = new User
        {
            NormalizedUserName = "admin",
            Email = "admin@demo.com",
            FirstName = "TestAdmin",
            LastName = "Admin",
            UserName = "admin.demo"
        };

        var newUser = new User
        {
            NormalizedUserName = "admin",
            Email = "user@demo.com",
            FirstName = "TestUser",
            LastName = "User",
            UserName = "user.demo"
        };
        
        await userManager.CreateAsync(newUser, "P@ss.W0rd");
        await userManager.CreateAsync(newAdmin, "P@ss.W0rd");
        
        await roleManager.CreateAsync(new IdentityRole
        {
            Name = "admin"
        });
        await roleManager.CreateAsync(new IdentityRole
        {
            Name = "user"
        });

        await userManager.AddToRoleAsync(newAdmin, "admin");
        //await userManager.AddToRoleAsync(newAdmin, "user");
        await userManager.AddToRoleAsync(newUser, "user");
    }
}