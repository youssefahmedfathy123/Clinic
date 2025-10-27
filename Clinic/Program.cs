using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Seeding;
using Microsoft.AspNetCore.Identity;
using MyProject.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Database Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    await DefaultRolesAndUsersSeeder.SeedAsync(roleManager, userManager);
    await ApplicationSeeder.SeedAsync(context, userManager);
}

// Enable Swagger Everywhere ✅
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProject API V1");
    c.RoutePrefix = string.Empty; // Swagger على الرoot مباشرة
});

app.UseHttpsRedirection();

// Authentication ثم Authorization لو عندك Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
