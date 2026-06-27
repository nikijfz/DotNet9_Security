using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Net9.Security.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

<<<<<<< HEAD
#region [- Database & Identity Configuration -]
=======
>>>>>>> 3c5ed30d555e0e9e5f1bedd4b957af89a62eb16a
var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:Default");
builder.Services.AddDbContext<ApplicationDbContext>(c => c.UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
}
).AddEntityFrameworkStores<ApplicationDbContext>();
<<<<<<< HEAD
#endregion

#region [- AddAuthorization() -]
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator", "Admin"));
});
#endregion

#region [- ConfigureApplicationCookie() -]
=======
>>>>>>> 3c5ed30d555e0e9e5f1bedd4b957af89a62eb16a
builder.Services.ConfigureApplicationCookie(Options =>
{
    Options.LoginPath = "/Account/Login";
    Options.AccessDeniedPath = "/Account/AccessDenied";
});
<<<<<<< HEAD
#endregion
=======
>>>>>>> 3c5ed30d555e0e9e5f1bedd4b957af89a62eb16a

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

#region [- IdentityDataSeeder -]

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

<<<<<<< HEAD
        if (app.Environment.IsDevelopment())
        {
            await IdentityDataSeeder.SeedDataAsync(userManager, roleManager);
        }
=======
        await IdentityDataSeeder.SeedDataAsync(userManager, roleManager);
>>>>>>> 3c5ed30d555e0e9e5f1bedd4b957af89a62eb16a
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
<<<<<<< HEAD
}
=======
} 
>>>>>>> 3c5ed30d555e0e9e5f1bedd4b957af89a62eb16a
#endregion

app.Run();
