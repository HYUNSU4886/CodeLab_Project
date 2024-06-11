using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public GameObject Conveyor;

    public GameObject pushCylinderSensor1;
    public GameObject pushCylinderSensor2;
    public GameObject pushCylinderSensor3;
    public GameObject pushCylinderSensor4;
    public GameObject ShieldCylinderSensor1;
    public GameObject PushSensor;
    public GameObject ShieldSensor1;
    public GameObject ShieldSensor2;

    public string preYDataBlock;
    public string yDataBlock;

    public int isManual;
    public int isAuto;
    public string isMRConveyorMoving;
    public string isMLConveyorMoving;
    public string isMRPCylinderMoving;
    public string isMLPCylinderMoving;
    public string isMRSCylinderMoving;
    public string isMLSCylinderMoving;

    void Start()
    {
        isTCPConnecting = false;
        isPLCConnecting = false;
        isMRConveyorMoving = "0";
        isMLConveyorMoving = "0";
        isMRSCylinderMoving = "0";
        isMLSCylinderMoving = "0";
        
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
                // Auto
                if(isAuto == 1)
                {
                    pushCylinder.GetComponent<Cylinder>().PLCInput1 = yDataBlock[1];
                    pushCylinder.GetComponent<Cylinder>().PLCInput2 = yDataBlock[20];
                    ShieldCylinder.GetComponent<Cylinder>().PLCInput1 = yDataBlock[23];
                    ShieldCylinder.GetComponent<Cylinder>().PLCInput2 = yDataBlock[21];
                    Conveyor.GetComponent<Conveyor>().PLCInput1 = yDataBlock[160];
                    Conveyor.GetComponent<Conveyor>().PLCInput2 = yDataBlock[0];

                }

                // Manual
                if(isManual == 1)
                {
                    pushCylinder.GetComponent<Cylinder>().PLCInput1 = yDataBlock[92];
                    pushCylinder.GetComponent<Cylinder>().PLCInput2 = yDataBlock[93];
                    ShieldCylinder.GetComponent<Cylinder>().PLCInput1 = yDataBlock[94];
                    ShieldCylinder.GetComponent<Cylinder>().PLCInput2 = yDataBlock[95];
                    Conveyor.GetComponent<Conveyor>().PLCInput1 = yDataBlock[90];
                    Conveyor.GetComponent<Conveyor>().PLCInput3 = yDataBlock[91];
                }


                print(yDataBlock);
            }
            
            Sensor(pushCylinderSensor1, "X10");
            Sensor(pushCylinderSensor2, "X11");
            Sensor(pushCylinderSensor3, "X12");
            Sensor(pushCylinderSensor4, "X13");
            Sensor(ShieldCylinderSensor1, "X30");
            Sensor(PushSensor, "X20");
            Sensor(ShieldSensor1, "X1");
            Sensor(ShieldSensor2, "X2");
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
        byte[] buffer = new byte[320];
        stream.Read(buffer, 0, 320);
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
    public void OnConveyorBtnClkEvent()
    {
        Write($"W,X0,1,");
    }
    public void OnAutoProcessBtnClkEvent()
    {
        Write($"W,X99,1,");
        isAuto = 1;
        isManual = 0;
    }
    public void OnManualProcessBtnClkEvent()
    {
        Write($"W,X4,1,");
        isAuto = 0;
        isManual = 1;
    }
    public void MRConveyorBtnClkEvent()
    {
        if (isMRConveyorMoving == "1")
            isMRConveyorMoving = "0";
        else
            isMRConveyorMoving = "1";
        Write($"W,X90,{isMRConveyorMoving},");
    }
    public void MLConveyorBtnClkEvent()
    {
        if (isMLConveyorMoving == "1")
            isMLConveyorMoving = "0";
        else
            isMLConveyorMoving = "1";
        Write($"W,X91,{isMLConveyorMoving},");
    }
    public void MRPCylinderBtnClkEvent()
    {
        if (isMRPCylinderMoving == "1")
            isMRPCylinderMoving = "0";
        else
            isMRPCylinderMoving = "1";
        Write($"W,X92,{isMRPCylinderMoving},");
    }
    public void MLPCylinderBtnClkEvent()
    {
        if (isMLPCylinderMoving == "1")
            isMLPCylinderMoving = "0";
        else
            isMLPCylinderMoving = "1";
        Write($"W,X93,{isMLPCylinderMoving},");
    }
    public void MRSCylinderBtnClkEvent()
    {
        if (isMRSCylinderMoving == "1")
            isMRSCylinderMoving = "0";
        else
            isMRSCylinderMoving = "1";
        Write($"W,X94,{isMRSCylinderMoving},");
    }
    public void MLSCylinderBtnClkEvent()
    {
        if (isMLSCylinderMoving == "1")
            isMLSCylinderMoving = "0";
        else
            isMLSCylinderMoving = "1";
        Write($"W,X95,{isMLSCylinderMoving},");
    }
}
