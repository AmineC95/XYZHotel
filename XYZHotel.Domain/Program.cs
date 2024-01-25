using System.Text.Json.Serialization;
using XYZHotel.Domain.Conf;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(a => a.JsonSerializerOptions.PropertyNamingPolicy = new PascalCasePolicy())
    .AddJsonOptions(a => a.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.WithOrigins("http://localhost:9000", "https://localhost:7057")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
