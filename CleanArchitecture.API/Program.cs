using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.IServices;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Repository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//register configuration
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


// add db service
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), _ => _.MigrationsAssembly("CleanArchitecture.Infrastructure")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDataProtection();
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"\\server\share\directory\"));

builder.Services.AddDataProtection()
    .ProtectKeysWithDpapi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
