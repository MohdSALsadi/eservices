using eservices.Culture;
using eservices.Models;
using eservices.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pattern_of_life;
using Pattern_of_life.Data;
using Pattern_of_life.Repository;
using Pattern_of_life.Repository.Interface;
using Pattern_of_life.Service;
using Pattern_of_life.Services;
using PdfSharp.Charting;
using PdfSharp.Fonts;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Pattern_of_life.Data;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Add logging services
// builder.Services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
// builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IContextAccessor, ContextAccessor>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ShipActivityDensityCalculator>();
builder.Services.AddScoped<SettingsRepository>();
builder.Services.AddScoped<INotficationService, NotificationService>();
//builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
 .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();


builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5242880;
});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//    {
//        options.LoginPath = "/account/login"; // Specify your login page
//        options.AccessDeniedPath = "/account/accessdenied"; // Specify your access denied page
//    });




builder.Services.AddAuthorization();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "eservices";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var supportedCultures = new[]
   {
        new CultureInfo("en-US"),
        //new CultureInfo("ar-EG"),
        // Add more cultures as needed
    };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseCultureMiddleware();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
