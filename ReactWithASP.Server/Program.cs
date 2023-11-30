using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactWithASP.Server.Data;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using ReactWithASP.Server.Helpers;
using AutoMapper;
using ReactWithASP.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Hackathon Application", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    option.OperationFilter<AuthResponsesOperationFilter>();
});

// Add db context

var dbConnection = builder.Configuration.GetConnectionString("DbString");

builder.Services.AddDbContext<DataContext>(options =>
{
    if (dbConnection != null)
    {
        options.UseNpgsql(dbConnection);
    }
});


builder.Services.AddAuthentication().AddCookie("default", o =>
{
    o.Cookie.Name = "Session_Token";
    //o.Cookie.Domain = "";
    o.Cookie.Path = "/";
    o.Cookie.HttpOnly = true;
    //o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    //o.Cookie.SameSite = SameSiteMode.Lax;
    o.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    o.SlidingExpiration = true;

});
/*
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
*/

builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("Admin-Policy", pb => pb.RequireAuthenticatedUser().RequireClaim(ClaimTypes.Role, "Admin"));
    builder.AddPolicy("User-Policy", pb => pb.RequireAuthenticatedUser().RequireClaim(ClaimTypes.Role, "User"));

});

var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<ControllerBase>>();
builder.Services.AddSingleton(typeof(ILogger), logger);

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfiles());
});

var mapper = config.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.SetIsOriginAllowed(host => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
));


// Declared Services
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<ConnectionStringsService>();
builder.Services.AddScoped<UserServices>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//app.MapControllers();

app.MapControllerRoute(name: "auth", pattern: "/auth/*");
app.MapControllerRoute(name: "cs", pattern: "/cs/*");
app.MapControllerRoute(name: "user", pattern: "/user/*");
app.MapControllerRoute(name: "external", pattern: "/external/*");

/*app.Use(async (context, next) =>
{
    var url = context.Request.Path.Value;

    // Rewrite to index
    if (url.Contains("/account/login"))
    {
        // rewrite and continue processing
        context.Response.Redirect("https://localhost:5173/account/login");
        return;
    }

    await next();
});
*/
app.MapFallbackToFile("/index.html");

app.Run();