using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
namespace VendingMachine.Model
{
    public class Automata : BindableBase
    {
        private readonly ObservableCollection<MoneyStack> _automataBank;
        private ObservableCollection<ProductStack> _productsInAutomata;

        public ReadOnlyObservableCollection<MoneyStack> AutomataBank { get; }

        internal void InsertBanknote(Banknote banknote)
        {
            _automataBank.First(ms => ms.Banknote.Equals(banknote)).PushOne();
            Credit += banknote.Nominal;
        }

        internal bool BuyProduct(Product product)
        {
            if (Credit >= product.Price && _productsInAutomata.First(p => p.Product.Equals(product)).PullOne())
            {
                Credit -= product.Price;
                return true;
            }
            return false;
        }

        public ReadOnlyObservableCollection<ProductStack> ProductsInAutomata { get; }

        private int credit;
        public int Credit { get { return credit; } set { SetProperty(ref credit, value); } }

        public  Automata()
        {
            _automataBank = new ObservableCollection<MoneyStack>(Banknote.banknotes.Select(b => new MoneyStack(b, 100)));

            AutomataBank = new ReadOnlyObservableCollection<MoneyStack>(_automataBank);

            _productsInAutomata = new ObservableCollection<ProductStack>(Product.Products.Select(p => new ProductStack(p, 100)));

            ProductsInAutomata = new ReadOnlyObservableCollection<ProductStack>(_productsInAutomata);
        }
    }
}
