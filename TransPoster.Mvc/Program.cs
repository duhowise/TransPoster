using AspNetCore.ReCaptcha;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data;
using TransPoster.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = "Server=.;Database=TransPoster;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True" ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentitySetup();
builder.Services.AddServices();
builder.Services.AddQuartzSetup();
builder.Services.AddLocalizationSetup();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);//You can set Time   
});
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));

builder.Services.AddControllersWithViews().AddViewLocalization();

var context = builder.Services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
await context.Database.MigrateAsync();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

await app.RunAsync();
