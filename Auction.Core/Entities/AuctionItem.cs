using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Core.Entities
{
    public class AuctionItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
        public string UserCreatedId { get; set; }

        //Relationship
        [ForeignKey(nameof(UserCreatedId))]
        public User CreatedUser { get; set; }
        public List<Bid> Bids { get; set; }
    }
}
