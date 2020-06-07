using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDbGenericRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using webApiV1.Services;
using webApiV1.Models.Identity;

namespace webApiV1
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

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
            }));

            var mongoDbContext = new MongoDbContext(Configuration.GetValue<string>("ConnectionStrings:MongoDbDatabase"), Configuration.GetValue<string>("ConnectionStrings:DatabaseName"));
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(mongoDbContext)
                .AddDefaultTokenProviders();
            // Use the mongoDbContext for other things.
            services.AddSingleton<MongoDbContext>(mongoDbContext);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            services.AddAuthentication(options =>
            {
                //Set default Authentication Schema as Bearer
                options.DefaultAuthenticateScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters =
                       new TokenValidationParameters
                       {
                           ValidIssuer = Configuration["JwtIssuer"],
                           ValidAudience = Configuration["JwtIssuer"],
                           IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                           ClockSkew = TimeSpan.Zero // remove delay of token when expire
                       };
            });


            services.AddControllers();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("ApiCorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            creatRole(service);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void creatRole(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            string email = "peter@hd.net.nz";
            IEnumerable<string> roleNames = new string[] { "Administrator", "Customer", "Staff" };
            string roleName = "Administrator";
            string password = "Pass!!word1234";

            //Check that there is an Administrator role and create if not

            foreach (string role in roleNames)
            {
                Task<bool> hasAdminRole = roleManager.RoleExistsAsync(role);
                hasAdminRole.Wait();
                if (!hasAdminRole.Result)
                {
                    Task<IdentityResult> roleResult = roleManager.CreateAsync(new ApplicationRole(role));
                    roleResult.Wait();
                }
            }

            //Check if the admin user exists and create it if not
            //Add to the Administrator role

            Task<ApplicationUser> testUser = userManager.FindByEmailAsync(email);
            testUser.Wait();


            if (testUser.Result == null)
            {
                ApplicationUser administrator = new ApplicationUser();
                administrator.Email = email;
                administrator.UserName = email;

                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, password);
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, roleName);
                    newUserRole.Wait();
                }
            }
            else
            {
                Task<bool> testUserRole = userManager.IsInRoleAsync(testUser.Result, roleName);
                testUserRole.Wait();
                Console.WriteLine(testUserRole.Result);
                if (!testUserRole.Result)
                {
                    Task<IdentityResult> addtoRolse = userManager.AddToRoleAsync(testUser.Result, roleName);
                    addtoRolse.Wait();
                }
            }
        }
    }
}
