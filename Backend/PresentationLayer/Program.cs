using System.Text;
using BusinessLayer.Dtos;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;
using BusinessLayer.Repositories;
using BusinessLayer.Services.ControllerServices;
using BusinessLayer.Services.DbServices;
using CoreLayer.Entities;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PresentationLayer.Middlewares;

//env
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

//cors policies
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
        

builder.Services.AddControllers();

//db context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//identity options
builder.Services.AddIdentityCore<AppUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//dependency injection
builder.Services.AddScoped(typeof(IGenericRepository<Post>), typeof(PostRepository));
builder.Services.AddScoped(typeof(IGenericDbService<PostDto>), typeof(PostDbService));
builder.Services.AddScoped(typeof(IGenericControllerService<PostDto>), typeof(PostControllerService));
builder.Services.AddScoped(typeof(IGenericRepository<AppUser>), typeof(UserRepository));
builder.Services.AddScoped(typeof(IGenericDbService<AppUserDto>), typeof(UserDbService));
builder.Services.AddScoped(typeof(IGenericControllerService<AppUserDto>), typeof(UserControllerService));
builder.Services.AddScoped(typeof(IGenericRepository<PostVote>), typeof(PostVoteRepository));
builder.Services.AddScoped(typeof(IGenericDbService<PostVoteDto>),  typeof(PostVoteDbService));
builder.Services.AddScoped(typeof(IGenericControllerService<PostVoteDto>), typeof(PostVoteControllerService));
builder.Services.AddScoped(typeof(IPostRepository), typeof(PostRepository));
builder.Services.AddScoped(typeof(IPostDbService), typeof(PostDbService));
builder.Services.AddScoped(typeof(IPostControllerService), typeof(PostControllerService));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IUserDbService), typeof(UserDbService));
builder.Services.AddScoped(typeof(IUserControllerService), typeof(UserControllerService));
builder.Services.AddScoped(typeof(IPostVoteControllerService),  typeof(PostVoteControllerService));
builder.Services.AddScoped(typeof(IPostVoteDbService),  typeof(PostVoteDbService));
builder.Services.AddScoped(typeof(IPostVoteControllerService), typeof(PostVoteControllerService));
builder.Services.AddScoped(typeof(IAuthRepository), typeof(AuthRepository));
builder.Services.AddScoped(typeof(IAuthDbService), typeof(AuthDbService));
builder.Services.AddScoped(typeof(IAuthControllerService), typeof(AuthControllerService));

//automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

//builder api test
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//global exception handler
app.UseMiddleware <GlobalExceptionHandlerMiddleware>();

//cors
app.UseCors("AllowAll");

//app api test
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
