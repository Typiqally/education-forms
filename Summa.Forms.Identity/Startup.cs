using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Summa.Forms.Identity.Data;
using Summa.Forms.Identity.Models;
using Summa.Forms.Identity.Services;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Summa.Forms.Identity
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
			services.AddMvc().AddRazorPagesOptions(options => { options.Conventions.AddPageRoute("/Home/Index", ""); });

			services.AddDbContext<ApplicationDbContext>(options =>
			{
				// Configure the context to use Microsoft SQL Server.
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

				// Register the entity sets needed by OpenIddict.
				// Note: use the generic overload if you need
				// to replace the default OpenIddict entities.
				options.UseOpenIddict();
			});

			// Register the Identity services.
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			// Configure Identity to use the same JWT claims as OpenIddict instead
			// of the legacy WS-Federation claims it uses by default (ClaimTypes),
			// which saves you from doing the mapping in your authorization controller.
			services.Configure<IdentityOptions>(options =>
			{
				options.ClaimsIdentity.UserNameClaimType = Claims.Name;
				options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
				options.ClaimsIdentity.RoleClaimType = Claims.Role;
			});

			services.AddOpenIddict()

				// Register the OpenIddict core components.
				.AddCore(options =>
				{
					// Configure OpenIddict to use the Entity Framework Core stores and models.
					options.UseEntityFrameworkCore()
						.UseDbContext<ApplicationDbContext>();
				})

				// Register the OpenIddict server components.
				.AddServer(options =>
				{
					// Enable the authorization, logout, token and userinfo endpoints.
					options.SetAuthorizationEndpointUris("/connect/authorize")
						.SetDeviceEndpointUris("/connect/device")
						.SetLogoutEndpointUris("/connect/logout")
						.SetTokenEndpointUris("/connect/token")
						.SetUserinfoEndpointUris("/connect/userinfo")
						.SetVerificationEndpointUris("/connect/verify");

					// Note: the Mvc.Client sample only uses the code flow and the password flow, but you
					// can enable the other flows if you need to support implicit or client credentials.
					options.AllowAuthorizationCodeFlow()
						.AllowDeviceCodeFlow()
						.AllowPasswordFlow()
						.AllowRefreshTokenFlow();

					// Mark the "email", "profile", "roles" and "demo_api" scopes as supported scopes.
					options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "summa_forms_api");

					// Register the signing and encryption credentials.
					options.AddDevelopmentEncryptionCertificate()
						.AddDevelopmentSigningCertificate();

					// Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
					options.UseAspNetCore()
						.EnableStatusCodePagesIntegration()
						.EnableAuthorizationEndpointPassthrough()
						.EnableLogoutEndpointPassthrough()
						.EnableTokenEndpointPassthrough()
						.EnableUserinfoEndpointPassthrough()
						.EnableVerificationEndpointPassthrough()
						.DisableTransportSecurityRequirement(); // During development, you can disable the HTTPS requirement.

					// Note: if you don't want to specify a client_id when sending
					// a token or revocation request, uncomment the following line:
					//
					// options.AcceptAnonymousClients();

					// Note: if you want to process authorization and token requests
					// that specify non-registered scopes, uncomment the following line:
					//
					// options.DisableScopeValidation();

					// Note: if you don't want to use permissions, you can disable
					// permission enforcement by uncommenting the following lines:
					//
					// options.IgnoreEndpointPermissions()
					//        .IgnoreGrantTypePermissions()
					//        .IgnoreScopePermissions();

					// Note: when issuing access tokens used by third-party APIs
					// you don't own, you can disable access token encryption:
					//
					options.DisableAccessTokenEncryption();
				})

				// Register the OpenIddict validation components.
				.AddValidation(options =>
				{
					// Configure the audience accepted by this resource server.
					// The value MUST match the audience associated with the
					// "demo_api" scope, which is used by ResourceController.
					options.AddAudiences("summa_forms_resources");

					// Import the configuration from the local OpenIddict server instance.
					options.UseLocalServer();

					// Register the ASP.NET Core host.
					options.UseAspNetCore();
				});

			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<ISmsSender, SmsSender>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
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
					"default",
					"{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}