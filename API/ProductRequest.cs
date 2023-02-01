public record ProductRequest{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<string> Tags { get; set; }
};