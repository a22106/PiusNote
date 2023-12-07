// Program.cs
using Azure.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "PiusNoteVue.Server", 
        Version = "v1",
        Description = "A RESTful API for PiusNote. PiusNote is an application project that speech to text and summarize it.",
        TermsOfService = new Uri("https://note.piusdev.com/terms"), // TODO: Add terms of service
        Contact = new OpenApiContact {
            Name = "Pius Hwang",
            Email = "bk22106@piusdev.com",
        },
        License = new OpenApiLicense {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT"),
        },
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PiusNoteVue.Server v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
