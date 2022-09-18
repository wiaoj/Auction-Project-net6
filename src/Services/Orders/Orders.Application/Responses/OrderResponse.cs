using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Responses;
public class OrderResponse {
    public Guid Id { get; set; }
    public String AuctionId { get; set; }
    public String SellerUserName { get; set; }
    public String ProductId { get; set; }
    public Decimal UnitPrice { get; set; }
    public Decimal TotalPrice { get; set; }
    public DateTime CreateAt { get; set; }
}