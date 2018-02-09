using Newtonsoft.Json;
using P2PNetwork.Blockchain.Helpers;
using P2PNetwork.Blockchain.Models;
using P2PNetwork.Services.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace P2PNetwork.Blockchain
{
    public class Blockchain
    {
        private List<Block> chain;
        private List<Transaction> currentTransactions;
        private List<Node> nodes;

        public Blockchain()
        {
            this.chain = new List<Block>();
            this.currentTransactions = new List<Transaction>();
            this.nodes = new List<Node>();

            this.AddBlock(100, "1");
        }

        public Block LastBlock
        {
            get
            {
                return this.chain.Last();
            }
        }

        public List<Block> Chain
        {
            get
            {
                return this.chain;
            }
        }

        public List<Node> Nodes
        {
            get
            {
                return this.nodes;
            }
        }

        public void RegisterNode(string address)
        {
            var node = new Node()
            {
                Address = address
            };

            this.nodes.Add(node);
        }

        public bool ResolveConflicts()
        {
            var neighbours = this.nodes;
            List<Block> newChain = null;

            var maxLenght = this.chain.Count;

            foreach (var node in neighbours)
            {
                List<Block> chain;

                var ip = node.Address.Split(' ')[0];
                var port = int.Parse(node.Address.Split(' ')[1]);

                IPAddress address = IPAddress.Parse(ip);
               
                chain = AsynchronousClient.GetBlockchain(address, port);

                var length = chain.Count;

                if (length > maxLenght && this.ValidChain(chain))
                {
                    maxLenght = length;
                    newChain = chain;
                }
            }

            if (newChain != null)
            {
                this.chain = newChain;

                return true;
            }

            return false;
        }

        public Block AddBlock(int proof, string previousHash)
        {
            var block = new Block()
            {
                Index = this.chain.Count + 1,
                //Timestamp = DateTime.Now,
                Transactions = this.currentTransactions,
                //Proof = proof,
                //PreviousHash = previousHash != null ? previousHash : this.Hash(JsonConvert.SerializeObject(this.chain.Last()))
            };

            this.currentTransactions = new List<Transaction>();

            this.chain.Add(block);

            return block;
        }

        public long AddTransaction(string sender, string recipient, decimal amount)
        {
            var transaction = new Transaction()
            {
                //Sender = sender,
                //Recipient = recipient,
                //Amount = amount
            };

            this.currentTransactions.Add(transaction);

            return this.LastBlock.Index + 1;
        }

        public bool ValidChain(List<Block> chain)
        {
            var lastBlock = chain[0];
            var currentIndex = 1;

            while (currentIndex < chain.Count)
            {
                var block = chain[currentIndex];
                var json = JsonConvert.SerializeObject(lastBlock);

                //if (block.PreviousHash != this.Hash(json))
                //{
                //    return false;
                //}

                //if (!this.ValidProof(lastBlock.Proof, block.Proof))
                //{
                //    return false;
                //}

                lastBlock = block;
                currentIndex += 1;
            }

            return true;
        }

        public bool ValidProof(int lastProof, int proof)
        {
            var guess = Encoding.UTF8.GetBytes(string.Format("{0}{1}", lastProof, proof));

            var guessHash = Encoding.UTF8.GetString(CryptographyHelper.Sha256(guess));

            return string.Join("", guessHash.Take(4)) == "0000";
        }

        public int ProofOfWork(int lastProof)
        {
            int proof = 0;

            while (!this.ValidProof(lastProof, proof))
            {
                proof++;
            }

            return proof;
        }

        public string Hash(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            
            var sha256 = CryptographyHelper.Sha256(bytes);

            return Encoding.UTF8.GetString(sha256);
        }
    }
}
