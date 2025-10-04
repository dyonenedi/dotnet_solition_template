
using app.blazor.UI.Shared;
using app.blazor.UI.Handlers;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#region Inject Services
builder.Services.AddMudServices();

// Configurar HttpClients para diferentes APIs
if (builder.Environment.IsDevelopment())
{
    // HttpClient para app.auth (porta 6000)
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

    // HttpClient para app.api (porta 7000)
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
    // HttpClient para app.auth (produção)
    builder.Services.AddHttpClient("AUTH", client =>
    {
        client.BaseAddress = new Uri("https://localhost:6000/");
    });

    // HttpClient para app.api (produção)
    builder.Services.AddHttpClient("API", client =>
    {
        client.BaseAddress = new Uri("https://localhost:7000/");
    });
}
#endregion

builder.Services.AddScoped<AuthHandler>();

var app = builder.Build();

#region  Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();
app.UseAntiforgery();
#endregion

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();