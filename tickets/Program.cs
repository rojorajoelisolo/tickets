using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using tickets.Application.Mappers;
using tickets.Application.Services;
using tickets.Domain.Contexts;
using tickets.Infrastructures.Repositories;
using tickets.Interfaces.Repositories;
using tickets.Interfaces.Services;
using UserEntity = tickets.Domain.Entities.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tickets", Version = "v1" });

    // Define the BearerAuth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }});
});

// Configure Entity Framework and Identity
builder.Services.AddDbContext<TicketsContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("tickets.WebApi")
    )
);

builder.Services.AddIdentity<UserEntity, IdentityRole>()
    .AddEntityFrameworkStores<TicketsContext>()
    .AddDefaultTokenProviders();

// Add authentication
//builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,    
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//builder.Services.AddIdentityCore<UserEntity>().AddEntityFrameworkStores<TicketsContext>().AddApiEndpoints();

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(typeof(TicketsProfile));

// Add scoped services.
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Add transient services and repositories.
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<ITaskStatusRepository, TaskStatusRepository>();
builder.Services.AddTransient<ITaskUserRepository, TaskUserRepository>(); 
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITaskService, TaskService>();

var app = builder.Build();

//app.MapIdentityApi<UserEntity>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tickets V1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
