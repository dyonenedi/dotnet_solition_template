using app.blazor.UI.Shared;
using app.blazor.UI.Handlers;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CookieAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

#region Inject Services
builder.Services.AddMudServices();

// Configurar HttpClients para diferentes APIs
if (builder.Environment.IsDevelopment())
{
    // HttpClient para app.auth (porta 5005)
    builder.Services.AddHttpClient("BLAZOR", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5005/");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        return handler;
    });
    // HttpClient para app.auth (porta 5006)
    builder.Services.AddHttpClient("AUTH", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5006/");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        return handler;
    });

    // HttpClient para app.api (porta 5007)
    builder.Services.AddHttpClient("API", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5007/");
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        return handler;
    });
}
else
{
    // HttpClient para app.blazor (produção)
    builder.Services.AddHttpClient("BLAZOR", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5005/");
    });
    // HttpClient para app.auth (produção)
    builder.Services.AddHttpClient("AUTH", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5006/");
    });
    // HttpClient para app.api (produção)
    builder.Services.AddHttpClient("API", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5007/");
    });
}
#endregion

// Dynamically register all handlers from app.blazor.Handlers namespace
var handlerTypes = typeof(Program).Assembly.GetTypes()
    .Where(t => t.Namespace == "app.blazor.Handlers" && t.IsClass && !t.IsAbstract);

foreach (var type in handlerTypes)
{
    builder.Services.AddScoped(type);
}

builder.Services.AddAuthentication("MyCookie")
    .AddCookie("MyCookie", options => {
        options.Cookie.Name = "accessToken"; // ou o nome do seu cookie
        // ...outras opções...
    });
builder.Services.AddAuthorization();

var app = builder.Build();

#region  Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
#endregion

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();