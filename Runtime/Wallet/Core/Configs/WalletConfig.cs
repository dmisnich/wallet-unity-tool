using System;
using System.Collections.Generic;
using wallet_unity_tool.Runtime.Wallet.Core.Enums;

namespace wallet_unity_tool.Runtime.Wallet.Core.Configs
{
    [Serializable]
    public class WalletConfig
    {
        public Dictionary<eCurrencyType, WalletData> WalletDatas;
    }

    [Serializable]
    public class WalletData
    {
        public double CurrencyAmount;
    }
}