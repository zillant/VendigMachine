using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace VendingMachine.Model
{
    public class User : BindableBase
    {
        public int UserSumm{ get { return 
                    _userWallet.Select(b => b.Banknote.Nominal * b.Amount).Sum(); } }

        public ReadOnlyObservableCollection<MoneyStack> UserWallet { get; }

        internal bool GetBanknote(Banknote banknote)
        {
            if (_userWallet.FirstOrDefault(ms => ms.Banknote.Equals(banknote))?.PullOne() ?? false)
            {
                RaisePropertyChanged(nameof(UserSumm));
                return true;
            }
            return false;
        }

        internal void AddProduct(Product product)
        {
            var stack = _userBuyings.FirstOrDefault(b => b.Product == product);
            if (stack == null)
                _userBuyings.Add(new ProductStack(product, 1));
            else
                stack.PushOne();
        }

        private readonly ObservableCollection<MoneyStack> _userWallet;

        public User()
        {
            _userWallet = new ObservableCollection<MoneyStack>(Banknote.banknotes.Select(b => new MoneyStack(b, 10)));
            UserWallet = new ReadOnlyObservableCollection<MoneyStack>(_userWallet);
            UserBuyings = new ReadOnlyObservableCollection<ProductStack>(_userBuyings);
        }

        public ReadOnlyObservableCollection<ProductStack> UserBuyings;
        private readonly ObservableCollection<ProductStack> _userBuyings = new ObservableCollection<ProductStack>();
    }
}
