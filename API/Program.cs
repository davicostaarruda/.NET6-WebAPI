using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);


var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

app.MapPost("/products", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created($"/products/{product.Code}", product.Code);
});

app.MapGet("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetByCode(code);
    if(product != null)
        return Results.Ok(product);
    return Results.NotFound();
});

app.MapPut("products/{code}", (Product product) => {
    var productSaved = ProductRepository.GetByCode(product.Code);
    productSaved.Name = product.Name;
    return Results.Ok();
});

app.MapDelete("products/{code}", ([FromRoute] string code) => {
    var productSaved = ProductRepository.GetByCode(code);
    ProductRepository.DeleteByProduct(productSaved);
    return Results.Ok();
});

app.MapGet("/configuration/database", (IConfiguration configuration)=> {
    return Results.Ok($"{configuration["database:connection"]}/{configuration["database:Port"]}");
});

app.Run();
