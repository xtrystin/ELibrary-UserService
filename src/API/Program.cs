using ELibrary_UserService.Application;
using ELibrary_UserService.Extensions;
using ELibrary_UserService.Infrastructure.EF;
using ELibrary_UserService.RabbitMq;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenCorsPolicy();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddPostgres(builder.Configuration);
builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddProviderCollection();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/error");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("OpenCorsPolicy");

app.UseMetricServer();
app.UseHttpMetrics(options => options.AddCustomLabel("host", context => context.Request.Host.Host));

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();
