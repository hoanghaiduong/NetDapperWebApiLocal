
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetDapperWebApi_local.Common.Filters;
using NetDapperWebApi_local.Common.Interfaces;

using NetDapperWebApi_local.Models;
using NetDapperWebApi_local.Services;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local
{
    public static class DependencyInjection
    {

        private static readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddStackExchangeRedisCache(c =>
            {
                c.Configuration = configuration.GetConnectionString("Redis") ?? "localhost:6379";
                c.InstanceName = "NetDapperWebApi_local_";
            });
            // services.AddOutputCache(options=>{
            //     options.AddBasePolicy(builder=>builder.Expire(TimeSpan.FromSeconds(60))); 
            // });
            services.AddScoped<IDbConnection>(cnn => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<IRoomService, RoomService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            services.AddScoped<IAmenitiesService, AmenitiesService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IServiceServices, ServiceServices>();
            services.AddScoped<IServiceUsageService, ServiceUsageService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<IServiceTypeService, ServiceTypeService>();
            // services.AddFluentValidationAutoValidation();



            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                policy =>
                                {
                                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                                }

                                );
            });
            services.AddLogging(s =>
            {
                s.AddConsole();
                s.AddDebug();

            });
            services.AddControllers();
            return services;
        }

        public static IServiceCollection AddSwaggerExplorers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen((configure) =>
              {
                  configure.SchemaFilter<EnumSchemaFilter>();

                  configure.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                  {
                      Name = "JWT Authentication",
                      In = ParameterLocation.Header,
                      Type = SecuritySchemeType.Http,
                      Scheme = "Bearer",
                      Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer ey...')",
                  });
                  configure.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                {
                     new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                  });
              });

            return services;
        }
        public static IServiceCollection AddAuthentications(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;// lưu token
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["AppSettings:Issuer"],
                    ValidAudience = configuration["AppSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:TokenSecret"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization();
            return services;
        }
        public static IServiceCollection AddCustomModelStateValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    var apiResponse = new ApiResponse<object>(
                        success: false,
                        data: null,
                        message: "One or more validation errors occurred.",
                        errors: errors
                    );

                    return new BadRequestObjectResult(apiResponse)
                    {
                        ContentTypes = { "application/json" }
                    };
                };

            });

            return services;
        }

        public static WebApplication UseCustomExtensions(this WebApplication app)
        {
            app.UseCors(MyAllowSpecificOrigins);
            //     app.UseExceptionHandler(config =>
            //  {
            //      //tối ưu hơn
            //      config.Run(async context =>
            //      {
            //          var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            //          var exception = exceptionHandlerPathFeature.Error;
            //          var message = exception.Message;
            //          var statusCode = 500;
            //          if (exception is SqlException)
            //          {
            //              statusCode = 400;
            //          }
            //          var response = new { message, statusCode };
            //          var result = JsonSerializer.Serialize(response);
            //          context.Response.ContentType = "application/json";
            //          context.Response.StatusCode = statusCode;
            //          await context.Response.WriteAsync(result);
            //      });
            //  });
            app.UseStaticFiles();
            return app;
        }
        public static WebApplication UseAuthentications(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}