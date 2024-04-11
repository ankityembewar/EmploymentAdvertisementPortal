using EAP.BAL.Agent.Login;
using EAP.BAL.IAgent.ILogin;
using EAP.Core.Data;
using EAP.Core.Mapper;
using EAP.DAL.IService.ILoginService;
using EAP.DAL.Service.LoginService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EmployeeAdvertisementPortalContext") ?? throw new InvalidOperationException("Connection string 'EmployeeAdvertisementPortalContext' not found.");

builder.Services.AddDbContext<EmployeeAdvertisementPortalContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Dependency Injection
builder.Services.AddScoped<ILoginAgent, LoginAgent>();
builder.Services.AddScoped<ILoginService, LoginService>();
#endregion

#region Automapper
builder.Services.AddAutoMapper(typeof(Mapper));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=UserLogin}/{id?}");

app.Run();
