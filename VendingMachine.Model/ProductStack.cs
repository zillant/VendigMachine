using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace VendingMachine.Model
{
    public class ProductStack:BindableBase
    {
        private int _amount;

        public Product Product { get; }
        
        public ProductStack(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

        public int Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        internal bool PullOne()
        {
            if (Amount > 0)
            {
                --Amount;
                return true;
            }
            return false;
        }
        internal void PushOne() => ++Amount;
    }
}
