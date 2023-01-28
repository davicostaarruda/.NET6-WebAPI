using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
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

public static class ProductRepository{
    public static List<Product> Products { get; set; } = new List<Product>();

public static void Init(IConfiguration configuration){
    var productsDefault = configuration.GetSection("Products").Get<List<Product>>();
    Products = productsDefault;
}
    public static void Add(Product product){
        Products.Add(product);
    }

    public static Product GetByCode(string code){
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void DeleteByProduct(Product product){
        Products.Remove(product);
    }
}

public class Product 
{
    public string Code { get; set; }
    public string Name { get; set; }
}

