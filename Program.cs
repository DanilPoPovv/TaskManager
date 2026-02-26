using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplication1.Authorization;
using WebApplication1.EntityFramework;
using WebApplication1.Filter;
using WebApplication1.Helpers;
using WebApplication1.Middlewares;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddJwtAuthenticationSchemes();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.AddSwaggerJwtBearer();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExecutionTimeFilter>();
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
