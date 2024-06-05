using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MxComponent : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    bool isTCPConnecting;
    bool isPLCConnecting;

    public GameObject pushCylinder;
    public GameObject ShieldCylinder;

    public GameObject pushCylinderStartPoint;
    public GameObject pushCylinderEndPoint;
    public GameObject pushCylinderSensor1;
    public GameObject pushCylinderSensor2;
    public GameObject pushCylinderSensor3;
    public GameObject pushCylinderSensor4;
    public GameObject ShieldCylinderStartPoint;
    public GameObject ShieldCylinderEndPoint;
    public GameObject ShieldCylinderSensor1;
    public GameObject ShieldCylinderSensor2;
    public GameObject PushSensor;
    public GameObject ShieldSensor1;
    public GameObject ShieldSensor2;

    public string preYDataBlock;
    public string yDataBlock;

    void Start()
    {
        isTCPConnecting = false;
        isPLCConnecting = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTCPConnecting && isPLCConnecting)
        {
            preYDataBlock = yDataBlock;
            Write("R,");
            Read();
            if(preYDataBlock != yDataBlock)
            {
                pushCylinder.GetComponent<Cylinder>().PLCInput1 = yDataBlock[0];
                pushCylinder.GetComponent<Cylinder>().PLCInput2 = yDataBlock[1];
                ShieldCylinder.GetComponent<Cylinder>().PLCInput1 = yDataBlock[2];
                ShieldCylinder.GetComponent<Cylinder>().PLCInput2 = yDataBlock[3];
            }
            
            Sensor(pushCylinderStartPoint, "X0");
            Sensor(pushCylinderEndPoint, "X1");
            Sensor(pushCylinderSensor1, "X2");
            Sensor(pushCylinderSensor2, "X3");
            Sensor(pushCylinderSensor3, "X4");
            Sensor(pushCylinderSensor4, "X5");
            Sensor(ShieldCylinderStartPoint, "X6");
            Sensor(ShieldCylinderEndPoint, "X7");
            Sensor(ShieldCylinderSensor1, "X8");
            Sensor(ShieldCylinderSensor2, "X9");
            Sensor(PushSensor, "X10");
            Sensor(ShieldSensor1, "X11");
            Sensor(ShieldSensor2, "X12");
            
        }
    }

    public void Sensor(GameObject Sensor,string component)
    {
        if (Sensor.GetComponent<Sensor>().isChange == 1)
        {
            Write($"W,{component},{Sensor.GetComponent<Sensor>().PLCOutput},");
                Sensor.GetComponent<Sensor>().isChange = 0;
            }
    }
    public void Read()
    {
        byte[] buffer = new byte[200];
        stream.Read(buffer, 0, 200);
        yDataBlock = Encoding.ASCII.GetString(buffer);
    }

    public void Write(string word)
    {

        byte[] buffer = Encoding.ASCII.GetBytes(word.PadRight(10));
        stream.Write(buffer, 0, buffer.Length);

    }

    public void ConnectTCPServer()
    {
        client = new TcpClient("127.0.0.1", 7000);

        stream = client.GetStream();
        print("TCP 서버 연결 완료");
        isTCPConnecting = true;
    }
    public void ConnectPLC()
    {
        Write("CP,");
        print("PLC 서버 연결 완료");
        isPLCConnecting = true;
    }
    public void DisconnectPLC()
    {
        Write("DP,");
    }
}
