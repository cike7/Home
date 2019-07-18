using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;//引入socket的空间

namespace MySocket
{
    class Program
    {
        static void Main(string[] args)
        {
            ///
            ///异步
            ///
            //Sever sever = new Sever();

            //sever.Start("172.27.0.9", 8888);

            //while (true)
            //{
            //    string str = Console.ReadLine();

            //    switch (str)
            //    {
            //        case "quit":
            //            return;
            //    }
            //}

            /////
            /////同步
            /////
            Socket listedfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//

            //IPAddress ipAdr = IPAddress.Parse("139.155.108.148");//服务端IP地址(本机局域网地址)

            //IPAddress ipAdr = IPAddress.Parse("117.162.24.151");//服务端IP地址(wifi)

            //IPAddress ipAdr = IPAddress.Parse("223.104.10.216");//服务端IP地址(手机)

            IPAddress ipAdr = IPAddress.Parse("172.27.0.2");//服务端IP地址(云服务器)

            //IPAddress ipAdr = IPAddress.Parse("127.0.0.1");//服务端IP地址(本机)

            IPEndPoint ipEp = new IPEndPoint(ipAdr, 8888);//端口(服务器IP地址,端口号)

            //listedfd.Bind(ipEp);//端口绑定

            listedfd.Connect(ipEp);

            listedfd.Listen(0);//监听人数，0代表不限制

            Console.WriteLine("服务器启动成功!");//提示

            while (true)
            {
                Socket connfd = listedfd.Accept();//创建一个新的链接

                Console.WriteLine("服务器接收成功!");

                byte[] readbuff = new byte[1024];//(创建一个byte缓冲区)传入进来的是一个byte的类型

                int count = connfd.Receive(readbuff);//用于接收的客户端数据,收到就去接收

                string str = System.Text.Encoding.Default.GetString(readbuff, 0, count);//编码处理接收的数据

                byte[] bytes = System.Text.Encoding.Default.GetBytes("服务器输出:" + str);//处理完后发送数据

                connfd.Send(bytes);//将新创建的链接数据发送出去

            }

        }
    }
}
