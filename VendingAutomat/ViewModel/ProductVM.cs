using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VendingMachine.Model;

namespace VendingAutomat.ViewModel
{
    public class ProductVM
    {
        public ProductStack ProductStack { get; }

        public ProductVM(ProductStack productStack)
        {
            ProductStack = productStack;
        }
        public Visibility IsBuyVisible => BuyCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public DelegateCommand BuyCommand { get; }
        public string Name => ProductStack.Product.Name;
        public string Price => $"({ProductStack.Product.Price} rub)";

        public Visibility isAmountVisible => BuyCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public int Amount => ProductStack.Amount;
    }
}
