using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace VendingMachine.Model
{
    public class MoneyStack : BindableBase 
    {
        private int _amount;

        public Banknote Banknote{ get; }
        public int Amount { get { return _amount; } set {SetProperty(ref _amount, value) ; } }
        public MoneyStack(Banknote banknot, int amount)
        {
            Banknote = banknot;
            Amount = amount;

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
