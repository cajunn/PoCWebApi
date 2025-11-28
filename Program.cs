using Microsoft.EntityFrameworkCore;
using PoCWebApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// EF Core
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Controllers
builder.Services.AddControllers();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<
    Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider,
    PoCWebApi.Auth.Policies.DynamicPermissionPolicyProvider>();

builder.Services.AddScoped<
    Microsoft.AspNetCore.Authorization.IAuthorizationHandler,
    PoCWebApi.Auth.Policies.PermissionHandler>();

builder.Services.AddScoped<
    PoCWebApi.Auth.IAuthorizationServicePdp,
    PoCWebApi.Auth.AuthorizationServicePdp>();

// JWT Bearer auth (HS256 - same key as UI)
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var key = builder.Configuration["Jwt:SigningKey"]; // from user-secrets

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
            ClockSkew = TimeSpan.FromSeconds(15)
        };
    });

// Swagger + JWT "Authorize"
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Drive API",
        Version = "v1",
        Description = "Simple PoC API protected by a demo JWT"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT here (no quotes, no Bearer prefix)."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Swagger BEFORE auth
app.UseSwagger();

// Serve UI at /swagger and point to a RELATIVE JSON path
app.UseSwaggerUI(c =>
{
    // Absolute path to YOUR doc
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drive API v1");

    // Serve UI at /swagger (so /swagger and /swagger/index.html both work)
    c.RoutePrefix = "swagger";

    // Disable online validator (reduces confusing errors)
    c.ConfigObject.AdditionalItems["validatorUrl"] = null;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.MapRazorPages();

//  Area route (for Admin)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// for MVC controllers/views
app.MapControllerRoute(              
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
