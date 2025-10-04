using app.auth.Extentions;
using app.auth.Application.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.AddArchtectures();
builder.AddServices();
builder.AddSecurity();

var app = builder.Build();

app.MapUserEndpoints();

app.UseArchtectures();

app.UseHttpsRedirection();

app.Run();
