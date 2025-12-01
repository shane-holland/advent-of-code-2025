using AdventOfCode.ApiService.Services;
using AdventOfCode.Solutions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Advent of Code 2025 API",
        Version = "v1",
        Description = "REST API for solving Advent of Code 2025 puzzles with automatic caching and solution discovery",
        Contact = new OpenApiContact
        {
            Name = "Advent of Code",
            Url = new Uri("https://adventofcode.com/2025")
        }
    });

    // Add security definition for session cookie
    options.AddSecurityDefinition("Session", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Session",
        Description = "Advent of Code session cookie (get from https://adventofcode.com after logging in)"
    });

    // Add security requirement for endpoints that need it
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Session"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Register application services
builder.Services.AddSingleton<SolutionRegistry>();
builder.Services.AddSingleton<CacheService>();
builder.Services.AddScoped<PuzzleRunner>();
builder.Services.AddHttpClient<AdventOfCodeClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Advent of Code 2025 API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at root
        options.DocumentTitle = "Advent of Code 2025 API";
        options.EnableTryItOutByDefault();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
