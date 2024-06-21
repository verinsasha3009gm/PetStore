using PetStore.Products.DAL.DependencyInjection;
using PetStore.Products.Application.DependencyInjection;
using PetStore.Products.API;
using PetStore.Products.API.Middlewares;
using PetStore.Products.Consumer.DependencyInjection;
using PetStore.Products.Producer.DependencyInjection;
using PetStore.Products.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
builder.Services.AddControllers();
builder.Services.AddSwagger();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddDataAccessLayer(builder.Configuration);

builder.Services.AddRedis();

builder.Services.AddProducer();

//builder.Host.UseSerilog((ctx, lc) => lc
//    .WriteTo.File("log.txt")
//    .WriteTo.Seq("http://localhost:5109"));
var app = builder.Build();

app.UseMiddleware<ExceptionHanlingMiddleware>();

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
