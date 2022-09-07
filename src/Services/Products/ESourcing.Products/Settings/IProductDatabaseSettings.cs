namespace ESourcing.Products.Settings;

public interface IProductDatabaseSettings {
	public String ConnectionString { get; set; }
	public String DatabaseName { get; set; }
	public String CollectionName { get; set; }
}