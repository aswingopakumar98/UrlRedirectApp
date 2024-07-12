using System.Net;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using UrlRedirectApp.Data;

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
    ctx.Response.Redirect(result.LongUrl);
});

app.Run();
