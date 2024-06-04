using ActUtlType64Lib;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;

namespace MxComponentServer
{
    enum State
    {
        Disconnected,
        Connected
    }

    public class MxComopentServer
    {
        State state = State.Disconnected;
        static ActUtlType64 mxComponent;

        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        Thread thread = new Thread(GetYDataBlock);
        static string ydata;

        public MxComopentServer()
        {
            mxComponent = new ActUtlType64();
            mxComponent.ActLogicalStationNumber = 1;
            StartTCPServer();

            while (true)
            {
                int bytes;
                byte[] buffer = new byte[100];
                Console.WriteLine(stream.CanRead);
                Console.WriteLine(stream.CanWrite);
                if (stream.CanRead)
                {
                    stream.Read(buffer, 0, buffer.Length);
                    string output = Encoding.ASCII.
                    GetString(buffer, 0, 100).Trim('\0');
                    Console.WriteLine(output);
                    Console.WriteLine(stream.CanRead);
                    Console.WriteLine(stream.CanWrite);
                    buffer = Encoding.ASCII.GetBytes(output);
                    stream.Write(buffer, 0, buffer.Length);

                }
            }
        }

        public string ConnectPLC()
        {
            int ret = mxComponent.Open();
            if (ret == 0)
            {
                return "Connection succeded!";
            }
            else
            {
                return "Connection failed...";
            }
        }

        public string DisconnectPLC()
        {
            int ret = mxComponent.Close();
            if (ret == 0)
            {
                return "Disconnection succeded!";
            }
            else
            {
                return "Disconnection failed...";
            }
        }

        static void GetYDataBlock()
        {
            short[] yData = new short[11];
            mxComponent.ReadDeviceBlock2("Y0", 11, out yData[0]);

            ydata = ConvertDataIntoString(yData);
        }

        public void SetData(string device, int value)
        {
            int ret = mxComponent.SetDevice(device, 1);
        }
        static string ConvertDataIntoString(short[] data)
        {
            string newYData = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    newYData += "0000000000";
                    continue;
                }

                string temp = Convert.ToString(data[i], 2);// 100
                string temp2 = new string(temp.Reverse().ToArray()); // reverse 100 -> 001  
                newYData += temp2; // 0000000000 + 001

                if (temp2.Length < 10)
                {
                    int zeroCount = 10 - temp2.Length; // 7 -> 7개의 0을 newYData에 더해준다. (0000000)
                    for (int j = 0; j < zeroCount; j++)
                        newYData += "0";
                } // 0000000000 + 001 + 0000000 -> 총 20개의 비트
            }

            return newYData;
        }

        void StartTCPServer()
        {
            listener = new TcpListener(IPAddress.Any, 7000);
            listener.Start();

            client = listener.AcceptTcpClient();
            stream = client.GetStream();
            Console.WriteLine("Start TCP Server");
        }

    }

    class Program
    {
        static void Main()
        {
            MxComopentServer server = new MxComopentServer();
        }
    }

}