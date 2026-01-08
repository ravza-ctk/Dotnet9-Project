using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApplication1.Core.Interfaces;
using WebApplication1.Data;
using WebApplication1.Data.Repositories;
using WebApplication1.Endpoints;
using WebApplication1.Middlewares;
using WebApplication1.Service.Mappings;
using WebApplication1.Service.Services;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);

// Add Services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1 API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, // Fixed from ApiModel
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

// Add Service Layer
builder.Services.AddServiceLayer(builder.Configuration);

// Data Layer Repositories (Can also move to Data layer DI if wanted, but keeping here or moving to ServiceLayer is fine. 
// Actually I moved Repositories registration to ServiceLayer? No, I didn't include them in the snippet above.
// Let's add Repositories here or modify the snippet above.
// I will keep Repositories here for now or adding them to the snippet in next step if needed. 
// Wait, I see I missed adding GenericRepository in the snippet above. 
// I should probably move GenericRepository registration to Data layer or keep it here.
// Let's keep Repositories here but remove the AutoMapper and Services lines.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value ?? "AvailableKeyForDemoUsesOnlyVeryLongPlease12345_MustBeAtLeast512BitsLongToWorkWithTheAlgorithmHmacSha512")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapCollectionEndpoints(); // Minimal API

// DB Migration at startup (Optional, good for demo)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    // Seed Data (Bonus Feature)
    await WebApplication1.Data.DataSeeder.SeedAsync(context);
}

app.Run();