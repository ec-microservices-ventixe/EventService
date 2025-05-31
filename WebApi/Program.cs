using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data.Context;
using WebApi.Data.Interfaces;
using WebApi.Data.Repositories;
using WebApi.Interfaces;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKeyResolver = (token, SecurityKey, kid, validationParameters) =>
            {
                HttpClient httpClient = new();
                string jwksJson = httpClient.GetStringAsync(builder.Configuration["Jwt:jwksUrl"]).Result;
                var keys = new JsonWebKeySet(jwksJson).Keys;
                return keys.Where(k => k.Kid == kid).ToList();
            }
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
builder.Services.AddScoped<IScheduleSlotRepository, ScheduleSlotRepository>();
builder.Services.AddScoped<IEventPackageRepository, EventPackageRepository>();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IScheduleSlotService, ScheduleSlotService>();
builder.Services.AddScoped<IPackageService, PackageService>();

builder.Services.AddScoped<EventInfoGrpcService>();

builder.Services.AddScoped<IFileService, AzureFilesService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapOpenApi();

app.UseSwagger();                              
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty;       
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
