using P2PNetwork.Services.Utils;
using P2PNetwork.Utils.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace P2PNetwork.Services.Providers
{
    public class CommandProvider
    {
        public static Blockchain.Blockchain blockchain;

        public static void RunCommand(string commandName, string input)
        {
            blockchain = NetworkFileProvider<Blockchain.Blockchain>.GetModel(Constants.BlockchainStoragePath);

            if (commandName == "connect")
            {
                string template = "{0} -ip {1} -p {2}";
                var args = ReverseStringFormat(template, input);
                int port = int.Parse(args[2]);

                IPAddress clientIp;

                if (IPAddress.TryParse(args[1], out clientIp))
                {
                    blockchain.RegisterNode($"{clientIp}:{port}");
                    AsynchronousClient.Connect(clientIp, port);
                }
                else
                {
                    Logger.LogLine("incorect command parameters", ConsoleColor.Red);
                }
            }

            if (commandName == "peers")
            {
                if (blockchain.Nodes.Count > 0)
                {
                    foreach (var node in blockchain.Nodes)
                    {
                        Logger.LogLine(node.Address, ConsoleColor.Green);
                    }
                }
                else
                {
                    Logger.LogLine("no peers", ConsoleColor.Red);
                }
            }

            if (commandName == "start-wallet")
            {
                WalletProvider.Initialize();
            }

            if (commandName == "sync")
            {
                var result = blockchain.ResolveConflicts();

                if (result)
                {
                    Logger.LogLine("successfully sync network", ConsoleColor.Green);
                }
                else
                {
                    Logger.LogLine("error while sync network", ConsoleColor.Red);
                }
            }
        }

        private static List<string> ReverseStringFormat(string template, string str)
        {
            string pattern = "^" + Regex.Replace(template, @"\{[0-9]+\}", "(.*?)") + "$";

            Regex r = new Regex(pattern);
            Match m = r.Match(str);

            List<string> ret = new List<string>();

            for (int i = 1; i < m.Groups.Count; i++)
            {
                ret.Add(m.Groups[i].Value);
            }

            return ret;
        }
    }
}
