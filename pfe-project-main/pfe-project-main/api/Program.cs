using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using PFE_PROJECT.Data;
using PFE_PROJECT.Repositories;
using PFE_PROJECT.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ➕ Controllers with JSON loop handling
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ➕ Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Gestion Utilisateurs & Équipements",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez le token comme : Bearer {votre_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// ➕ DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

// ➕ Authentification JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role
        };
    });

// ✅ CORS - updated to allow requests from Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",   // Angular CLI default
                "http://localhost:49296",  // Your custom dev port
                "http://localhost:51563"   // Your browser’s temporary dev port
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Needed if using cookies or Authorization header
    });
});

// ✅ Dependency Injection
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// ✅ Register password hasher (custom)
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// ➕ Services supplémentaires - Équipements
builder.Services.AddScoped<IMarqueService, MarqueService>();
builder.Services.AddScoped<ITypeEquipService, TypeEquipService>();
builder.Services.AddScoped<ICategorieService, CategorieService>();
builder.Services.AddScoped<IUniteService, UniteService>();
builder.Services.AddScoped<ICaracteristiqueService, CaracteristiqueService>();
builder.Services.AddScoped<IGroupeIdentiqueService, GroupeIdentiqueService>();
builder.Services.AddScoped<IReformeService, ReformeService>();
builder.Services.AddScoped<IOrganeService, OrganeService>();
builder.Services.AddScoped<IReaffectationService, ReaffectationService>();
builder.Services.AddScoped<IPretService, PretService>();

var app = builder.Build();

// ➤ Middleware order is important
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ CORS must come before auth
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "✅ API Gestion Utilisateurs + Équipements opérationnelle !");
app.Run();
