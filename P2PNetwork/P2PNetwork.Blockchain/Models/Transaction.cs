using System;

namespace P2PNetwork.Blockchain.Models
{
    public class Transaction
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Value { get; set; }

        public string SenderPublicKey { get; set; }

        public Tuple<string, string> SenderSignature { get; set; }

        /*--------------------------------------------------------*/

        public string TransactionHash { get; set; }

        public DateTime DateReceived { get; set; }

        public long MinedInBlockIndex { get; set; }

        public bool Paid { get; set; }
    }
}
