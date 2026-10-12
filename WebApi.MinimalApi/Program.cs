using System.Buffers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApi.MinimalApi.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");
builder.Services.AddControllers(options =>
    {
        options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
        options.OutputFormatters.Insert(0, new 
            NewtonsoftJsonOutputFormatter(new JsonSerializerSettings
            {
                ContractResolver = new 
                    CamelCasePropertyNamesContractResolver()
            }, ArrayPool<char>.Shared, options));
        options.ReturnHttpNotAcceptable = true;
        options.RespectBrowserAcceptHeader = true;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
    });

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
var app = builder.Build();

app.MapControllers();

app.Run();