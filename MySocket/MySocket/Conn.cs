using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MySocket
{
    /// <summary>
    /// 处理客户端连接
    /// </summary>
    class Conn
    {
        public const int buff_size = 1024;//缓冲大小
        public Socket socket;//定义服务器
        public bool isUse = false;//是否使用
        public byte[] readBuff = new byte[buff_size];//读取缓冲
        public int bufCount = 0;

        public Conn()//构造函数
        {
            readBuff = new byte[buff_size];
        }

        public void Init(Socket socket)//初始化
        {
            this.socket = socket;
            isUse = true;
            bufCount = 0;
        }

        //缓冲剩余的字节数
        public int BuffRemain()
        {
            return buff_size - bufCount;
        }

        //获取客户端地址
        public string GetAdress()
        {
            if(!isUse)
            {
                return "无法获取地址\n";
            }
            else
            {
                return socket.RemoteEndPoint.ToString();//获取客户端IP地址和端口
            }
        }

        public void Close()//关闭连接
        {
            if (!isUse) return;
            Console.WriteLine(GetAdress()+"与服务器断开连接-----\n");
            socket.Close();//断开连接
            isUse = false;
        }
    }
}
