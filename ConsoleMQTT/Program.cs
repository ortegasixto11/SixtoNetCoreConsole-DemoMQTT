using MQTTnet.Extensions.ManagedClient;
using System;
using System.Text;

namespace ConsoleMQTT
{
    class Program
    {
        static void Main(string[] args)
        {
            // Para recibir mensaje
            string topic = "Topic 2";
            var mqttHelper = new MQTTHelper(Guid.NewGuid().ToString(), "postman.cloudmqtt.com", "lhnrhvok", "FiHorUN2CkCP", 11252, topic, Callback_MessageReceived);

            // Para enviar mensaje
            //var mqttHelper = new MQTTHelper(Guid.NewGuid().ToString(), "postman.cloudmqtt.com", "lhnrhvok", "FiHorUN2CkCP", 11252);
            //var x = mqttHelper.PublishAsync("Sixto", "Un nuevo mensaje").Result;
            Console.ReadLine();
        }

        static void Callback_MessageReceived(string message)
        {
            Console.WriteLine($"Callback_MessageReceived => {message}");
        }

    }
}
