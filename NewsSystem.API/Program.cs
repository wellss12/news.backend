using Microsoft.EntityFrameworkCore;
using NewsSystem.Infrastructure;
using MediatR;
using NewsSystem.API.Extensions;
using NewsSystem.Application;
using NewsSystem.Application.Command;
using NewsSystem.Domain;
using NewsSystem.Infrastructure.Config;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddSwaggerDoc();

builder.Services.AddCors();
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
builder.Services.AddJWTAuthentication(jwtOptions);
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));

builder.Services.AddMediatR(typeof(LoginCommandHandler).Assembly);
builder.Services.AddTransient<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddDbContext<IdDbContext>(opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetSection("DB:SqlServer:Connection").Value);
        opt.LogTo(Console.WriteLine, LogLevel.Information);
    }
);

builder.ConfigureIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();