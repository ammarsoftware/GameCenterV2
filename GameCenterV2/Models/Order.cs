using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCenterV2.Models
{
    public class Order
    {
        public List<TbService> Items { get; set; } = new List<TbService>();
        public double Subtotal => Items.Sum(item => item.SePrice??0 * item.SeQty??0);
        public double Discount { get; set; }
        public double Total => Subtotal - Discount;
    }
}
