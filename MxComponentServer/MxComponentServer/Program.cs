using ActUtlType64Lib;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System;

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
        static int isGetYDataBlock = 0;
        static short[] yData = new short[32];

        public MxComopentServer()
        {
            mxComponent = new ActUtlType64();
            mxComponent.ActLogicalStationNumber = 1;
            new Thread(RepeatYThread).Start();
<<<<<<< Updated upstream

=======
            StartTCPServer();
>>>>>>> Stashed changes
            while (true)
            {
                int bytes;
                byte[] buffer = new byte[320];
                stream.Read(buffer, 0, 10);
                string output = Encoding.ASCII.GetString(buffer, 0, 10).Trim('\0');
                string[] splitOutput = output.Split(',');
                switch (splitOutput[0])
                {
                    case "R":
                        {
                            buffer = Encoding.ASCII.GetBytes(ydata);
                            stream.Write(buffer, 0, buffer.Length);
                            break;
                        }
                    case "W":
                        {
                            Console.WriteLine(output);
                            new Thread(() => SetData(splitOutput[1], int.Parse(splitOutput[2]))).Start();
                            break;
                        }
                    case "CP":
                        {
                            Console.WriteLine(ConnectPLC());
                            break;
                        }
                    case "DP":
                        {
                            Console.WriteLine(DisconnectPLC());
                            break;
                        }
                    case "CS":
                        {
                            CloseTCPServer();
                            break;
                        }
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

        static void RepeatYThread()
        {
            while (true)
            {
                if (isGetYDataBlock == 0)
                {
                    isGetYDataBlock = 1;
                    GetYDataBlock();
                    
                }
            }
        }
        static void GetYDataBlock()
        {
<<<<<<< Updated upstream
=======
            short[] yData = new short[32];
>>>>>>> Stashed changes
            mxComponent.ReadDeviceBlock2("Y0", 32, out yData[0]);
            ydata = ConvertDataIntoString(yData);
            isGetYDataBlock = 0;
        }

        public void SetData(string device, int value)
        {
            int ret = mxComponent.SetDevice(device, value);
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
            Console.WriteLine("Start TCP Server and Waiting Client");
        }

        void CloseTCPServer()
        {
            stream.Close();
            listener.Stop();

            listener.Start();
            client = listener.AcceptTcpClient();
            stream = client.GetStream();
            Console.WriteLine("Start TCP Server and Waiting Client");
        }

    }
    public class MxComponentServer
    {
        State state = State.Disconnected;
        static ActUtlType64 mxComponent;

        

        TcpListener listener;
        TcpClient client;
        NetworkStream stream;

        static TcpListener listener1;
        static TcpClient client1;
        static NetworkStream stream1;
        static string ydata;
        static string ddata;
        static int isGetYDataBlock = 0;
        static short[] yData = new short[20];
        static short[] dData = new short[10];

        static Stopwatch sw = Stopwatch.StartNew();


        public MxComponentServer()
        {
            mxComponent = new ActUtlType64();
            mxComponent.ActLogicalStationNumber = 1;
            Thread yThread = new Thread(RepeatYThread);
            yThread.Start();
            StartTCPServer();

            int bytes;
            byte[] buffer = new byte[200];
            while (true)
            {
                bytes = stream.Read(buffer, 0, 10);

                string output = Encoding.ASCII.GetString(buffer, 0, 10).Trim('\0');
                string[] splitOutput = output.Split(',');
                switch (splitOutput[0])
                {
                    case "RY":
                        {
                            buffer = Encoding.ASCII.GetBytes(ydata);
                            stream.Write(buffer, 0, buffer.Length);
                            break;
                        }
                    case "RD":
                        {
                            buffer = Encoding.ASCII.GetBytes(ddata);
                            stream.Write(buffer, 0, buffer.Length);
                            break;
                        }
                    case "W":
                        {
                            Console.WriteLine(output);
                            new Thread(() => SetData(splitOutput[1], int.Parse(splitOutput[2]))).Start();
                            break;
                        }
                    case "CP":
                        {
                            Console.WriteLine(ConnectPLC());
                            break;
                        }
                    case "DP":
                        {
                            Console.WriteLine(DisconnectPLC());
                            break;
                        }
                    case "CS":
                        {
                            CloseTCPServer();
                            break;
                        }
                }

            }
        }

        public string ConnectPLC()
        {
            int ret = mxComponent.Open();
            if (ret == 0)
            {
                state = State.Connected;
                return "Connection succeeded!";
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
                state = State.Disconnected;
                return "Disconnection succeeded!";
            }
            else
            {
                return "Disconnection failed...";
            }
        }

        static void RepeatYThread()
        {
            while (true)
            {
                if (isGetYDataBlock == 0)
                {
                    isGetYDataBlock = 1;
                    GetYDataBlock();
                }
            }
        }

        
        static void GetYDataBlock()
        {
            mxComponent.ReadDeviceBlock2("Y0", 20, out yData[0]);
            ydata = ConvertYDataIntoString(yData);
            isGetYDataBlock = 0;
        }
        static void GetDDataBlock()
        {
            mxComponent.ReadDeviceBlock2("D0", 10, out dData[0]);
            ddata = ConvertDDataIntoString(dData);
            isGetYDataBlock = 0;
        }

        public void SetData(string device, int value)
        {
            int ret = mxComponent.SetDevice(device, value);
        }

        static string ConvertYDataIntoString(short[] data)
        {
            StringBuilder newYData = new StringBuilder();
            foreach (var item in data)
            {
                string temp = Convert.ToString(item, 2).PadLeft(10, '0');
                newYData.Append(new string(temp.Reverse().ToArray()));
            }
            return newYData.ToString();
        }
        static string ConvertDDataIntoString(short[] data)
        {
            StringBuilder newDData = new StringBuilder();
            foreach (var item in data)
            {
                string temp = Convert.ToString(item, 2).PadLeft(16, '0');
                newDData.Append(new string(temp.Reverse().ToArray()));
            }
            return newDData.ToString();
        }

        void StartTCPServer()
        {
            listener = new TcpListener(IPAddress.Any, 7000);
            Console.WriteLine("Start TCP Server and Waiting Client");
            listener.Start();

            client = listener.AcceptTcpClient();
            stream = client.GetStream();
        }

        void CloseTCPServer()
        {
            stream.Close();
            client.Close();
            listener.Stop();

            Console.WriteLine("TCP Server Closed");
            listener.Start();

            client = listener.AcceptTcpClient();
            stream = client.GetStream();
        }
    }

    class Program
    {
        static void Main()
        {
            MxComponentServer server = new MxComponentServer();
        }
    }

}