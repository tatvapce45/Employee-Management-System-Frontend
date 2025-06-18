using EmployeeManagementSystemFrontend.Web;
using EmployeeManagementSystemFrontend.Web.Common;
using EmployeeManagementSystemFrontend.Web.DtoValidators;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<UserClaimsHelper>();
builder.Services.AddScoped<JwtAuthorizeFilter>();
builder.Services.AddScoped<TokensHelper>();
builder.Services.AddTransient<JwtTokenHandler>();

builder.Services.AddValidationServices();

builder.Services.AddHttpClient("EmployeeManagementApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5287");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
})
.AddHttpMessageHandler<JwtTokenHandler>();

builder.Services.AddHttpClient("TokenApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5287");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<JwtAuthorizeFilter>();  // Global JWT filter
})
.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<UserRegistrationDtoValidator>();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}");

app.Run();
