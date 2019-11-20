using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Model
{
    public class ProductStack
    {
        public Product Product { get; }
        public int Amount { get; }
        public ProductStack(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

       
    }
}
