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

            label3.Text = "trying to send message";
            model.ExchangeDeclare(exchange: "logexc", type: "direct");
            var body = Encoding.UTF8.GetBytes(textBox2.Text);
            var myprop = model.CreateBasicProperties();
            myprop.Persistent = false;
            model.BasicPublish(exchange: "logexc",
                routingKey: textBox1.Text,
                basicProperties: null,
                body: body);
            label3.Text = "message sending successful";
        }

        
    }
}
