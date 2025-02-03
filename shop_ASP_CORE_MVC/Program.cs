using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shop_ASP_CORE_MVC.Models;
using shop_ASP_CORE_MVC.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options =>
{
    //lay ben trong json
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectDb"]);
});



// Thêm các d?ch v? vào container
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // B? nh? t?m cho session

// C?u hình Session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Th?i gian h?t h?n khi không ho?t ??ng
	options.Cookie.IsEssential = true; // ??m b?o cookie c?a session là c?n thi?t
});

builder.Services.AddIdentity<AppUserModel,IdentityRole>()
	.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 4;

	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = false;
});
var app = builder.Build();
app.UseStatusCodePagesWithRedirects("~/Home/Error?statuscode={0}");

app.UseSession();
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "category",
    pattern: "category/{Slug?}",
    defaults: new { controller = "Category", action = "Index" }
);
app.MapControllerRoute(
    name: "brand",
    pattern: "brand/{Slug?}",
    defaults: new { controller = "brand", action = "Index" }
);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seeding Data
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<DataContext>();
	SeedData.SeedingData(context);
}

app.Run();
