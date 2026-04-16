using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Review.Api.Contexts;
using Review.Api.Handlers;
using Review.Api.Models;
using Review.Api.Repositories;
using Review.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IApiKeySecretHasher, ApiKeySecretHasher>();

builder.Services.AddAutoMapper(options =>
{
    options.LicenseKey = builder.Configuration["AutoMapper:ApiKey"];
    options.AddMaps(typeof(Program));
});

const string databaseName = "ReviewDatabase";

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName));

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication().AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyConstants.ApiKeyName,
    options =>
    {
        options.DisplayMessage = "Api key test";
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BearerOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(
            IdentityConstants.BearerScheme
        );
    });

    options.AddPolicy("ApiKeyOrUser", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(
            ApiKeyConstants.ApiKeyName,
            IdentityConstants.BearerScheme
        );
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();


    // TODO remove before deployment
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    string email = builder.Configuration["SeedUser:Email"]!;
    string password = builder.Configuration["SeedUser:Password"]!;

    var existingUser = userManager.FindByEmailAsync(email);
    if (existingUser.Result == null)
    {
        User user = new User
        {
            Id = "USER0-0000-0000-0000-000000000001",
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