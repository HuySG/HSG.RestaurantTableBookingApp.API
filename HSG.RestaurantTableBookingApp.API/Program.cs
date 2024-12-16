using HSG.RestaurantTableBooking.Data;
using HSG.RestaurantTableBookingApp.API;
using HSG.RestaurantTableBookingApp.Service;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System.Net;
using System.Text.Json.Serialization;

namespace HSG.RestaurantTableBooking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog with the settings
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Debug()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .CreateBootstrapLogger();
			try
			{
                var builder = WebApplication.CreateBuilder(args);
                var configuration = builder.Configuration;

                builder.Services.AddApplicationInsightsTelemetry();

                builder.Host.UseSerilog((context, services, LoggerConfiguration) => LoggerConfiguration
                .WriteTo.ApplicationInsights(
                    services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Events));

                Log.Information("Strating the application....");

              //  Adds Microsoft Identity platform(AAD v2.0) support to protect this Api
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(options =>

                        {
                            configuration.Bind("AzureAdB2C", options);
                            options.Events = new JwtBearerEvents();

                            /// <summary>
                            /// Below you can do extended token validation and check for additional claims, such as:
                            ///
                            /// - check if the caller's account is homed or guest via the 'acct' optional claim
                            /// - check if the caller belongs to right roles or groups via the 'roles' or 'groups' claim, respectively
                            ///
                            /// Bear in mind that you can do any of the above checks within the individual routes and/or controllers as well.
                            /// For more information, visit: https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data
                            /// </summary>

                            //options.Events.OnTokenValidated = async context =>
                            //{
                            //    string[] allowedClientApps = { /* list of client ids to allow */ };

                            //    string clientAppId = context?.Principal?.Claims
                            //        .FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

                            //    if (!allowedClientApps.Contains(clientAppId))
                            //    {
                            //        throw new System.Exception("This client is not authorized");
                            //    }
                            //};
                        }, options => { configuration.Bind("AzureAdB2C", options); });
                IdentityModelEventSource.ShowPII = false;

                // Add services to the container.
                builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
                builder.Services.AddScoped<IRestaurantService, RestaurantService>();
                builder.Services.AddScoped<IReservationService, ReservationService>();
                builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
                builder.Services.AddScoped<IEmailNotification, EmailNotification>();

                builder.Services.AddDbContext<RestaurantTableBookingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbContext") ?? "").EnableSensitiveDataLogging() //should not be used in production, only for development purpose
                );

                builder.Services.AddControllers().AddJsonOptions(options => 
                { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
                // In production, modify this with the actual domains you want to allow
                builder.Services.AddCors(o => o.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();



                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception,"Unhandle exception occured. {ExceptionDetails}", exception?.ToString());
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occured. Please try again later");
                    });
                });
                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                // Configure the HTTP request pipeline.
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API V1");
                    c.RoutePrefix = "swagger"; // Đặt Swagger UI tại /swagger
                });

                app.UseCors("default");
                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
			catch (Exception ex)
			{

                Log.Fatal(ex, "Host terminated unexpectedly");
			}
            finally { 
                Log.CloseAndFlush();
            }
        }
    }
}