// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using Radzen;
using TrakHound.Apps;
using TrakHound.Clients;
using TrakHound.Configurations;
using TrakHound.Volumes;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRadzenComponents();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

//var clientConfiguration = new TrakHoundHttpClientConfiguration("www.trakhound.com", 443, "demo", true);
var clientConfiguration = new TrakHoundHttpClientConfiguration("localhost", 8472);

var clientProvider = new TrakHoundHttpClientProvider(clientConfiguration);
builder.Services.AddSingleton<ITrakHoundClientProvider>(clientProvider);
builder.Services.AddSingleton<ITrakHoundClient>(clientProvider.GetClient());

var volumeProvider = new TrakHoundVolumeProvider();

var injectionServiceManager = new TrakHoundAppInjectionServiceManagerDebug(clientProvider, volumeProvider);
builder.Services.AddSingleton<ITrakHoundAppInjectionServiceManager>(injectionServiceManager);

builder.Services.AddTransient<TrakHoundTransientAppInjectionServices>();
builder.Services.AddScoped<TrakHoundScopedAppInjectionServices>();
builder.Services.AddSingleton<TrakHoundSingletonAppInjectionServices>();
builder.Services.AddTransient<ITrakHoundAppInjectionServices, TrakHoundAppInjectionServices>();

var app = builder.Build();

// Set ServiceProvider for TrakHound Injection Services
injectionServiceManager.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<SHARC.App.Debug>().AddInteractiveServerRenderMode();
app.Run();
