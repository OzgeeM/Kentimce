using Kentimce.Application;
using Kentimce.Infrastructure;
using Kentimce.Persistence;
using Kentimce.WebApi.Middlewares;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//servisleri ekle
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5174", "http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. Middleware S?ralamas?
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication(); // Kimlik Dogrulama
app.UseAuthorization();  // Yetkilendirme
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
