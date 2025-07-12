
using api.Data;
using api.Interfaces;
using api.models;
using api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStockRepository,StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
}).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(options =>
{
    // Définit le schéma d’authentification par défaut (JWT)
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    // Schéma utilisé quand un utilisateur non authentifié demande une ressource protégée
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    // Schéma utilisé quand l’utilisateur est authentifié mais pas autorisé (403)
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;

    // Schéma global par défaut
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    // Schéma utilisé pour la connexion (non utilisé avec JWT, mais requis par certains composants)
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;

    // Schéma utilisé pour la déconnexion
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Vérifie que l’émetteur du token est valide
        ValidateIssuer = true,

        // L’émetteur attendu (doit correspondre à celui du token)
        ValidIssuer = builder.Configuration["JWT:Issuer"],

        // Vérifie que l’audience est correcte
        ValidateAudience = true,

        // L’audience attendue
        ValidAudience = builder.Configuration["JWT:Audience"],

        // Vérifie que le token n’a pas été falsifié
        ValidateIssuerSigningKey = true,

        // Clé secrète utilisée pour vérifier la signature du token
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthentication();
app.MapControllers();

app.Run();

