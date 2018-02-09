using System.Collections.Generic;

namespace P2PNetwork.Blockchain.Models
{
    public class Node
    {
        public string Address { get; set; }

        //public List<string> Peers { get; set; }

        public List<Block> Blocks { get; set; }

        public List<Transaction> PendingTransactions { get; set; }

        public Dictionary<string, decimal> Balances { get; set; }

        public long Difficulty { get; set; }

        public Dictionary<string, long> MiningJobs { get; set; }
    }
}
