using Eihal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

string test = string.Empty;
// Write file using StreamWriter

test = "l8";
var builder = WebApplication.CreateBuilder(args);
test = "l10";
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
test = "l12";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
test = "l16";
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
test = "l18";

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.User.RequireUniqueEmail = false).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
test = "l22";
builder.Services.AddRazorPages();
test = "l25";
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});
test = "l36";
builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.SignIn.RequireConfirmedEmail = true;
});
test = "l41";

builder.Services.AddScoped<RoleManager<IdentityRole>>();


test = "l46";
//// Add Admin Role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
         policy => policy.RequireRole("Administrator"));

    options.AddPolicy("RequireUserRole",
         policy => policy.RequireRole("User"));

    options.AddPolicy("RequireDoctorRole",
         policy => policy.RequireRole("Doctor"));
});

test = "l60";
// Read a file
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    //using (var scope = app.Services.CreateScope())
    //{
    //    await DbInitializer.Initialize(scope.ServiceProvider);
    //}
}
test = "l70";
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
test = "l82";
app.UseHttpsRedirection();
app.UseStaticFiles();
test = "l85";
app.UseRouting();
test = "87";
app.UseAuthentication();
app.UseAuthorization();
test = "l90";
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (StreamWriter writer = new StreamWriter("./Debug.txt"))
    writer.WriteLine(test);

app.Run();

