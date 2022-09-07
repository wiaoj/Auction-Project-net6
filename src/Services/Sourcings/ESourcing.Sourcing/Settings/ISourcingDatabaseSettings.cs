namespace ESourcing.Sourcing.Settings;

public interface ISourcingDatabaseSettings {
    public String ConnectionString { get; set; }
    public String DatabaseName { get; set; }
}