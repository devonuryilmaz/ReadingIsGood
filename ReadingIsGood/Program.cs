using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.API.Attributes;
using ReadingIsGood.API.Infastructure;
using ReadingIsGood.Data;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllers(x =>
{
    x.Filters.Add<AuthorizeAttribute>();
    x.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);

builder.Services.InstallServices(builder.Configuration);

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterAssemblyTypes(Assembly.Load(("ReadingIsGood.Business"))).Where(t => t.Namespace?.Contains("Service") == true)
       .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestQuery
                    | HttpLoggingFields.RequestMethod
                    | HttpLoggingFields.RequestPath
                    | HttpLoggingFields.RequestBody
                    | HttpLoggingFields.ResponseStatusCode
                    | HttpLoggingFields.ResponseBody;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.InstallServicesApp();

app.MapControllers();
app.UseHttpLogging();

app.Run();
