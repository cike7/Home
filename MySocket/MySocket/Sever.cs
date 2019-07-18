using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MySocket
{
    class Sever
    {
        public Socket listedfd;//监听嵌套字
        public Conn[] conns;//客户端连接池
        public int maxCoun = 50;//最大连接数

        public int NewIndex()//获取连接池，返回负数表示连接失败
        {
            if (conns == null)
            {
                return -1;
            }
            for (int i = 0; i < conns.Length; i++)
            {
                if (conns[i] == null)//如果连接数为空则新建一个连接
                {
                    conns[i] = new Conn();
                    return i;
                }
                else if (conns[i].isUse == false)//如果连接没有被使用返i
                {
                    return i;
                }
            }
            return -1;
        }

        public void Start(string host, int port)//开启连接
        {
            //连接池
            conns = new Conn[maxCoun];//定义一个连接，传入最大连接数

            for (int i = 0; i < maxCoun; i++)
            {
                conns[i] = new Conn();
            }

            listedfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAdr = IPAddress.Parse(host);//服务端IP地址(服务器IP地址)

            IPEndPoint ipEp = new IPEndPoint(ipAdr, port);//端口(服务器IP地址,端口号)

            //listedfd.Bind(ipEp);//绑定端口

            listedfd.Connect(ipEp);

            listedfd.Listen(maxCoun);//最大监听数

            listedfd.BeginAccept(AcceptCb, null);//开启接收数据

            Console.WriteLine("服务器启动成功!\n");

        }

        //回调
        private void AcceptCb(IAsyncResult ar)//用于异步接收客户端的数据
        {
            try
            {
                Socket socket = listedfd.EndAccept(ar);
                int index = NewIndex();
                if (index < 0)
                {
                    socket.Close();//断开连接
                    Console.WriteLine("警告:连接数已满!\n");
                }
                else
                {
                    Conn conn = conns[index];//创建连接

                    conn.Init(socket);//初始化

                    string adr = conn.GetAdress();//获取客户端的地址

                    Console.WriteLine("有客户端连接:" + adr + "ID号为:" + index + "\n");

                    conn.socket.BeginReceive(conn.readBuff, conn.bufCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, conn);//接收数据

                    //完成客户端连接之后再去开启监听
                    listedfd.BeginAccept(AcceptCb, null);//再次调用循环
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("回调失败:" + e.Message);
            }
        }

        private void ReceiveCb(IAsyncResult ar)
        {
            Conn conn = (Conn)ar.AsyncState;
            try
            {
                int count = conn.socket.EndReceive(ar);

                if (count <= 0)//判断是否收到客户端与服务端连接信号
                {
                    conn.Close();//调用关闭服务器连接方法
                    return;
                }
                string str = System.Text.Encoding.UTF8.GetString(conn.readBuff, 0, count);//编译数据处理

                Console.WriteLine("收到 " + conn.GetAdress() + "发送过来的数据 " + str + "\n");

                str = conn.GetAdress() + ":" + str;

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);//处理完后再发送数据

                for (int i = 0; i < conns.Length; i++)//将收到消息发送给其他客户端
                {
                    if (conns[i] == null)
                    {
                        continue;//继续
                    }
                    if (!conns[i].isUse)
                    {
                        continue;//继续
                    }
                    Console.WriteLine("服务器消息转发给" + conns[i].GetAdress() + "\n");

                    conns[i].socket.Send(bytes);//将消息发送给客户端

                }

                conn.socket.BeginReceive(conn.readBuff, conn.bufCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, conn);//继续接受数据

            }
            catch (Exception e)
            {
                Console.WriteLine("收到:" + conn.GetAdress() + "断开连接/n" + "异常消息:" + e.Message + "\n");
                conn.Close();//断开连接
            }
        }

    }
}
