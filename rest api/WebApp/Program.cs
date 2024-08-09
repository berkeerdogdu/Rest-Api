using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "Controller=Home/(Action=Index)");
});
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
