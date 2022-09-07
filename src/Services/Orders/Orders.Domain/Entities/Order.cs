namespace Orders.Domain.Entities;
public class Order : EntityBase {
    public String AuctionId { get; set; }
    public String SellerUserName { get; set; }
    public String ProductId { get; set; }
    public Decimal UnitPrice { get; set; }
    public Decimal TotalPrice { get; set; }
    public DateTime CreateAt { get; set; }
}