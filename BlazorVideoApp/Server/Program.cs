using BlazorVideoApp.Server.Hubs;
using BlazorVideoApp.Server.Options;
using BlazorVideoApp.Server.Services;
using BlazorVideoApp.Shared;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(options => options.EnableDetailedErrors = true)
    .AddMessagePackProtocol();

builder.Services.Configure<TwilioSettings>(settings =>
{
    settings.AccountSid = builder.Configuration["Twilio:TWILIO_ACCOUNT_SID"];
    settings.ApiSecret = builder.Configuration["Twilio:TWILIO_API_SECRET"];
    settings.ApiKey = builder.Configuration["Twilio:TWILIO_API_KEY"];
});

builder.Services.AddSingleton<TwilioService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddResponseCompression(opts =>
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" }));

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles(new StaticFileOptions
{
    HttpsCompression = HttpsCompressionMode.Compress,
    OnPrepareResponse = context =>
        context.Context.Response.Headers[HeaderNames.CacheControl] =
            $"public,max-age={86_400}"
});

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHub<NotificationHub>(HubEndpoint.NotificationHub);
app.Run();
