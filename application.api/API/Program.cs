using API.Dependency;
using API.Middlewares;
using Application.Dependency;
using Application.Implementation;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.Services;
using Infrastructure.Dependency;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(
        options => options.WithOrigins(["*", "http://localhost:4200"]).AllowAnyMethod().AllowAnyHeader()
 );
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();
await app.PrepareDatabase();
app.Run();