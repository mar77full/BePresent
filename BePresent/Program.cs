using BePresent.Application.Interfaces;
using BePresent.Domain;
using BePresent.Infrastructure;
using BePresent.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();


builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 5;
    options.User.RequireUniqueEmail = true;
}
)
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Add services to the container.
//builder.Services.AddRazorPages();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

//})
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//    {
//        options.LoginPath = "/Accounts/Signin"; // Redirect here if not logged in
//        options.LogoutPath = "/Accounts/Signout";
//        options.Cookie.Name = "BePresentCookie";

//    });


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Signin"; // Redirect here if not logged in
    options.LogoutPath = "/Accounts/Signout";
    options.Cookie.Name = "BePresentCookie";
    options.ExpireTimeSpan= TimeSpan.FromMinutes(5);
    options.SlidingExpiration = true;

});
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Accounts/Signin", "");
});
var app = builder.Build();

app.UseSerilogRequestLogging();

await AppDbInitializer.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
//app.MapRazorPages(options => {
//    options.Conventions.AddPageRoute("/Accounts/Signin");
//});

app.Run();
