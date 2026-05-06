using RabbitMQ.Client;
using System.Text;

namespace umfg.publisher.console
{
    internal class Program
    {
        private const string C_CONNECTION = "amqps://srwrdfzu:i59_Ow2ni7Qayn4UecU30KFSIU-v2-NX@jackal.rmq.cloudamqp.com/srwrdfzu";
        private const string C_QUEUE = "umfg-programacao-iv-teste-2026";

        static async Task Main(string[] args)
        {
            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    Uri = new Uri(C_CONNECTION),
                };
                var connection = await connectionFactory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();

                Console.WriteLine("Inicio envio de mensagens!");

                await channel.QueueDeclareAsync(queue: C_QUEUE,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                foreach (var numero in Enumerable.Range(1, 10))
                {
                    /*
                     * (string exchange, string routingKey,
            bool mandatory, TProperties basicProperties, ReadOnlyMemory<byte> body,
            CancellationToken cancellationToken = default)
                     */

                    var properties = new BasicProperties
                    {
                        Persistent = true
                    };

                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await channel.BasicPublishAsync("",
                        C_QUEUE,
                        false,
                        properties,
                        Encoding.UTF8.GetBytes($"{numero}º Hello Word!"),
                        CancellationToken.None);
                }

                Console.WriteLine("Fim envio de mensagens!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
