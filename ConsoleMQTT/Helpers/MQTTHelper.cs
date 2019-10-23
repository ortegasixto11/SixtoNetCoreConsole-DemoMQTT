using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// Tutorial
// https://dzone.com/articles/mqtt-publishing-and-subscribing-messages-to-mqtt-b


public class MQTTHelper
{
    public IManagedMqttClient Client { get; set; }
    public Action<string> _CallBack;

    public  MQTTHelper(string clientId, string mqttURI, string mqttUser, string mqttPassword, int mqttPort, string topicSubscribe = "", Action<string> CallBack = null, bool mqttSecure = false)
    {
        _CallBack = CallBack;

        var messageBuilder = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithCredentials(mqttUser, mqttPassword)
            .WithTcpServer(mqttURI, mqttPort)
            .WithCleanSession();

        var options = mqttSecure
            ? messageBuilder
              .WithTls()
              .Build()
            : messageBuilder
              .Build();

        var managedOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(options)
            .Build();

        Client = new MqttFactory().CreateManagedMqttClient();
        Client.StartAsync(managedOptions);

        if(!string.IsNullOrEmpty(topicSubscribe)) 
        {
            var x = this.SubscribeAsync(topicSubscribe).Result;
        }
        this.Client.UseApplicationMessageReceivedHandler(e => {
            string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            _CallBack?.Invoke(message);
        });
    }

    public async Task<bool> PublishAsync(string topic, string payload, bool retainFlag = true, int qos = 1)
    {
        await Client.PublishAsync(new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .WithRetainFlag(retainFlag)
            .Build());
        return true;
    }

    private async Task<bool> SubscribeAsync(string topic, int qos = 1)
    {
        await Client.SubscribeAsync(new TopicFilterBuilder()
            .WithTopic(topic)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .Build());

        return true;
    }


}
