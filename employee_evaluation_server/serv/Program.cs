using DataRepository;
using Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Configure services
void ConfigureServices(IServiceCollection services)
{

    // Add framework services.
    services.AddControllers();

    // Add configuration
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    services.AddSingleton(configuration);
}

// Add services to the container.
builder.Services.AddControllers();
ConfigureServices(builder.Services); // Call ConfigureServices method

builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
builder.Services.AddScoped<IEmployeeDAL, EmployeeDAL>();
builder.Services.AddScoped<IMeasureBL, MeasureBL>();
builder.Services.AddScoped<IMeasureDAL, MeasureDAL>();
builder.Services.AddScoped<IFileUploadBL, FileUploadBL>();
builder.Services.AddScoped<IFileUploadDAL, FileUploadDAL>();
builder.Services.AddScoped<IStatusBL, StatusBL>();
builder.Services.AddScoped<IStatusDAL, StatusDAL>();
builder.Services.AddScoped<ISchoolBL, SchoolBL>();
builder.Services.AddScoped<ISchoolDAL, SchoolDAL>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Run();
