namespace Ui.Frontend
{
	using System;
	using System.Linq;

	using Microsoft.AspNetCore.Authentication.OpenIdConnect;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Identity.Web;
	using Microsoft.Identity.Web.UI;

	public class Startup
	{
		#region constructors and destructors

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		#endregion

		#region methods

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
			app.UseEndpoints(
				endpoints =>
				{
					endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
					endpoints.MapRazorPages();
				});
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdB2C"));
			services.AddControllersWithViews();
			services.AddRazorPages()
				.AddMicrosoftIdentityUI();
		}

		#endregion

		#region properties

		public IConfiguration Configuration { get; }

		#endregion
	}
}
