namespace ESourcing.Sourcing.Settings;

public class SourcingDatabaseSettings : ISourcingDatabaseSettings {
    public String ConnectionString { get; set; }
    public String DatabaseName { get; set; }
}