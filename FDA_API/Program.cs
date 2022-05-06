using FDA_API.Integration.Abstractions;
using FDA_API.Integration.Clients;
using FDA_API.Integration.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
								.MinimumLevel.Debug()
								.WriteTo.File("logs/FDA_API_.txt", rollingInterval: RollingInterval.Day)
								.CreateLogger();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IFdaService, FdaService>();
builder.Services.AddScoped<IFdaClient, FdaClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
