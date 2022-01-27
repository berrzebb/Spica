using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
.ClearProviders()
.AddSimpleConsole();

builder.Services.AddMemoryCache();

// Add Youtube Music Service Singleton With DI
builder.Services.AddSingleton<YoutubeConfig>();
builder.Services.AddSingleton<IYoutubeMusicService, YoutubeMusicService>();

// Add services to the container.
builder.Services.AddCors();

builder.Services.AddControllers(options => {
    options.RespectBrowserAcceptHeader = true;
}) // Newtonsoft.Json 을 사용하도록 변경합니다.
.AddNewtonsoftJson(options => {
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

// Swagger Configure
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "UnOfficial Youtube Music API",
        Description = "ASP .NET Core Web API for Youtube Music API",
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
} else {
    app.UseExceptionHandler("/Error");
}
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();



app
.UseCors(options => {
    options.AllowAnyMethod().AllowAnyHeader();
    options.SetIsOriginAllowed((host) => true);
    options.AllowCredentials();
})
.UseHttpsRedirection()
.UseHttpLogging()
.UseDefaultFiles()
.UseStaticFiles()
.UseRouting()
.UseAuthentication()
.UseAuthorization();

app.MapControllers();
//app.MapFallbackToFile("index.html").AllowAnonymous();

await app.RunAsync();