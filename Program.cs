using ProductApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MVC + tell Razor to look in /Features/{Feature}/{View}.cshtml first
builder.Services
    .AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Clear();
        // Feature folders
        options.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
        // Fallback to default view locations (so existing Home views keep working)
        options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    });

// DI registration from Week 2
builder.Services.AddSingleton<IPriceCalculator, PriceCalculator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Keep default conventional route as a fallback.
// Attribute routes on controllers take precedence.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
