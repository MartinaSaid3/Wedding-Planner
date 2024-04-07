
using Business_Logic_Layer.Service.AccountServices;
using Business_Logic_Layer.Service.ReservationService;
using Business_Logic_Layer.Service.VenueService;
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.AccountRepo;
using Data_Access_Layer.Repo.ReservationRepo;
using Data_Access_Layer.Repo.VenueRepo;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Business_Logic_Layer.Service.EmailService;
using Business_Logic_Layer.Service.AdminServices;
using Data_Access_Layer.Repo.AdminRepo;
using Business_Logic_Layer.Service.RateService;
using Data_Access_Layer.Repo.RateRepo;

namespace Wedding_Planner_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddDbContext<ApplicationEntity>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<ApplicationEntity>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationEntity>()
                .AddDefaultTokenProviders();
            builder.Services.AddCors();
            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //options.SaveToken = true;
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidateIssuerSigningKey = true,
                    //ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    //ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });

            // Add UserManager service
            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            builder.Services.AddScoped<IReservationBLL, ReservationBLL>();
            builder.Services.AddScoped<IReservationDAL, ReservationDAL>();
            builder.Services.AddScoped<IVenueDAL, VenueDAL>();
            builder.Services.AddScoped<IVenueBLL, VenueBLL>();
            builder.Services.AddScoped<IAccountBLL, AccountBLL>();
            builder.Services.AddScoped<IAccountDAL, AccountDAL>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IAdminBLL, AdminBLL>();
            builder.Services.AddScoped<IAdminDAL, AdminDAL>();
            builder.Services.AddScoped<IRateDAL, RateDAL>();
            builder.Services.AddScoped<IRateBLL, RateBLL>();


            builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHangfireServer();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Configure JWT bearer authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Add JWT bearer token support to Swagger UI
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
            });

            //cloudinary
            builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));

            builder.Services.AddScoped<IPhotoServices, PhotoServices>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            // Enable authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/dashboard");

            app.MapControllers();

            app.Run();
        }
    }
}
