using P2PNetwork.Services.Providers;
using System;

namespace P2PNetwork
{
    public class StartUp
    {
        public static void Main()
        {
            AsynchronousSocketListener.StartListening();
            AsynchronousClient.StartClient();

            while (true)
            {
                string input = Console.ReadLine();

                CommandProvider.RunCommand(input.Split(' ')[0], input);
            }
        }
    }
}
