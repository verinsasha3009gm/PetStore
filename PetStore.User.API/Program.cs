using PetStore.Users.API;
using PetStore.Users.DAL.DependencyInjection;
using PetStore.Users.Application.DependencyInjection;
using Serilog;
using PetStore.Users.Domain.Settings;
using System.Text.Json.Serialization;
using PetStore.Users.Consumer.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.Defaultsection));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(nameof(RedisSettings)));

builder.Services.AddControllers();
builder.Services.AddSwagger();

builder.Services.AddDataAccessAssembly(builder.Configuration);
builder.Services.AddDependencyInjection(builder.Configuration);

builder.Host.UseSerilog((context, configuration)
    => configuration.ReadFrom.Configuration(context.Configuration));
builder.Host.UseSerilog().ConfigureLogging(logging =>
{
    logging.AddSerilog();
    logging.SetMinimumLevel(LogLevel.Information);
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
//builder.Services.AddProducer();

//builder.Services.AddConsumer();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetStore Swagger v 1.0");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "PetStore Swagger v 2.0");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.UseHttpsRedirection();
app.MapControllers();
app.Run();
