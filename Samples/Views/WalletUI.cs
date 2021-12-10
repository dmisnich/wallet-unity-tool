using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using wallet_unity_tool.Runtime.Wallet.API;
using wallet_unity_tool.Runtime.Wallet.Core.Enums;
using wallet_unity_tool.Runtime.Wallet.Models;

namespace wallet_unity_tool.Samples.Views
{
    public class WalletUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private TMP_Text _currencyStateText;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Dropdown _currencyDropdown;
        [SerializeField] private Button _addCurrency;
        [SerializeField] private Button _spendCurrency;
        [SerializeField] private Button _resetCurrency;
        
        private IWallet _wallet;
        private eCurrencyType _currentCurrencyType;
        
        private void Awake()
        {
            SetWallet(new WalletModel()); // You can use this method for set up wallet from other scripts
            SetDropdownOptions();
            OnDropdownValueChanged();
        }

        private void OnEnable()
        {
            _wallet.OnAmountChanged += OnWalletAmountChanged;
            _addCurrency.onClick.AddListener(AddCurrencyAmount);
            _spendCurrency.onClick.AddListener(SpendCurrencyAmount);
            _resetCurrency.onClick.AddListener(ResetCurrencyAmount);
        }

        private void OnDisable()
        {
            _wallet.OnAmountChanged -= OnWalletAmountChanged;
            _addCurrency.onClick.RemoveListener(AddCurrencyAmount);
            _spendCurrency.onClick.RemoveListener(SpendCurrencyAmount);
            _resetCurrency.onClick.RemoveListener(ResetCurrencyAmount);
        }

        public void SetWallet(IWallet wallet)
        {
            _wallet = wallet;
        }

        private void SetDropdownOptions()
        {
            var currencyTypes = Enum.GetNames(typeof(eCurrencyType));
            _currencyDropdown.AddOptions(currencyTypes.ToList());
        }

        private void AddCurrencyAmount()
        {
            try
            {
                var amount = Int32.Parse(_inputField.text);
                _wallet.AddCurrency(_currentCurrencyType, amount);
                _currencyStateText.text = $"{_currentCurrencyType.ToString()} currency added";
            }
            catch
            {
                _currencyStateText.text = "Please, enter valid value";
                Debug.Log($"Unable to parse '{_inputField.text}'");
            }
        }
        
        private void SpendCurrencyAmount()
        {
            try
            {
                var amount = Int32.Parse(_inputField.text);
                if (_wallet.TrySpendCurrency(_currentCurrencyType, amount))
                {
                    _wallet.SpendCurrency(_currentCurrencyType, amount);
                    _currencyStateText.text = $"{_currentCurrencyType.ToString()} currency spent";
                }
                else
                {
                    _currencyStateText.text = $"Not enough {_currentCurrencyType.ToString()} currency";
                }
            }
            catch
            {
                _currencyStateText.text = "Please, enter valid value";
                Debug.Log($"Unable to parse '{_inputField.text}'");
            }
        }
        
        private void ResetCurrencyAmount()
        {
            _wallet.ResetCurrency(_currentCurrencyType);
            _currencyStateText.text = $"{_currentCurrencyType.ToString()} currency reset";
        }

        public void OnDropdownValueChanged()
        {
            _currentCurrencyType = (eCurrencyType) _currencyDropdown.value;
            _currencyText.text = $"BALANCE: {_wallet.GetWalletAmount(_currentCurrencyType)}";
            _currencyStateText.text = "";
        }

        private void OnWalletAmountChanged(eCurrencyType currencyType, double amount)
        {
            _currencyText.text = $"BALANCE: {_wallet.GetWalletAmount(_currentCurrencyType)}";
        }
    }
}