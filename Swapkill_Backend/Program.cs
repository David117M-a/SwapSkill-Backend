using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swapkill_Backend.Brevo;
using Swapkill_Backend.Busisness;
using Swapkill_Backend.Firebase;
using Swapkill_Backend.Repositories;
using Swapkill_Backend.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register FirebaseService as a singleton
builder.Services.AddSingleton<FirebaseService>(provider =>
{
    IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
    string authSecret = configuration["FirebaseConfig:AuthSecret"];
    string basePath = configuration["FirebaseConfig:BasePath"];
    return new FirebaseService(authSecret, basePath);
});

builder.Services.AddSingleton<BrevoService>(provider =>
{
    IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
    string apiKey = configuration["BrevoConfig:ApiKey"];
    return new BrevoService(apiKey);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            IConfiguration configuration = builder.Configuration;
            string key = configuration["TokenKey"];
            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            options.TokenValidationParameters.ValidateIssuerSigningKey = true;
            options.TokenValidationParameters.ValidateIssuer = false;
            options.TokenValidationParameters.ValidateAudience = false;
            options.TokenValidationParameters.ClockSkew =  TimeSpan.Zero;
        });

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddScoped<IServicePostService, ServicePostService>();
builder.Services.AddScoped<IServicePostRepository, ServicePostRepository>();

builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllers();

app.Run();