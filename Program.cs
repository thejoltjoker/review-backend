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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddProblemDetails();

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

builder.Services.AddAuthentication().AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
    ApiKeyConstants.ApiKeyName,
    options => { options.DisplayMessage = "Api key test"; });


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
    const string userId = "USER0-0000-0000-0000-000000000001";
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();


    // TODO remove before deployment
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    string email = builder.Configuration["SeedUser:Email"]!;
    string password = builder.Configuration["SeedUser:Password"]!;

    User? seededUser = await userManager.FindByEmailAsync(email);
    if (seededUser == null)
    {
        User user = new User
        {
            Id = userId,
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

        seededUser = user;
    }
    
    
    // Seed at runtime instead of HasData to be able to make relationship to user
    DateTime seedCreatedAt = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    const string asset1Id = "ASSET000-0000-0000-0000-000000000001";
    const string asset2Id = "ASSET000-0000-0000-0000-000000000002";
    const string comment1Id = "COMMENT0-0000-0000-0000-000000000001";
    const string comment2Id = "COMMENT0-0000-0000-0000-000000000002";
    const string project1Id = "PROJECT0-0000-0000-0000-000000000001";
    const string project2Id = "PROJECT0-0000-0000-0000-000000000002";
    
    if (!await context.Projects.AnyAsync(p => p.Id == project1Id))
    {
        context.Projects.Add(new Project("The Code Awakens")
        {
            Id = project1Id,
            CreatedAt = seedCreatedAt
        });
    }
    if (!await context.Projects.AnyAsync(p => p.Id == project2Id))
    {
        context.Projects.Add(new Project("Ctrl+Alt+Delight")
        {
            Id = project2Id,
            CreatedAt = seedCreatedAt
        });
    }

    await context.SaveChangesAsync();

    var project1 = await context.Projects.FirstAsync(p => p.Id == project1Id);
    if (project1.Users.All(u => u.Id != seededUser.Id))
    {
        project1.Users.Add(seededUser);
    }
    var project2 = await context.Projects.FirstAsync(p => p.Id == project2Id);
    if (project2.Users.All(u => u.Id != seededUser.Id))
    {
        project2.Users.Add(seededUser);
    }
    
    if (!await context.Assets.AnyAsync(a => a.Id == asset1Id))
    {
        context.Assets.Add(new Asset
        {
            Id = asset1Id,
            FileName = "kickoff-notes.mp4",
            FileUrl = "https://cdn.review.local/assets/kickoff-notes.mp4",
            FileType = "video/mp4",
            ProjectId = project1Id,
            UserId = seededUser.Id,
            CreatedAt = seedCreatedAt
        });
    }
    if (!await context.Assets.AnyAsync(a => a.Id == asset2Id))
    {
        context.Assets.Add(new Asset
        {
            Id = asset2Id,
            FileName = "retro-summary.pdf",
            FileUrl = "https://cdn.review.local/assets/retro-summary.pdf",
            FileType = "application/pdf",
            ProjectId = project2Id,
            UserId = seededUser.Id,
            CreatedAt = seedCreatedAt
        });
    }
    if (!await context.Comments.AnyAsync(c => c.Id == comment1Id))
    {
        context.Comments.Add(new Comment
        {
            Id = comment1Id,
            Content = "Great onboarding section, very clear.",
            TimestampSeconds = 18.5f,
            AssetId = asset1Id,
            UserId = seededUser.Id,
            CreatedAt = seedCreatedAt
        });
    }
    if (!await context.Comments.AnyAsync(c => c.Id == comment2Id))
    {
        context.Comments.Add(new Comment
        {
            Id = comment2Id,
            Content = "Please add a short decision summary at the end.",
            TimestampSeconds = 0f,
            AssetId = asset2Id,
            UserId = seededUser.Id,
            CreatedAt = seedCreatedAt
        });
    }
    
    await context.SaveChangesAsync();
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

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<User>();
app.MapControllers();

app.Run();