using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//O que nos precisamos para comecar o projeto
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//Procedure called sitting 
//Este bloco de codigo é chamado sempre que reiniciamos a aplicacao
using (var scope = app.Services.CreateScope())
{
    var roleManager = 
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Member" };
    
    //se role nao existir devemos criar
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
           
    }
}

// Caso este email exista, adicionar a role admin
using (var scope = app.Services.CreateScope())
{
    //Definicao de UserManager
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
   //Todos os utilizadores sao membros  
    var users = userManager.Users.ToList();
        foreach (var user in users)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            if (!userRoles.Contains("Member"))
            {
                await userManager.AddToRoleAsync(user, "Member");
            }
        } 
        
    // O utilizador com este email passa a ser admin 
    string email = "admin123@gmail.com";

    var adminuser = await userManager.FindByEmailAsync(email);
    if (adminuser != null )
    {
        var roles = await userManager.GetRolesAsync(adminuser);
        if (!roles.Contains("Admin"))
        {
         await userManager.AddToRoleAsync(adminuser, "Admin");
        }
    }
}

app.Run();