using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace umfg.consumer.console
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

                Console.WriteLine("Inicio consumo de mensagens!");

                await channel.QueueDeclareAsync(queue: C_QUEUE,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += ConsumirMensagem;

                await channel.BasicConsumeAsync
                    (
                        queue: C_QUEUE,
                        autoAck: true,
                        consumer: consumer
                    );

                Console.WriteLine("Pressione [enter] para sair.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private static async Task ConsumirMensagem(object sender, BasicDeliverEventArgs e)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"[Mensagem recebida:] | " +
                    $"{DateTime.Now} | " +
                    $"{Encoding.UTF8.GetString(e.Body.ToArray())}");
            });
        }
    }
}