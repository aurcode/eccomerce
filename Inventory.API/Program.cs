
using Inventory;
using Inventory.ApplicationServices.Brands;
using Inventory.ApplicationServices.Categories;
using Inventory.ApplicationServices.Products;
using Inventory.Core.Brands;
using Inventory.Core.Categories;
using Inventory.Core.Products;
using Inventory.DataAccess;
using Inventory.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
//builder.Host.UseSerilog(((ctx, lc) => lc
//                        .ReadFrom.Configuration(ctx.Configuration)));

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console());

// Add services to the container.

builder.Services.AddCors(options =>
 {
     options.AddPolicy("MyPolicy",
         policy =>
         {
             policy.WithOrigins("http://localhost:4200",
                                "https://localhost:4200")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
         });
 });

string connectionString = builder.Configuration.GetConnectionString("Default");
//string connectionString = builder.Configuration.GetConnectionString("MigrationConnection");
builder.Services.AddDbContext<InventoryContext>(options =>
               //options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
               options.UseInMemoryDatabase(databaseName: "inventory"));

builder.Services.AddTransient<IBrandsAppService, BrandsAppService>();
builder.Services.AddTransient<IRepository<int, Brand>, Repository<int, Brand>>();

builder.Services.AddTransient<ICategoriesAppService, CategoriesAppService>();
builder.Services.AddTransient<IRepository<int, Category>, Repository<int, Category>>();

builder.Services.AddTransient<IProductsAppService, ProductsAppService>();
builder.Services.AddTransient<IRepository<int, Product>, ProductsRepository>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddControllers().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<InventoryContext>();
    //dataContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Policy");

app.MapControllers().RequireCors("MyPolicy");

app.Run();
