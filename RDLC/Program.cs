using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RDLC.Infrastructure;
using RDLC.Models;
using RDLC.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSingleton(new AppData());

services.AddScoped<IReportService, ReportService>();

// Add services to the container.
services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
