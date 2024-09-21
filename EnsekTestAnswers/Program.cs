using EnsekTestAnswers.Data;
using EnsekTestAnswers.Repositories;
using EnsekTestAnswers.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IAccountDataSource, CsvAccountDataSource>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
builder.Services.AddScoped<IMeterReadingsUploadService, MeterReadingsUploadService>();
builder.Services.AddScoped<IMeterReadingValidatorService, MeterReadingValidatorService>();
builder.Services.AddScoped<IMeterReadingValidationRule, AccountValidationRule>();
builder.Services.AddScoped<IMeterReadingValidationRule, MeterReadingValueValidationRule>();
builder.Services.AddScoped<IMeterReadingValidationRule, DuplicateReadingValidationRule>();



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowAllOrigins");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await Seed.SeedDatabaseAsync(dbContext);
}

app.Run();
