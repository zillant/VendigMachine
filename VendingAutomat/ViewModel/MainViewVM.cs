using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using VendingMachine.Model;

namespace VendingAutomat.ViewModel
{
    public class MainViewVM : BindableBase
    {
        private User _user;
        private PurchaseManager _manager;
        private Automata _automata;

        //public int UserSumm { get; }

        public ObservableCollection<MoneyVM> UserWallet { get; }
        
        public ObservableCollection<ProductVM> UserBuyings { get; }
        
        public DelegateCommand GetChange { get; }
        
        public int Credit { get; }
        
        public ObservableCollection<MoneyVM> AutomataBank { get; }
        
        public ObservableCollection<ProductVM> ProductsInAutomata { get; }

        public MainViewVM()
        {
            _manager = new PurchaseManager();
            _user = _manager.User;
            _automata = _manager.Automata;

            _user.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(UserSumm)); };
            _automata.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Credit)); };


            UserWallet = new ObservableCollection<MoneyVM>(_user.UserWallet.Select(ms => new MoneyVM(ms)));

            UserBuyings = new ObservableCollection<ProductVM>(_user.UserBuyings.Select(ub => new ProductVM(ub)));

            ((INotifyCollectionChanged)_user.UserWallet).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count == 1) UserWallet.Add(new MoneyVM(a.NewItems[0] as MoneyStack));
                if (a.OldItems?.Count == 1) UserWallet.Remove(UserWallet.First(mv => mv.MoneyStack == a.OldItems[0]));
            };

            ((INotifyCollectionChanged)_user.UserBuyings).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count == 1) UserBuyings.Add(new ProductVM(a.NewItems[0] as ProductStack));
                if (a.OldItems?.Count == 1) UserBuyings.Remove(UserBuyings.First(mv => mv.ProductStack == a.OldItems[0]));
            };

            Watch(_user.UserWallet, UserWallet, um => um.MoneyStack);
            Watch(_user.UserBuyings, UserBuyings, ub => ub.ProductStack);


            AutomataBank = new ObservableCollection<MoneyVM>(_automata.AutomataBank.Select(a => new MoneyVM(a)));
            ProductsInAutomata = new ObservableCollection<ProductVM>(_automata.ProductsInAutomata.Select(ap => new ProductVM(ap)));


            Watch(_automata.ProductsInAutomata, ProductsInAutomata, p => p.ProductStack);
        }

        public int UserSumm => _user.UserSumm;

        private static void Watch<T,T2> (ReadOnlyObservableCollection<T> collToWatch, ObservableCollection<T2> collToUpdate, Func<T2, object> modelProperty)
        {
            ((INotifyCollectionChanged)collToWatch).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count == 1) collToUpdate.Add((T2)Activator.CreateInstance(typeof(T2), (T)a.NewItems[0]));
                if (a.OldItems?.Count == 1) collToUpdate.Remove(collToUpdate.First(mv => modelProperty(mv) == a.OldItems[0]));
            };
        }

    }

    public class ProductVM : BindableBase
    {
        public ProductStack ProductStack { get; }
        public ProductVM(ProductStack productStack, PurchaseManager manager = null)
        {
            ProductStack = productStack;
            productStack.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Amount)); };

            if (manager != null)
                BuyCommand = new DelegateCommand(() => {
                    manager.BuyProduct(ProductStack.Product);
                });
        }
        public Visibility IsBuyVisible => BuyCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public DelegateCommand BuyCommand { get; }
        public string Name => ProductStack.Product.Name;
        public string Price => $"({ProductStack.Product.Price} руб.)";
        public int Amount => ProductStack.Amount;
    }
    public class MoneyVM : BindableBase
    {
        public MoneyStack MoneyStack { get; }

        public MoneyVM(MoneyStack moneyStack, PurchaseManager manager = null)
        {
            MoneyStack = moneyStack;
            moneyStack.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Amount)); };

            if (manager != null)
                InsertCommand = new DelegateCommand(() => {
                    manager.InsertMoney(MoneyStack.Banknote);
                });
        }
        public Visibility IsInsertVisible => InsertCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public DelegateCommand InsertCommand { get; }
        public string Icon => MoneyStack.Banknote.IsCoin ? "..\\Images\\coin.png" : "..\\Images\\banknote.png";
        public string Name => MoneyStack.Banknote.Name;
        public int Amount => MoneyStack.Amount;
    }


}
