using RabbitMQ.Client;
using System;
using System.Text;
using System.Windows.Forms;

namespace demo1_sender
{
    public partial class Form1 : Form
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel model;

    
        public Form1()
        {
            factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            model = connection.CreateModel();
            InitializeComponent();

        }

        private void button1_Click(Object sender,EventArgs e)
        {

            model.ExchangeDeclare(exchange: textBox1.Text, type: "fanout");
            label3.Text = "trying to send message";
            
            var body = Encoding.UTF8.GetBytes(textBox2.Text);
            var myprop = model.CreateBasicProperties();
            myprop.Persistent = false;
            model.BasicPublish(exchange: textBox1.Text,
                routingKey: "",
                basicProperties: myprop,
                body: body);
            label3.Text = "message sending successful";
        }

        
    }
}
