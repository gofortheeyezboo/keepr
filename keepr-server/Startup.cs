using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using keepr_server.Repositories;
using keepr_server.Services;

namespace keepr_server
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

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "partyplanner", Version = "v1" });
      });

      //NOTE this is required for using auth
      services.AddAuthentication(options =>
             {
               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             }).AddJwtBearer(options =>
             {
               options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
               options.Audience = Configuration["Auth0:Audience"];
             });

      //NOTE this is for communicating with client
      services.AddCors(options =>
        {
          options.AddPolicy("CorsDevPolicy", builder =>
              {
                builder
                          .WithOrigins(new string[]{
                            "http://localhost:8080",
                            "http://localhost:8081"
                          })
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
              });
        });
      //NOTE this is required to connect to DB
      services.AddScoped<IDbConnection>(x => CreateDbConnection());

      services.AddTransient<ProfileService>();
      services.AddTransient<ProfileRepository>();

      services.AddTransient<KeepService>();
      services.AddTransient<KeepRepository>();

      services.AddTransient<VaultService>();
      services.AddTransient<VaultRepository>();

      services.AddTransient<VaultKeepService>();
      services.AddTransient<VaultKeepRepository>();




    }
    //NOTE also required for connecting to db
    private IDbConnection CreateDbConnection()
    {
      var connectionString = Configuration["db:gearhost"];
      return new MySqlConnection(connectionString);
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "partyplanner v1"));
        //NOTE needed to access from client
        app.UseCors("CorsDevPolicy");
      }

      app.UseHttpsRedirection();

      app.UseRouting();
      //NOTE must have so server can auth
      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}