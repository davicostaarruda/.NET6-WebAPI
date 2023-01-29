using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();


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
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Tag 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }
}

public class Product 
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category {get; set;}
    public List<Tag> Tags {get; set;}
}

public class ApplicationDbContext : DbContext 
{
    public DbSet<Product> Products {get; set;}

protected override void OnModelCreating(ModelBuilder builder)
{
    builder.Entity<Product>()
        .Property(p => p.Description).HasMaxLength(500).IsRequired(false);
    builder.Entity<Product>()
        .Property(P => P.Name).HasMaxLength(120).IsRequired();
    builder.Entity<Product>()
        .Property(p => p.Code).HasMaxLength(20).IsRequired();
}
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@Sql2023;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES"); 
}
