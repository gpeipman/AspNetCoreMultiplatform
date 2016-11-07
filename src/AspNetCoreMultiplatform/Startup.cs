using AspNetCoreMultiplatform.Data;
using AspNetCoreMultiplatform.Models.EventViewModels;
using AspNetCoreMultiplatform.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace AspNetCoreMultiplatform
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.private.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var databaseType = Configuration.GetValue<int>("DatabaseType");
            if (databaseType == 0)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MssqlConnection")));
            }
            else if (databaseType == 1)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")));
            }
            else if (databaseType == 2)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySQL(Configuration.GetConnectionString("MysqlConnection")));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var cookieOptions = new CookieAuthenticationOptions();
            cookieOptions.AutomaticChallenge = true;
            cookieOptions.LoginPath = "/Account/Login";

            app.UseCookieAuthentication(cookieOptions);

            Mapper.Initialize(config =>
            {
                config.CreateMap<Event, EventListModel>();
                config.CreateMap<Event, EventViewModel>();
                config.CreateMap<EventViewModel, Event>()
                      .ForMember(m => m.Id, m => m.Ignore())
                      .ForMember(m => m.Owner, m => m.Ignore());
            });

            app.UseStaticFiles();
            app.UseIdentity();
            

            //var options = new MicrosoftAccountOptions();
            //options.ClientId = Configuration.GetValue<string>("Authentication:MicrosoftAccount:ClientId");
            //options.ClientSecret = Configuration.GetValue<string>("Authentication:MicrosoftAccount:ClientSecret");
            //app.UseMicrosoftAccountAuthentication(options);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
