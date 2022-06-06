using Microsoft.EntityFrameworkCore;
using Ordering.ApplicationServices;
using Ordering.ApplicationServices.Orders;
using Ordering.ApplicationServices.Products;
using Ordering.ApplicationServices.Users;
using Ordering.Core.Orders;
using Ordering.DataAccess;
using Ordering.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<OrderingContext>(options =>
               //options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
               options.UseInMemoryDatabase(databaseName: "ordering"));

builder.Services.AddTransient<IRepository<int, Order>, Repository<int, Order>>();

builder.Services.AddTransient<IOrdersAppService, OrdersAppService>();
builder.Services.AddTransient<IProductsAppService, ProductsAppService>();
builder.Services.AddTransient<IUsersAppService, UsersAppService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

HttpClientHandler clientHandler = new HttpClientHandler();
clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

builder.Services.AddHttpClient("inventory", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AppSettings:InventoryUrlBase"]);
}).ConfigurePrimaryHttpMessageHandler(() => (clientHandler));

builder.Services.AddHttpClient("user", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AppSettings:UserUrlBase"]);
}).ConfigurePrimaryHttpMessageHandler(() => (clientHandler));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
