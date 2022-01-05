var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddCors();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.UseDeveloperExceptionPage();
} else {
    app.UseExceptionHandler("/Error");
}
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
/*
app.UseCors(options => {
    options.AllowAnyMethod().AllowAnyHeader();
    options.SetIsOriginAllowed((host) => true);
    options.AllowCredentials();
});
*/
app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
app.MapFallbackToFile("index.html").AllowAnonymous();

app.Run();