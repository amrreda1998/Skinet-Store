//Create the builder object
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container to the dependecy injection container.
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt=>{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductRepository,ProductRepository>();

//build the app
var app = builder.Build();

// Configure the HTTP request pipeline if there were middlewares.

//map the Routes to the apropriate controller/action for it 
app.MapControllers();

//run the app
app.Run();
