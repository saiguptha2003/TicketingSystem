using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using TicketBookingSystem.Models;
using System.Text;
using Microsoft.OpenApi.Models;
using TicketBookingSystem;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger generation and configure JWT Bearer security
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add Security Definition for JWT Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
Enter 'Bearer' [space] and then your token in the text input below
\r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add Security Requirement to require JWT Bearer token in the requests
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext with SQLite connection
builder.Services.AddDbContext<TicketContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com", // Change this to your actual issuer
            ValidAudience = "yourdomain.com", // Change this to your actual audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("lYifrI8MCHI9CWYWphTT3wiFpiPvh6AD3EcorB4k9jxKktLGursWeM4AdTN0UoayAlDuj0NpE8FGBW38cpKZXavv75Oj8ZffvNI3FAN0poxPmGsPJdptwBQJkn6Ym5kt")) // Replace with your actual secret key
        };
    });

// Register Authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

// Apply any pending migrations and create the database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TicketContext>();
    dbContext.Database.Migrate();  // This applies the migrations automatically
    // DataSeeder.SeedData(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable Authentication middleware
app.UseAuthentication();

// Enable Authorization middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
