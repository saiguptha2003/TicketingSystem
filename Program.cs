using Microsoft.EntityFrameworkCore;
using TicketBookingSystem.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(); 
// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext with SQLite connection
builder.Services.AddDbContext<TicketContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection" )));

// Register other services as needed

var app = builder.Build();

// Apply any pending migrations and create the database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TicketContext>();
    dbContext.Database.Migrate();  // This applies the migrations automatically
}

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