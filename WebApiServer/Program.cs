using WebApiServer.Data;
using WebApiServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddSignalR();

builder.Services.AddLogging();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var loggerString = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MessageRepository>>();
//Scoped vs Singleton
builder.Services.AddSingleton<MessageRepository>(mr => new MessageRepository(connectionString, loggerString));

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

DataBaseInitializer.Initialize(builder.Configuration.GetConnectionString("DefaultConnection"));

app.UseHttpsRedirection();

app.MapHub<MessageHub>("/messageHub");

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();
