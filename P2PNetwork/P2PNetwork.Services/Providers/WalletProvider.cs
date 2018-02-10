using P2PNetwork.Blockchain.Models;
using P2PNetwork.Logic.Utils;
using P2PNetwork.Services.Utils;
using P2PNetwork.Utils.Services;
using Secp256k1;
using System;
using System.Numerics;

namespace P2PNetwork.Services.Providers
{
    public static class WalletProvider
    {
        private static Wallet wallet;

        public static void Initialize()
        {
            wallet = NetworkFileProvider<Wallet>.GetModel(Constants.WalletStoragePrivateKey);
            BigInteger privateKey;
            
            if (wallet.Address == null || wallet.Address == string.Empty)
            {
                Logger.LogLine("set password (must be 16 symbols):", ConsoleColor.Gray);
                var password = Console.ReadLine();
                var hex = CryptographyHelper.GetRandomHexNumber(62);
                privateKey = Hex.HexToBigInteger(hex);
                Logger.LogLine($"your private key is {privateKey}");

                var encryptedPrivateKey = CryptographyHelper.Encrypt(privateKey.ToString(), password);

                ECPoint publicKey = Secp256k1.Secp256k1.G.Multiply(privateKey);
                string bitcoinAddressUncompressed = publicKey.GetBitcoinAddress(false);
                string bitcoinAddressCompressed = publicKey.GetBitcoinAddress(compressed: true);

                wallet.PublicKey = publicKey;
                wallet.Address = bitcoinAddressCompressed;
                wallet.EncryptedPrivateKey = encryptedPrivateKey;

                NetworkFileProvider<Wallet>.SetModel(Constants.WalletStoragePrivateKey, wallet);
            }
            else
            {
                Logger.LogLine("your password:", ConsoleColor.Gray);
                var password = Console.ReadLine();

                Logger.LogLine($"your private key is {GetPrivateKey(password)}");
            }

            Logger.LogLine($"your address is {wallet.Address}");
        }

        public static Wallet Wallet
        {
            get
            {
                return wallet;
            }
        }

        public static BigInteger GetPrivateKey(string password)
        {
            return BigInteger.Parse(CryptographyHelper.Decrypt(wallet.EncryptedPrivateKey, password));
        }
    }
}
