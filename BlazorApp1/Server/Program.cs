using BlazorApp1.Server.Data;
using BlazorApp1.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlServer(connectionString));
    options/*.UseLazyLoadingProxies()*/.UseMySql("Server=127.0.0.1;Database=BlazDb;uid=root;pwd=root;default command timeout=120", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.15-MariaDB-0+deb11u1")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer(options =>
{
    options.LicenseKey = "eyJhbGciOiJQUzI1NiIsImtpZCI6IklkZW50aXR5U2VydmVyTGljZW5zZWtleS83Y2VhZGJiNzgxMzA0NjllODgwNjg5MTAyNTQxNGYxNiIsInR5cCI6ImxpY2Vuc2Urand0In0.eyJpc3MiOiJodHRwczovL2R1ZW5kZXNvZnR3YXJlLmNvbSIsImF1ZCI6IklkZW50aXR5U2VydmVyIiwiaWF0IjoxNjUyNDM3ODEzLCJleHAiOjE2ODM5NzM4MTMsImNvbXBhbnlfbmFtZSI6IkFydGljYSBXYXJlIFNybCIsImNvbnRhY3RfaW5mbyI6InN2aWx1cHBvQGFydGljYXdhcmUuaXQiLCJlZGl0aW9uIjoiQ29tbXVuaXR5In0.hmQ_mrqMsI5a2kju_RI6Ch3hUFSVP_56qx4vS87-steVB7U52O9EPi7PhlhUWivViq6LQt3wE1ERZ5DdpMGho6JLeuy4jgEJHD6yfBiI3Tvfp1AS0zK2Fksgw_1LKDOhivUNrLgNvE2LkOATLEm6OESnPDeIWfC4mzgCOkG9d8bn1PYIIZM1h4h9BHZ8ziLdi03oZOBa_l5MBFlmfAXo2L0JC_XSkXKrmzbj4inruuapBIBB3L618orFBpPuRMKKEzq3khmpn3QJrXYRiInC2fveLPSTZLeWhm9f8Fz2Ift8KRevUURpGyjuX9XnaHpbFSs0lJk3Uuv5lB0RfRRQMw";

})
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
