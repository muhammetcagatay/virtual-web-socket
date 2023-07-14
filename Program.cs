using virtual_web_socket.Extensions;
using virtual_web_socket.Handlers;
using virtual_web_socket.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<WebSocketHandler>();
builder.Services.AddSingleton<ConnectionManager>();


var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseWebSockets();

app.MapControllers();

app.CustomRoute<WebSocketMiddleware>("/ws/channel/{channelName}");

app.Run();
