using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecastData.Caching;
using WeatherForecastData.ExternalServices;
using WeatherForecastData.Repositories;
using WeatherForecastData.Settings;
using WeatherForecastData.Translations;

namespace WeatherForecastData
{
    public class Startup
    {
        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json")
        //        .AddEnvironmentVariables();

        //    if (env.IsDevelopment())
        //    {
        //        builder.AddJsonFile("appsettings.development.json");
        //    }

        //    Configuration = builder.Build();
        //}

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddOptions();

            //services.Configure<WeatherCacheSettings>(Configuration);
            //services.Configure<WeatherAPISettings>(Configuration.GetSection("WeatherAPISettings"));

            var configWeatherCacheSettings = new WeatherCacheSettings();
            Configuration.Bind("WeatherCacheSettings", configWeatherCacheSettings);
            services.AddSingleton(configWeatherCacheSettings);

            var configWeatherApiSettings = new WeatherAPISettings();
            Configuration.Bind("WeatherAPISettings", configWeatherApiSettings);
            services.AddSingleton(configWeatherApiSettings);

            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped(typeof(IWeatherDataCache), typeof(WeatherDataCache));
            services.AddScoped(typeof(IJsonParsor), typeof(JsonParsor));
            services.AddScoped(typeof(IWeatherService), typeof(WeatherService));
            services.AddScoped(typeof(IRegionsRepository), typeof(RegionsRepository));
            services.AddScoped(typeof(IWeatherRepository), typeof(WeatherRepository));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
