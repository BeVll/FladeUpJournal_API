using FladeUp_Api.Data;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Interfaces;
using FladeUp_Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Text.Json.Serialization;
using System.Reflection;
using AutoMapper;
using FladeUp_Api.Mapper;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<AppEFContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionDB")));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Stores.MaxLengthForKeys = 128;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    
})
    .AddEntityFrameworkStores<AppEFContext>()
    .AddDefaultTokenProviders();

var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<String>("JwtSecretKey")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = signinKey,
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
builder.Services.AddSwaggerGen(c =>
{
    var fileDoc = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");

    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Jwt Auth header using the Bearer scheme",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Id="Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddAutoMapper(typeof(AppMapperProfile));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICloudStorageService, CloudStorageService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(conf =>
    conf.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
if (!Directory.Exists(dir))
    Directory.CreateDirectory(dir);

var dir_Avatars = Path.Combine(Directory.GetCurrentDirectory(), "images/user_avatars");
if (!Directory.Exists(dir_Avatars))
    Directory.CreateDirectory(dir_Avatars);

var dir_Headers = Path.Combine(Directory.GetCurrentDirectory(), "images/user_headers");
if (!Directory.Exists(dir_Headers))
    Directory.CreateDirectory(dir_Headers);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(dir),
    RequestPath = "/images"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.SeedData();
app.Run();
