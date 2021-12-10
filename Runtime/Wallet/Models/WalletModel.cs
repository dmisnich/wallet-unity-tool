using System;
using System.Collections.Generic;
using wallet_unity_tool.Runtime.Saves.API;
using wallet_unity_tool.Runtime.Saves.Impl;
using wallet_unity_tool.Runtime.Wallet.API;
using wallet_unity_tool.Runtime.Wallet.Core.Configs;
using wallet_unity_tool.Runtime.Wallet.Core.Enums;

namespace wallet_unity_tool.Runtime.Wallet.Models
{
    public class WalletModel : IWallet
    {
        public event Action<eCurrencyType, double> OnAmountChanged;
        
        private WalletConfig _walletConfig;
        private ISaveService _saveService = new UnitySaveService(); // Tou can use other save services (LocalStorageSaveService, BinarySaveService)

        private const string SAVE_PATH = "WalletCurrency";

        public WalletModel()
        {
            InitWallet();
        }

        private void InitWallet()
        {
            if (_saveService.TryToLoad<WalletConfig>(SAVE_PATH, out var walletConfig))
            {
                _walletConfig = walletConfig;
            }
            else
            {
                _walletConfig = new WalletConfig();
                _walletConfig.WalletDatas = new Dictionary<eCurrencyType, WalletData>();
                AddNewWallet(eCurrencyType.Soft, 100);
            }
        }

        private void AddNewWallet(eCurrencyType currencyType, double amount = 0)
        {
            if (!_walletConfig.WalletDatas.ContainsKey(currencyType))
            {
                _walletConfig.WalletDatas.Add(currencyType, CreateNewWallet(amount));
                _saveService.Save(_walletConfig, SAVE_PATH);
            }
        }

        public double GetWalletAmount(eCurrencyType currencyType)
        {
            CheckWallet(currencyType);
            return _walletConfig.WalletDatas[currencyType].CurrencyAmount;
        }

        public void AddCurrency(eCurrencyType currencyType, double amount)
        {
            CheckWallet(currencyType);
            _walletConfig.WalletDatas[currencyType].CurrencyAmount += amount;
            OnAmountChanged?.Invoke(currencyType, _walletConfig.WalletDatas[currencyType].CurrencyAmount);
            _saveService.Save(_walletConfig, SAVE_PATH);
        }

        public bool TrySpendCurrency(eCurrencyType currencyType, double amount)
        {
            return _walletConfig.WalletDatas[currencyType].CurrencyAmount >= amount;
        }

        public void SpendCurrency(eCurrencyType currencyType, double amount)
        {
            CheckWallet(currencyType);
            if (_walletConfig.WalletDatas[currencyType].CurrencyAmount >= amount)
            {
                _walletConfig.WalletDatas[currencyType].CurrencyAmount -= amount;
                OnAmountChanged?.Invoke(currencyType, _walletConfig.WalletDatas[currencyType].CurrencyAmount);
                _saveService.Save(_walletConfig, SAVE_PATH);
            }
            else
            {
                throw new Exception($"Not enough {currencyType.ToString()} currency");
            }

        }

        public void ResetCurrency(eCurrencyType currencyType)
        {
            _walletConfig.WalletDatas[currencyType].CurrencyAmount = 0;
            OnAmountChanged?.Invoke(currencyType, _walletConfig.WalletDatas[currencyType].CurrencyAmount);
            _saveService.Save(_walletConfig, SAVE_PATH);
        }

        private void CheckWallet(eCurrencyType currencyType)
        {
            if (!_walletConfig.WalletDatas.ContainsKey(currencyType))
                AddNewWallet(currencyType);
        }

        private WalletData CreateNewWallet(double amount)
        {
            return new WalletData
            {
                CurrencyAmount = amount
            };
        }
    }
}