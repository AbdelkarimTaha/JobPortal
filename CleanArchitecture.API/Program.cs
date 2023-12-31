using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.IServices;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Repository;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//register configuration
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                                                        {
                                                            options.SignIn.RequireConfirmedAccount = false;
                                                            options.SignIn.RequireConfirmedEmail = false;
                                                            options.Password.RequireLowercase = false;
                                                            options.Password.RequireUppercase = false;
                                                            options.Password.RequireDigit = false;
                                                            options.Password.RequireNonAlphanumeric = false;
                                                            options.Password.RequiredLength = 3;

                                                        }).AddEntityFrameworkStores<ApplicationDbContext>();

// ConfigureServices method
builder.Services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();


// add db service
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), _ => _.MigrationsAssembly("CleanArchitecture.Infrastructure")));



builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"])),
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Issuer"],
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };
    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };

});


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVacancyRepository, VacancyRepository>();
builder.Services.AddScoped<IVacancyService, VacancyService>();
builder.Services.AddScoped<IUserVacanciesRepository, UserVacanciesRepository>();
builder.Services.AddScoped<IRecurringJobService, RecurringJobService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();
app.UseHangfireServer();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

BackgroundJob.Enqueue(() => Console.WriteLine("Hello from Hangfire!"));

RecurringJob.AddOrUpdate<IRecurringJobService>(
            "Archiving Expired Vacancies",
            x => x.ArchivingExpiredVacancies(),
            Cron.Daily);

app.Run();
