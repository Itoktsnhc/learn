using System;
using System.Net.Http;
using System.Threading.Tasks;
using Dapr.Client;

namespace DaprCounter
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            const string storeName = "statestore";
            const string key = "counter";

            var daprClient = new DaprClientBuilder().Build();
            var counter = 0;

            while (true)
            {
                counter = await daprClient.GetStateAsync<int>(storeName, key);
                long ticks = 0;
                //ticks = await daprClient.InvokeMethodAsync<long>(HttpMethod.Get, "DaprCounterApi", "hello/ticks");
                Console.WriteLine($"Counter = {counter++} in  {ticks}");

                await daprClient.SaveStateAsync(storeName, key, counter);
                await Task.Delay(5000);
            }
        }
    }
}
