using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System;
using System.Text;
using System.Net;


public class MxComponentTest : MonoBehaviour
{
    public enum Connection
    {
        Connected,
        Disconnected
    }

    TcpClient client;
    NetworkStream stream;

    public string dataBlock;
    // Start is called before the first frame update
    void Start()
    {
        string word = "다이다이다";
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Read()
    {
        byte[] buffer = new byte[100];
        stream.Read(buffer, 0, 100);
        dataBlock = Encoding.ASCII.GetString(buffer);
        print(dataBlock);
    }

    public void Write(string word)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(word);
        print("Write");
        stream.Write(buffer, 0, buffer.Length);
    }


    public void connectTCPServer()
    {
        client = new TcpClient("127.0.0.1", 7000);

        print("TCP 서버 연결 대기중");
        stream = client.GetStream();
        print("TCP 서버 연결 완료");
    }
}
