using AHB.Core;
using AHB.Extensions;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Configuration.AddJsonFile(RsConfigurationDefaults.AppSettingsFilePath, true, true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureApplicationServices(builder);

// Add services to the container.
//builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapRazorPages();

app.ConfigureRequestPipeline();
app.StartEngine();

app.Run();
