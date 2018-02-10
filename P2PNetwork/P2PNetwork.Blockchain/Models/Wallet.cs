using Secp256k1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace P2PNetwork.Blockchain.Models
{
    public class Wallet
    {
        public string Address { get; set; }

        public string EncryptedPrivateKey { get; set; }
        
        public ECPoint PublicKey { get; set; }
    }
}
