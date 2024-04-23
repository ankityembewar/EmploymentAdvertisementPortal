using EAP.BAL.Agent.Employee;
using EAP.BAL.Agent.Login;
using EAP.BAL.IAgent.IEmployee;
using EAP.BAL.IAgent.ILogin;
using EAP.Core.Data;
using EAP.Core.Mapper;
using EAP.DAL.IService.Employee;
using EAP.DAL.IService.Login;
using EAP.DAL.Service.Employee;
using EAP.DAL.Service.LoginService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EmployeeAdvertisementPortalContext") ?? throw new InvalidOperationException("Connection string 'EmployeeAdvertisementPortalContext' not found.");

builder.Services.AddDbContext<EmployeeAdvertisementPortalContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure session options.
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".YourApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

#region Dependency Injection
builder.Services.AddScoped<ILoginAgent, LoginAgent>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmployeeAgent, EmployeeAgent>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddHttpContextAccessor();
#endregion

#region Automapper
builder.Services.AddAutoMapper(typeof(Mapper));
#endregion

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login/userlogin"; // Set the login page path
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Use session middleware.
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=UserLogin}/{id?}");

app.Run();
