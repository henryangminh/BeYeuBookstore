using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BeYeuBookstore.Services;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.EF;
using BeYeuBookstore.Data.IRepositories;
using BeYeuBookstore.Data.EF.Repositories;
using BeYeuBookstore.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using BeYeuBookstore.Helpers;
using User = BeYeuBookstore.Data.Entities.User;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.Implementation.Acc;
using Microsoft.AspNetCore.Authorization;
using BeYeuBookstore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BeYeuBookstore.Extensions;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.Implementation;
using BeYeuBookstore.Application.Interfaces.Boq;
using BeYeuBookstore.Application.Implementation.Boq;
using Microsoft.AspNetCore.Http;

namespace BeYeuBookstore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //add
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});          

            // add migrations assembly khi chạy
            // lấy kết nối tới csdl the đường dẩn trong appsettings.json
            var connection =
            services.AddDbContext<ERPDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),mysqlOptions =>
                {
                    mysqlOptions
                        .ServerVersion(new Version(8, 0, 13), ServerType.MySql)
                        .CharSetBehavior(CharSetBehavior.AppendToAllColumns)
                        .AnsiCharSet(CharSet.Latin1)
                        .UnicodeCharSet(CharSet.Utf8mb4);
                }));

            
            services.AddIdentity<User, Role>()
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ERPDbContext>();
              
            //add BeYeuBookstore.Extensions (Nén response trả về)
            services.AddMinResponse();

            services.AddMemoryCache();

            //add path save dataProtection (fix eror Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException: The antiforgery token could not be decrypted.)
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"A_server\share\directory\"));

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(5);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

          

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(6);
                options.Cookie.HttpOnly = true;
            });

            //Set time out
            // https://stackoverflow.com/questions/45825684/asp-net-core2-0-user-auto-logoff-after-20-30-min?rq=1
            services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromSeconds(10));
            services.AddAuthentication()
                .Services.ConfigureApplicationCookie(options =>
                {
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(3);
                });
            //automapper
            services.AddAutoMapper();
            
            // Add application services.
            services.AddScoped<UserManager<User>, UserManager<User>>();
            services.AddScoped<RoleManager<Role>, RoleManager<Role>>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsPrincipalFactory>();

            // Cau hinh automapper cho dot.net core
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<DbInitializer>();

            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            //Repositories
         

            // khai bao cac service viet them ( voi interface <--> class)
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IAdvertiseContractRepository, AdvertiseContractRepository>();
            services.AddTransient<IAdvertiserRepository, AdvertiserRepository>();
            services.AddTransient<IAdvertisementContentRepository, AdvertisementContentRepository>();
            services.AddTransient<IAdvertisementPositionRepository, AdvertisementPositionRepository>();
            services.AddTransient<IBookCategoryRepository, BookCategoryRepository>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IDeliveryRepository, DeliveryRepository>();
            services.AddTransient<IInvoiceDetailRepository, InvoiceDetailRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IMerchantContractRepository, MerchantContractRepository>();
            services.AddTransient<IMerchantRepository, MerchantRepository>();
            services.AddTransient<IWebMasterRepository, WebMasterRepository>();
            services.AddTransient<IWebMasterTypeRepository, WebMasterTypeRepository>();
            services.AddTransient<IRatingDetailRepository, RatingDetailRepository>();
            services.AddTransient<IBooksInRepository, BooksInRepository>();
            services.AddTransient<IBooksOutRepository, BooksOutRepository>();
            services.AddTransient<IBooksOutDetailRepository, BooksOutDetailRepository>();
            services.AddTransient<IBooksInDetailRepository, BooksInDetailRepository>();

            services.AddTransient<IUserRolesRepository, UserRolesRepository>();
            services.AddMvc();
            services.AddTransient<IEmailService, EmailService>();

            //Services

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IAdvertiseContractService, AdvertiseContractService>();
            services.AddTransient<IAdvertiserService, AdvertiserService>();
            services.AddTransient<IAdvertisementContentService, AdvertisementContentService>();
            services.AddTransient<IAdvertisementPositionService, AdvertisementPositionService>();
            services.AddTransient<IBookCategoryService, BookCategoryService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IDeliveryService, DeliveryService>();
            services.AddTransient<IInvoiceDetailService, InvoiceDetailService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IMerchantContractService, MerchantContractService>();
            services.AddTransient<IMerchantService, MerchantService>();
            services.AddTransient<IWebMasterService, WebMasterService>();
            services.AddTransient<IWebMasterTypeService, WebMasterTypeService>();
            services.AddTransient<IFunctionService, FunctionService>();
            //services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IUserService,UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IRatingDetailService, RatingDetailService>();
            services.AddTransient<IBooksInService, BooksInService>();
            services.AddTransient<IBooksOutService, BooksOutService>();
            services.AddTransient<IBooksInDetailService, BooksInDetailService>();
            services.AddTransient<IBooksOutDetailService, BooksOutDetailService>();


            services.AddTransient<ISO_NotificationGeneralService, SO_NotificationGeneralService>();
            services.AddTransient<ISO_NotificationGeneralDetailService, SO_NotificationGeneralDetailService>();

            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            
            //add          
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 60
                    });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            }).AddViewLocalization(
                   LanguageViewLocationExpanderFormat.Suffix,
                   opts => { opts.ResourcesPath = "Resources"; })
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
               .AddDataAnnotationsLocalization()
               //.AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
               .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.Configure<RequestLocalizationOptions>(
              opts =>
              {
                  var supportedCultures = new List<CultureInfo>
                  {
                        new CultureInfo("en-US"),
                        new CultureInfo("vi-VN")
                  };

                  opts.DefaultRequestCulture = new RequestCulture("en-US");
                  // Formatting numbers, dates, etc.
                  opts.SupportedCultures = supportedCultures;
                  // UI strings that we have localized.
                  opts.SupportedUICultures = supportedCultures;
              });

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/ERP-{Date}.txt"); // ghi log
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
              
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseDatabaseErrorPage();
            app.UseStaticFiles();
            app.UseMinResponse();

            app.UseAuthentication();

            app.UseSession();
          
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller=BeyeuBookstore}/{action=Index}");

                routes.MapRoute(
                name: "Home",
                 template: "{controller=BeyeuBookstore}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "RouteAdmin",
                //   // template: "{area:exists}/{ controller = Home}/{ action = Index}/{ id ?}");
                //    template: "{area:exists}/{controller=login}/{action=Index}/{id?}");
            });
        }
    }
}
