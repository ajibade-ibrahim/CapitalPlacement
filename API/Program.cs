using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();
var localConnectionString = config["LocalCosmosConnectionString"];

builder.Services.AddDbContextFactory<ProgramContext>(optionsBuilder =>
  optionsBuilder
    .UseCosmos(
      connectionString: localConnectionString,
      databaseName: "ProgramsDb",
      cosmosOptionsAction: options =>
      {
        options.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Gateway);
      }));
builder.Services.AddTransient<ProgramsService>();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
