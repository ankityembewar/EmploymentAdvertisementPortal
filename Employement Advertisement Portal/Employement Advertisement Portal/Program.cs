using EAP.BAL.Agent.Advertisement;
using EAP.BAL.Agent.Employee;
using EAP.BAL.Agent.Login;
using EAP.BAL.IAgent.IAdvertisement;
using EAP.BAL.IAgent.IEmployee;
using EAP.BAL.IAgent.ILogin;
using EAP.Core.Data;
using EAP.Core.HelperUtilities;
using EAP.Core.Mapper;
using EAP.DAL.IService.Employee;
using EAP.DAL.IService.IAdvertisement;
using EAP.DAL.IService.Login;
using EAP.DAL.Service.Advertisement;
using EAP.DAL.Service.Employee;
using EAP.DAL.Service.LoginService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

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
builder.Services.AddScoped<HelperUtility>();
builder.Services.AddScoped<ILoginAgent, LoginAgent>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmployeeAgent, EmployeeAgent>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAdvertisementAgent, AdvertisementAgent>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
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
// Configure the HTTP request pipeline to serve static files from the specified directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(@"D:\PersonalProject\EAP\Employement Advertisement Portal\Employement Advertisement Portal\wwwroot", "Image\\Advertisement")), // Specify the directory
    RequestPath = "/Image" // Specify the request path
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Use session middleware.
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
