using HSG.RestaurantTableBooking.Data;
using HSG.RestaurantTableBookingApp.API;
using HSG.RestaurantTableBookingApp.Service;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;

namespace HSG.RestaurantTableBooking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
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


                // Add services to the container.
                builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
                builder.Services.AddScoped<IRestaurantService, RestaurantService>();

                builder.Services.AddDbContext<RestaurantTableBookingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbContext") ?? "").EnableSensitiveDataLogging() //should not be used in production, only for development purpose
                );

                builder.Services.AddControllers();
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