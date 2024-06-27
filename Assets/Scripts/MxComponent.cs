using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Linq;

public class MxComponent : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    bool isTCPConnecting;
    bool isPLCConnecting;

    public GameObject pushCylinder;
    public GameObject ShieldCylinder;
    public GameObject Conveyor;
    public GameObject XServoMotor;
    public GameObject YServoMotor;
    public GameObject ZServoMotor;
    public GameObject Rotate;
    public GameObject ForkCylinder;
    public GameObject Fork;

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

    public string preDDataBlock;
    public string dDataBlock;
    public int[] dDataArray = new int[10];



    public int isManual;
    public int isAuto;
    public string isMRConveyorMoving;
    public string isMLConveyorMoving;
    public string isMRPCylinderMoving;
    public string isMLPCylinderMoving;
    public string isMRSCylinderMoving;
    public string isMLSCylinderMoving;
    public string isMRXTransfering;
    public string isMLXTransfering;
    public string isMRYTransfering;
    public string isMLYTransfering;
    public string isMRZTransfering;
    public string isMLZTransfering;
    public string isMRFCylinderMoving;
    public string isMLFCylinderMoving;
    public string isMRRotating;
    public string isMLRotating;

    void Start()
    {
        isTCPConnecting = false;
        isPLCConnecting = false;
        isMRConveyorMoving = "0";
        isMLConveyorMoving = "0";
        isMRSCylinderMoving = "0";
        isMLSCylinderMoving = "0";
        
    }

    public void OnDestroy()
    {
        DisconnectPLC();
        DisConnectTCPServer();
    }
    void Update()
    {
        if (isTCPConnecting && isPLCConnecting)
        {
            preYDataBlock = yDataBlock;
            preDDataBlock = dDataBlock;
            Write("RY,");
            ReadY();
            Write("RD,");
            ReadD();
            if(preYDataBlock != yDataBlock || preDDataBlock != dDataBlock)
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
                    XServoMotor.GetComponent<Transfer>().PLCInput1 = yDataBlock[170];
                    XServoMotor.GetComponent<Transfer>().PLCInput2 = yDataBlock[171];
                    XServoMotor.GetComponent<Transfer>().PLCInput3 = dDataArray[0];
                    XServoMotor.GetComponent<Transfer>().PLCInput4 = dDataArray[1];
                    YServoMotor.GetComponent<Transfer>().PLCInput1 = yDataBlock[172];
                    YServoMotor.GetComponent<Transfer>().PLCInput2 = yDataBlock[173];
                    YServoMotor.GetComponent<Transfer>().PLCInput3 = dDataArray[2];
                    YServoMotor.GetComponent<Transfer>().PLCInput4 = dDataArray[3];
                    ZServoMotor.GetComponent<Transfer>().PLCInput1 = yDataBlock[174];
                    ZServoMotor.GetComponent<Transfer>().PLCInput2 = yDataBlock[175];
                    ZServoMotor.GetComponent<Transfer>().PLCInput3 = dDataArray[4];
                    ZServoMotor.GetComponent<Transfer>().PLCInput4 = dDataArray[5];
                    ForkCylinder.GetComponent<Cylinder>().PLCInput1 = yDataBlock[176];
                    ForkCylinder.GetComponent<Cylinder>().PLCInput2 = yDataBlock[177];
                    Rotate.GetComponent<Rotate>().PLCInput1 = yDataBlock[178];
                    Rotate.GetComponent<Rotate>().PLCInput2 = yDataBlock[179];
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
                    XServoMotor.GetComponent<Transfer>().PLCInput1 = yDataBlock[40];
                    XServoMotor.GetComponent<Transfer>().PLCInput2 = yDataBlock[41];
                    YServoMotor.GetComponent<Transfer>().PLCInput1 = yDataBlock[42];
                    YServoMotor.GetComponent<Transfer>().PLCInput2 = yDataBlock[43];
                    ZServoMotor.GetComponent<Transfer>().PLCInput1 = yDataBlock[44];
                    ZServoMotor.GetComponent<Transfer>().PLCInput2 = yDataBlock[45];
                    ForkCylinder.GetComponent<Cylinder>().PLCInput1 = yDataBlock[46];
                    ForkCylinder.GetComponent<Cylinder>().PLCInput2 = yDataBlock[47];
                    Rotate.GetComponent<Rotate>().PLCInput1 = yDataBlock[48];
                    Rotate.GetComponent<Rotate>().PLCInput2 = yDataBlock[49];

                }
            
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
    public void ReadY()
    {
        byte[] buffer = new byte[320];
        stream.Read(buffer, 0, 320);
        yDataBlock = Encoding.ASCII.GetString(buffer);
    }
    public void ReadD()
    {
        byte[] buffer = new byte[160];
        stream.Read(buffer, 0, 160);
        dDataBlock = Encoding.ASCII.GetString(buffer);

        for(int i = 0; i * 16 < dDataBlock.Length; i++)
        {
            string segment = new string(dDataBlock.Substring(i * 16, 16).Reverse().ToArray());
            dDataArray[i] = Convert.ToInt32(segment, 2);
        }
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

    public void DisConnectTCPServer()
    {
        Write("CS,");
        print("TCP 서버 연결 해제 완료");
        isTCPConnecting = false;
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
        Write($"W,X4,0,");
        Write($"W,X0,0,");
        isAuto = 1;
        isManual = 0;

        XServoMotor.GetComponent<Transfer>().location = 0.12f;
        XServoMotor.GetComponent<Transform>().localPosition = new Vector3(-1.58f, 0, -0.2f);
        YServoMotor.GetComponent<Transfer>().location = 3.36f;
        YServoMotor.GetComponent<Transform>().localPosition = new Vector3(1.8f, 0.45f, 2.9f);
        ZServoMotor.GetComponent<Transfer>().location = 0.32f;
        ZServoMotor.GetComponent<Transform>().localPosition = new Vector3(-1.43f, -0.4f, -1.95f);
        ForkCylinder.GetComponent<Cylinder>().location = 0.23f;
        ForkCylinder.GetComponent<Cylinder>().piston.GetComponent<Transform>().localPosition = new Vector3(-0.03f, 0, 0);
        Rotate.GetComponent<Rotate>().angle = 0;
        Rotate.GetComponent<Rotate>().RotateComponent.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0,0,0));
        if(Fork.GetComponent<Fork>().Box != null)
            Destroy(Fork.GetComponent<Fork>().Box);

    }
    public void OnManualProcessBtnClkEvent()
    {
        Write($"W,X4,1,");
        Write($"W,X99,0,");
        isAuto = 0;
        isManual = 1;
    }
    public void MRConveyorBtnClkEvent()
    {
        if (isMRConveyorMoving == "1")
            isMRConveyorMoving = "0";
        else
        {
            isMRConveyorMoving = "1";
            isMLConveyorMoving = "0";
        }
        Write($"W,X90,{isMRConveyorMoving},");
        Write($"W,X91,{isMLConveyorMoving},");
    }
    public void MLConveyorBtnClkEvent()
    {
        if (isMLConveyorMoving == "1")
            isMLConveyorMoving = "0";
        else
        {
            isMLConveyorMoving = "1";
            isMRConveyorMoving = "0";
        }
        Write($"W,X91,{isMLConveyorMoving},");
        Write($"W,X90,{isMRConveyorMoving},");
    }
    public void MRPCylinderBtnClkEvent()
    {
        isMRPCylinderMoving = "1";
        isMLPCylinderMoving = "0";
        Write($"W,X92,{isMRPCylinderMoving},");
        Write($"W,X93,{isMLPCylinderMoving},");
    }
    public void MLPCylinderBtnClkEvent()
    {
        isMLPCylinderMoving = "1";
        isMRPCylinderMoving = "0";
        Write($"W,X93,{isMLPCylinderMoving},");
        Write($"W,X92,{isMRPCylinderMoving},");
    }
    public void MRSCylinderBtnClkEvent()
    {
        isMRSCylinderMoving = "1";
        isMLSCylinderMoving = "0";
        Write($"W,X94,{isMRSCylinderMoving},");
        Write($"W,X95,{isMLSCylinderMoving},");
    }
    public void MLSCylinderBtnClkEvent()
    {
        isMLSCylinderMoving = "1";
        isMRSCylinderMoving = "0";
        Write($"W,X95,{isMLSCylinderMoving},");
        Write($"W,X94,{isMRSCylinderMoving},");
    }
    public void MRXTransferBtnClkEvent()
    {
        if(isMRXTransfering == "1")
            isMRXTransfering = "0";
        else
        {
            isMRXTransfering = "1";
            isMLXTransfering = "0";
        }
        Write($"W,X40,{isMRXTransfering},");
        Write($"W,X41,{isMLXTransfering},");
    }
    public void MLXTransferBtnClkEvent()
    {
        if (isMLXTransfering == "1")
            isMLXTransfering = "0";
        else
        {
            isMLXTransfering = "1";
            isMRXTransfering = "0";
        }
        Write($"W,X41,{isMLXTransfering},");
        Write($"W,X40,{isMRXTransfering},");
    }
    public void MRYTransferBtnClkEvent()
    {
        if (isMRYTransfering == "1")
            isMRYTransfering = "0";
        else
        {
            isMRYTransfering = "1";
            isMLYTransfering = "0";
        }
        Write($"W,X42,{isMRYTransfering},");
        Write($"W,X43,{isMLYTransfering},");
    }
    public void MLYTransferBtnClkEvent()
    {
        if (isMLYTransfering == "1")
            isMLYTransfering = "0";
        else
        {
            isMLYTransfering = "1";
            isMRYTransfering = "0";
        }
        Write($"W,X43,{isMLYTransfering},");
        Write($"W,X42,{isMRYTransfering},");
    }
    public void MRZTransferBtnClkEvent()
    {
        if (isMRZTransfering == "1")
            isMRZTransfering = "0";
        else
        {
            isMRZTransfering = "1";
            isMLZTransfering = "0";
        }
        Write($"W,X44,{isMRZTransfering},");
        Write($"W,X45,{isMLZTransfering},");
    }
    public void MLZTransferBtnClkEvent()
    {
        if (isMLZTransfering == "1")
            isMLZTransfering = "0";
        else
        {
            isMLZTransfering = "1";
            isMRZTransfering = "0";
        }
        Write($"W,X45,{isMLZTransfering},");
        Write($"W,X44,{isMRZTransfering},");
    }
    public void MRFCylinderBtnClkEvent()
    {
        isMRFCylinderMoving = "1";
        isMLFCylinderMoving = "0";
        Write($"W,X46,{isMRFCylinderMoving},");
        Write($"W,X47,{isMLFCylinderMoving},");
    }
    public void MLFCylinderBtnClkEvent()
    {
        isMLFCylinderMoving = "1";
        isMRFCylinderMoving = "0";
        Write($"W,X47,{isMLFCylinderMoving},");
        Write($"W,X46,{isMRFCylinderMoving},");
    }
    public void MRRotateBtnClkEvent()
    {
        isMRRotating = "1";
        isMLRotating = "0";
        Write($"W,X48,{isMRRotating},");
        Write($"W,X49,{isMLRotating},");
    }
    public void MLRotateBtnClkEvent()
    {
        isMLRotating = "1";
        isMRRotating = "0";
        Write($"W,X49,{isMLRotating},");
        Write($"W,X48,{isMRRotating},");
    }
}
