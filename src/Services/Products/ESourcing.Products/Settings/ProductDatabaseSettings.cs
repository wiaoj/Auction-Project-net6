namespace ESourcing.Products.Settings;

public class ProductDatabaseSettings : IProductDatabaseSettings {
	public String ConnectionString { get; set; }
	public String DatabaseName { get; set; }
	public String CollectionName { get; set; }
}