using PetStore.Markets.Application.DependencyInjection;
using PetStore.Markets.DAL.DependencyInjection;
using PetStore.Markets.API;
using Serilog;
using PetStore.Markets.Consumer.DependencyInjection;
using PetStore.Markets.Domain.Settings;
using PetStore.Markets.Producer.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
builder.Services.AddControllers();
builder.Services.AddRedis();
builder.Services.AddDependencyInjection();
builder.Services.AddAccessData(builder.Configuration);
builder.Services.AddSwagger();

builder.Host.UseSerilog((context, configuration)
    => configuration.ReadFrom.Configuration(context.Configuration));
builder.Host.UseSerilog().ConfigureLogging(logging =>
{
    logging.AddSerilog();
    logging.SetMinimumLevel(LogLevel.Information);
});

builder.Services.AddConsumer();
builder.Services.AddProducer();

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
