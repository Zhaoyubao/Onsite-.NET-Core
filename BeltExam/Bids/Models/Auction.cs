using System;
using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public class Auction : BaseEntity
    {
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Please input the product name!")]
        [MinLength(4, ErrorMessage = "Product name should be greater than 3!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please input the Description!")]
        [MinLength(11, ErrorMessage = "Description should be greater than 10!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please input the starting bid!")]
        public float Bid { get; set; }
        public string Bidder { get; set; }
        public byte IsEnded { get; set; } = 0;

        [Required(ErrorMessage = "Please input the end date!")]
        public DateTime EndDate { get; set; }
       
    }
}