using System.Net;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using UrlRedirectApp.Data;
using UrlRedirectApp.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapFallback(handler: async (ApplicationDbContext db, HttpContext ctx) =>
{
    var path = ctx.Request.Path.ToUriComponent().Trim('/');
    var result = await db.UrlMappings.Where(r => r.shortUrl == path).FirstOrDefaultAsync();
    if (result != null)
    {
        result.Clicks += 1;
        await db.SaveChangesAsync();
        ctx.Response.Redirect(result.LongUrl);
    }
});
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var ipAddress = context.Connection.RemoteIpAddress?.ToString();
    var userAgent = context.Request.Headers["User-Agent"].ToString();
    logger.LogInformation($"Incoming Request: IP {ipAddress} User Agent {userAgent} ");

    await next();

});
app.Run();
