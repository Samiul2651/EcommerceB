using System.Net.Mail;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Routing.Template;
using SendGrid;
using SendGrid.Helpers.Mail;


var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "order", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    int dots = message.Split('.').Length - 1;
    await Task.Delay(dots * 1000);

    var order = JsonSerializer.Deserialize<Order>(message);
    var apiKey = Environment.GetEnvironmentVariable("SENDGRID_KEY");
    var client = new SendGridClient(apiKey);
    var from = new EmailAddress("istiaqsamiul@gmail.com", "MyStore");
    var subject = "Your Order has been Placed";
    var to = new EmailAddress(order.CustomerId, "");
    var plainTextContent = order.GeneratePlainTextEmail(order);
    var productLines = string.Join("", order.Products.Select(p =>
        $"<tr><td>{p.Name}</td><td>{p.Price}</td><td>{p.Quantity}</td><td>{p.Price * p.Quantity}</td></tr>"));
    string htmlContent = $@"

    <h2>Order Confirmation</h2>
    <p><strong>Order ID:</strong> {order.Id}</p>
    <p><strong>Customer ID:</strong> {order.CustomerId}</p>
    <p><strong>Order Time:</strong> {order.OrderTime}</p>
    <p><strong>Total Price:</strong> {order.Price}</p>

    <h3>Shipping Address</h3>
    <p>{order.Address}</p>
    <p>Phone: {order.PhoneNumber}</p>

    <h3>Products:</h3>
    <table class='order-details'>
        <tr><th>Product</th><th>Price</th><th>Quantity</th><th>Total</th></tr>
        {productLines}
    </table>

    <p class='summary'>Thank you for your purchase!</p>";
    //Console.WriteLine(htmlContent);
    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
    var response = await client.SendEmailAsync(msg);
    Console.WriteLine(response.StatusCode);
    Console.WriteLine(" [x] Done");
    //return Task.CompletedTask;
};

await channel.BasicConsumeAsync("order", autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();



internal class Order
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public List<Product> Products { get; set; }
    public int Price { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime OrderTime { get; set; }

    public string GeneratePlainTextEmail(Order order)
    {
        var productLines = string.Join("\n", order.Products.Select(p =>
            $"Product: {p.Name}\nPrice: {p.Price} x {p.Quantity} = {p.Price * p.Quantity}\nCategory: {p.Category}\n"));

        string plainText = $@"
Order Confirmation

Order ID: {order.Id}
Customer ID: {order.CustomerId}
Order Time: {order.OrderTime}
Total Price: {order.Price}

Shipping Address:
{order.Address}
Phone: {order.PhoneNumber}

Products:
{productLines}

Thank you for your purchase!
";
        return plainText;
    }

    public string GenerateHtmlEmail(Order order)
    {
        var productLines = string.Join("", order.Products.Select(p =>
            $"<tr><td>{p.Name}</td><td>{p.Price}</td><td>{p.Quantity}</td><td>{p.Price * p.Quantity}</td></tr>"));

        string htmlContent = $@"

    <h2>Order Confirmation</h2>
    <p><strong>Order ID:</strong> {order.Id}</p>
    <p><strong>Customer ID:</strong> {order.CustomerId}</p>
    <p><strong>Order Time:</strong> {order.OrderTime}</p>
    <p><strong>Total Price:</strong> {order.Price}</p>

    <h3>Shipping Address</h3>
    <p>{order.Address}</p>
    <p>Phone: {order.PhoneNumber}</p>

    <h3>Products:</h3>
    <table class='order-details'>
        <tr><th>Product</th><th>Price</th><th>Quantity</th><th>Total</th></tr>
        {productLines}
    </table>

    <p class='summary'>Thank you for your purchase!</p>";
        return htmlContent;
    }
}

public class Product
{
   
    public string Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Type { get; set; }
    public string ImageLink { get; set; }
    public int TrendingScore { get; set; }
    public string Category { get; set; }
    public int Quantity { get; set; }
}