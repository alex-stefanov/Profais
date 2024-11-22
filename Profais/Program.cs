namespace Profais
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Profais.Data;
    using Profais.Data.Models;
    using Profais.Extensions;

    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var environment = builder.Environment;

            builder.Configuration
                .AddEnvironmentSpecificJsonFiles(environment, out string connectionString);

            builder.Services
                .AddDbContext<ProfaisDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

            builder.Services
               .AddIdentity<ProfUser, IdentityRole<Guid>>(cfg =>
               {
                   ConfigureIdentity(builder, cfg);
               })
               .AddEntityFrameworkStores<ProfaisDbContext>()
               .AddRoles<IdentityRole<Guid>>()
               .AddSignInManager<SignInManager<ProfUser>>()
               .AddUserManager<UserManager<ProfUser>>();

            builder.Services
                .RegisterRepositories()
                .RegisterUserDefinedServices();

            builder.Services
                .AddControllersWithViews();

            builder.Services
                .AddRazorPages()
                .AddRazorRuntimeCompilation();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/User/Login";
                cfg.LogoutPath = "/User/Logout";
            });

            builder.Services.AddSession(cfg =>
            {
                cfg.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder, IdentityOptions cfg)
        {
            cfg.Password.RequireDigit =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
            cfg.Password.RequireLowercase =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            cfg.Password.RequireUppercase =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            cfg.Password.RequireNonAlphanumeric =
                builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumerical");
            cfg.Password.RequiredLength =
                builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
            cfg.Password.RequiredUniqueChars =
                builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueCharacters");

            cfg.SignIn.RequireConfirmedAccount =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            cfg.SignIn.RequireConfirmedEmail =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
            cfg.SignIn.RequireConfirmedPhoneNumber =
                builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

            cfg.User.RequireUniqueEmail =
                builder.Configuration.GetValue<bool>("Identity:User:RequireUniqueEmail");
        }
    }
}
