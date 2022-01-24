using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Create Youtube Music Service
var youtubeMusicService = new YoutubeMusicService();
// Add Youtube Music Service Singleton
builder.Services.AddSingleton<IYoutubeMusicService>(youtubeMusicService);

// Add services to the container.
builder.Services.AddCors();

builder.Services.AddControllers()
.AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
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