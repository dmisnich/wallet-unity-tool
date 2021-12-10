using System;
using wallet_unity_tool.Runtime.Wallet.Core.Enums;

namespace wallet_unity_tool.Runtime.Wallet.API
{
    public interface IWallet
    {
        event Action<eCurrencyType, double> OnAmountChanged;
        double GetWalletAmount(eCurrencyType currencyType);
        void AddCurrency(eCurrencyType currencyType, double amount);
        bool TrySpendCurrency(eCurrencyType currencyType, double amount);
        void SpendCurrency(eCurrencyType currencyType, double amount);

        void ResetCurrency(eCurrencyType currencyType);
    }
}