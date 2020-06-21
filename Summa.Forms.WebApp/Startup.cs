using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Summa.Forms.WebApp.Services;

namespace Summa.Forms.WebApp
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
            services.AddAuthentication(options => { options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; })
                .AddCookie(options => { options.LoginPath = new PathString("/signin"); })
                .AddOpenIdConnect(options =>
                {
                    // Note: these settings must match the application details
                    // inserted in the database at the server level.
                    options.ClientId = "summa_forms_web";
                    options.ClientSecret = "nyseESlJCe36SJUuWWP68E5hIPNav0zpxOmGSW0yuIDG9kDW";

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    // Use the authorization code flow.
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

                    // Note: setting the Authority allows the OIDC client middleware to automatically
                    // retrieve the identity provider's configuration and spare you from setting
                    // the different endpoints URIs or the token validation parameters explicitly.
                    options.Authority = "https://localhost:5000/";

                    options.Scope.Add("openid");
                    options.Scope.Add("email");
                    options.Scope.Add("profile");
                    options.Scope.Add("roles");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("summa_forms_api");

                    options.SecurityTokenValidator = new JwtSecurityTokenHandler
                    {
                        // Disable the built-in JWT claims mapping feature.
                        InboundClaimTypeMap = new Dictionary<string, string>()
                    };

                    options.MaxAge = TimeSpan.FromSeconds(5);
                    options.UseTokenLifetime = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                    
                    options.AccessDeniedPath = "/";
                });

            services.AddControllersWithViews(options =>
            {
                // TODO: Awaiting credentials for Azure AD
                // var policy = new AuthorizationPolicyBuilder()
                //     .RequireAuthenticatedUser()
                //     .Build();
                // options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IFormProxyService, FormProxyService>();
            services.AddScoped<IQuestionProxyService, QuestionProxyService>();
            services.AddScoped<IRepositoryProxyService, RepositoryProxyService>();
            services.AddScoped<IResponseProxyService, ResponseProxyService>();
            services.AddScoped<ICategoryProxyService, CategoryProxyService>();
            services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5002"),
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}