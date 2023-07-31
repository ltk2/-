using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using ThingMagic;

namespace ModuleReaderManager
{
    public class Comm
    {
        public delegate void EventHandle(byte[] readBuffer);
        //public event EventHandle DataReceived;

        public SerialPort serialPort;

        public Comm()
        {
            serialPort = new SerialPort();
        }

        public bool IsOpen
        {
            get
            {
                return serialPort.IsOpen;
            }
        }

        public int ReadAll(byte[] buffer, int offset, int count)
        {
            try
            {
                int bytesReceived = 0;
                int exitLoop = 0;
                while (bytesReceived < count && exitLoop == 0)
                    bytesReceived += serialPort.Read(buffer, offset + bytesReceived, count - bytesReceived);

                return bytesReceived;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public void Open()
        {
            Close();
            serialPort.Open();
        }

        public void Close()
        {
            serialPort.Close();
        }

        public byte[] WritePort(byte[] item)
        {
            if (IsOpen)
            {
                byte[] m5eMessage = new byte[2 + item.Length + 2];
                m5eMessage[0] = 0xFF;
                m5eMessage[1] = (byte)((item.Length) - 1);
                Array.Copy(item, 0, m5eMessage, 2, item.Length);
                byte[] crc = new byte[2];
                crc = calcCRC(m5eMessage);
                Array.Copy(crc, 0, m5eMessage, ((item.Length) + 2), 2);

                var m5etext = "";
                for (int i = 0; i < m5eMessage.Length; i++)
                {
                    m5etext += (m5eMessage[i].ToString("X2"));
                }
                Debug.WriteLine(serialPort.PortName + "发送指令:" + m5etext);
                serialPort.Write(m5eMessage, 0, m5eMessage.Length);

                byte[] inputBuffer = new byte[256];
                int ibuf = 0;

                ibuf += ReadAll(inputBuffer, ibuf, 1);
                if (ibuf == 0)
                    return null;

                ibuf += ReadAll(inputBuffer, ibuf, 1);

                byte argLength = inputBuffer[1];
                int responseLength = argLength + 1 + 1 + 1 + 2 + 2;

                ibuf += ReadAll(inputBuffer, ibuf, responseLength - 2);

                byte[] returnCRC = new byte[2];
                byte[] inputBufferNoCRC = new byte[(responseLength - 2)];
                Array.Copy(inputBuffer, 0, inputBufferNoCRC, 0, (responseLength - 2));

                returnCRC = calcReturnCRC(inputBufferNoCRC);
                if (ByteConv.ToU16(inputBuffer, (responseLength - 2)) == ByteConv.ToU16(returnCRC, 0))
                {
                    string text = "";
                    for (int i = 0; i < inputBufferNoCRC.Length; i++)
                    {
                        text += inputBufferNoCRC[i].ToString("X2");
                    }
                    Debug.WriteLine(serialPort.PortName + "接收指令:" + text);
                    return inputBufferNoCRC;
                }
            }
            return null;
        }

        public static byte[] calcCRC(byte[] command)
        {
            UInt16 tempcalcCRC1 = calcCRC8(65535, command[1]);
            tempcalcCRC1 = calcCRC8(tempcalcCRC1, command[2]);
            byte[] CRC = new byte[2];
            if (command[1] != 0)
            {
                for (int i = 0; i < command[1]; i++)
                {
                    tempcalcCRC1 = calcCRC8(tempcalcCRC1, command[3 + i]);
                }
            }
            CRC = BitConverter.GetBytes(tempcalcCRC1);
            Array.Reverse(CRC);
            return CRC;
        }

        public static UInt16 calcCRC8(UInt16 beginner, byte ch)
        {
            byte[] tempByteArray;
            byte xorFlag;
            byte element80 = new byte();
            element80 = 0x80;
            byte chAndelement80 = new byte();
            bool[] forxorFlag = new bool[16];
            for (int i = 0; i < 8; i++)
            {
                tempByteArray = BitConverter.GetBytes(beginner);
                Array.Reverse(tempByteArray);
                BitArray tempBitArray = new BitArray(tempByteArray);
                for (int j = 0; j < tempBitArray.Count; j++)
                {
                    forxorFlag[j] = tempBitArray[j];
                }
                Array.Reverse(forxorFlag, 0, 8);
                Array.Reverse(forxorFlag, 8, 8);
                for (int k = 0; k < tempBitArray.Count; k++)
                {
                    tempBitArray[k] = forxorFlag[k];
                }
                xorFlag = BitConverter.GetBytes(tempBitArray.Get(0))[0];
                beginner = (UInt16)(beginner << 1);
                chAndelement80 = (byte)(ch & element80);
                if (chAndelement80 != 0)
                {
                    ++beginner;
                }
                if (xorFlag != 0)
                {
                    beginner = (UInt16)(beginner ^ 0x1021);
                }
                element80 = (byte)(element80 >> 1);
            }

            return beginner;
        }

        public static byte[] calcReturnCRC(byte[] command)
        {
            UInt16 tempcalcCRC1 = calcCRC8(65535, command[1]);
            tempcalcCRC1 = calcCRC8(tempcalcCRC1, command[2]);
            byte[] CRC = new byte[2];
            //if (command[1] != 0)
            {
                for (int i = 0; i < (command[1] + 2); i++)
                {
                    tempcalcCRC1 = calcCRC8(tempcalcCRC1, command[3 + i]);
                }
            }
            CRC = BitConverter.GetBytes(tempcalcCRC1);
            Array.Reverse(CRC);
            return CRC;
        }

    }
}
