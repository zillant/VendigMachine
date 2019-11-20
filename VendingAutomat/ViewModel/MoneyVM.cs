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
    public class MoneyVM
    {
        public Visibility IsInsertVisible => InsertCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public DelegateCommand InsertCommand { get; }

        //C:\\Users\\Ильшат\\source\\repos\\VendingAutomat\\VendingAutomat
        public string Icon => MoneyStack.Banknote.IsCoin ? "\\Images\\coin.jpg" : "\\Images\\banknote.jpg";
        public string Name => MoneyStack.Banknote.Name;
        public int Amount => MoneyStack.Amount;

        public MoneyStack MoneyStack { get; }

        public MoneyVM(MoneyStack moneyStack, PurchaseManager manager = null)
        {
            MoneyStack = moneyStack;

            if (manager != null)
            {
                InsertCommand = new DelegateCommand(() =>{
                    manager.InsertMoney(MoneyStack.Banknote);
                });
            }

            moneyStack.PropertyChanged += (s,a) => { RaisePropertyChanged(nameof(Amount)); }
        }

        
    }
}
