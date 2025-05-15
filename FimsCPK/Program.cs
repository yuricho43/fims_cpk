using FimsCPK.Entities;
using FimsCPK.Models;
using FimsCPK.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Telerik.SvgIcons;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var cscpk = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FimsDbContext>(options => options.UseSqlServer(cscpk));
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(cscpk));

//--- yscho add ----------------------
builder.Services.AddTelerikBlazor();
builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<CpkService>();
builder.Services.AddScoped<UserConfigureService>();
builder.Services.AddHttpClient();

//--- login
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();  // for access user in razor
builder.Services.AddIdentity<AspNetUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
    options.SignIn.RequireConfirmedEmail = false;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();
//------------------------------------

var app = builder.Build();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
