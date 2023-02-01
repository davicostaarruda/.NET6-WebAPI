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