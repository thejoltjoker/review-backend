using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Review.Api;
using Review.Api.Contexts;
using Review.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string databaseName = "ReviewDatabase";
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName));
builder.Services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase(databaseName));

builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    // TODO Remove lower password strength in dev
    if (builder.Environment.IsDevelopment())
    {
        options.Password.RequiredLength = 1;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    }
    // TODO set owasp password standards

    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
    identityContext.Database.EnsureCreated();

    // TODO remove before deployment
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    string email = builder.Configuration["SeedUser:Email"]!;
    string password = builder.Configuration["SeedUser:Password"]!;

    var existingUser = userManager.FindByEmailAsync(email);
    if (existingUser.Result == null)
    {
        User user = new User
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
        };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception(
                $"Failed to seed {email}:{password} | {string.Join(", ", result.Errors.Select(e => e.Description))}"
            );
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapIdentityApi<User>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();