using DirectoryMS.Application.Interfaces;
using DirectoryMS.Application.UseCases;
using DirectoryMS.Infrastructure.Repositories;
using DirectoryMS.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Hygea DirectoryMS API",
        Version = "v1",
        Description = "Directory microservice for user and fumigator approval management."
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<IUserUseCase, UserUseCase>();

var app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hygea DirectoryMS API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
