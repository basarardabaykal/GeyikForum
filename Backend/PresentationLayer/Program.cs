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
using DotNetEnv;

//env
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


//dependency injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IGenericRepository<Post>), typeof(PostRepository));
builder.Services.AddScoped(typeof(IGenericDbService<PostDto>), typeof(PostDbService));
builder.Services.AddScoped(typeof(IGenericControllerService<PostDto>), typeof(PostControllerService));
builder.Services.AddScoped(typeof(IPostRepository), typeof(PostRepository));
builder.Services.AddScoped(typeof(IPostDbService), typeof(PostDbService));
builder.Services.AddScoped(typeof(IPostControllerService), typeof(PostControllerService));

//automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//api test
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
