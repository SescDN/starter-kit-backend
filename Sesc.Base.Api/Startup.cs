using System; 
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Newtonsoft.Json.Linq;
using Sesc.Base.CrossCutting.IoC;
using Stefanini.Web.Filters;
using Microsoft.OpenApi.Models;

namespace Sesc.Base.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private Microsoft.Extensions.Hosting.IHostEnvironment Env { get; }
        readonly string DefaultOrigins = "DefaultOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<NotificationFilter>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: DefaultOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyOrigin();
                                      // builder.AllowCredentials();
                                  });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(jwtOptions =>
            {
                jwtOptions.Authority = System.Environment.GetEnvironmentVariable("JWT_AUTHORITY");
                jwtOptions.Audience = System.Environment.GetEnvironmentVariable("JWT_AUDIENCE");
                jwtOptions.SaveToken = true;
                jwtOptions.RequireHttpsMetadata = false;
                jwtOptions.IncludeErrorDetails = true;
                jwtOptions.MetadataAddress = System.Environment.GetEnvironmentVariable("JWT_METADATAADDRESS");

                jwtOptions.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        c.Response.WriteAsync(c.Exception.ToString()).Wait();
                        return Task.CompletedTask;
                    },
                    //OnTokenValidated = context =>
                    //{
                    //    MapKeycloakRolesToRoleClaims(context);
                    //    return Task.CompletedTask;
                    //},

                };
            });

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy("salvar", new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireAssertion(ctx => this.VerifyRoleByApp(ctx, new[] { "cadastro" }, new[] { "novo_cadastro" }))
                .Build());

                authorizationOptions.AddPolicy("editar", new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireAssertion(ctx => this.VerifyRoleByApp(ctx, new[] { "cadastro" }, new[] { "editar_cadastro" }))
                .Build());
            });


            services.AddControllers();

            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sesc DN - Starter Kit API Base", Version = "v1", });
           });
        }

        public bool VerifyRoleByApp(AuthorizationHandlerContext context, string[] appNames, string[] roleValues)
        {
            return context.User.HasClaim(it => it.Type == "resource_access" && it.ValueType == "JSON" && this.VerifyRoleInJsonClaimValue(it.Value, appNames, roleValues));
        }

        public bool VerifyRoleInJsonClaimValue(string claimValue, string[] appNames, string[] roleValues)
        {
            JObject value = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(claimValue);
            if (appNames?.Length > 0)
            {
                foreach (var appName in appNames)
                {
                    var prop = value[appName];
                    var roles = prop?["roles"];
                    if (roles != null)
                    {
                        if (roles.Any(it => roleValues.Contains(it.Value<string>())))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add any Autofac modules or registrations.
            // This is called AFTER ConfigureServices so things you
            // register here OVERRIDE things registered in ConfigureServices.
            //
            // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
            // when building the host or this won't be called.
            builder.RegisterModule(new ApplicationModule());
        }

        private void MapKeycloakRolesToRoleClaims(TokenValidatedContext context)
        {
            JObject resourceAccess = JObject.Parse(context.Principal.FindFirst("resource_access").Value);
            JToken clientResource = resourceAccess[context.Principal.FindFirstValue("azp")];
            ClaimsIdentity claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity == null || clientResource == null)
            {
                return;
            }
            var clientRoles = clientResource["roles"];

            foreach (var clientRole in clientRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, clientRole.ToString()));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            System.Environment.SetEnvironmentVariable("JWT_AUTHORITY", "http://keycloak.labs-sesc.com.br/auth/realms/sesc-dn");
            System.Environment.SetEnvironmentVariable("JWT_AUDIENCE", "cadastro");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(DefaultOrigins);

            app.UseAuthorization();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
