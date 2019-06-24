using System;
using System.IO.Ports;
using System.Security.Permissions;
using System.Threading;

namespace ArduinoSlidesAndRotary
{
    public class ArduinoReader
    {
        private SerialPort port;

        Thread readThread;

        int slider1;

        public int Slider1
        {
            get
            {
                return slider1;
            }
            set
            {
                slider1 = value;
            }
        }

        int slider2;

        public int Slider2
        {
            get
            {
                return slider2;
            }
            set
            {
                slider2 = value;
            }
        }

        int slider3;

        public int Slider3
        {
            get
            {
                return slider3;
            }
            set
            {
                slider3 = value;
            }
        }

        int slider4;

        public int Slider4
        {
            get
            {
                return slider4;
            }
            set
            {
                slider4 = value;
            }
        }

        int slider5;

        public int Slider5
        {
            get
            {
                return slider5;
            }
            set
            {
                slider5 = value;
            }
        }

        int slider6;

        public int Slider6
        {
            get
            {
                return slider6;
            }
            set
            {
                slider6 = value;
            }
        }

        int rotary1;

        public int Rotary1
        {
            get
            {
                return rotary1;
            }
            set
            {
                rotary1 = value;
            }
        }

        int press1;

        public int Press1
        {
            get
            {
                return press1;
            }
            set
            {
                press1 = value;
            }
        }

        int rotary2;

        public int Rotary2
        {
            get
            {
                return rotary2;
            }
            set
            {
                rotary2 = value;
            }
        }

        int press2;

        public int Press2
        {
            get
            {
                return press2;
            }
            set
            {
                press2 = value;
            }
        }

        int rotary3;

        public int Rotary3
        {
            get
            {
                return rotary3;
            }
            set
            {
                rotary3 = value;
            }
        }

        int press3;

        public int Press3
        {
            get
            {
                return press3;
            }
            set
            {
                press3 = value;
            }
        }

        private bool shouldRead;

        public ArduinoReader(String portName, int baudRate)
        {
            port = new SerialPort(portName, baudRate);
            port.ReadTimeout = 10;
            port.WriteTimeout = 10;
            port.NewLine = "\n";
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public void Terminate()
        {

            shouldRead = false;
            readThread.Interrupt();
            readThread.Abort();

            port.Close();

            Console.WriteLine("CODEMAX: Thread is alive" + readThread.IsAlive);


        }

        public bool BeginRead()
        {
            try
            {
                port.Open();
            }
            catch (TimeoutException)
            {
                return false;
            }
            shouldRead = true;
            readThread = new Thread(DoPortRead);
            readThread.Start();
            return true;
        }


        //read the values from the Arduino
        private void DoPortRead()
        {
            while (shouldRead)
            {
                try
                {
                    string line = port.ReadLine();
                    string[] values = line.Split(new char[] { ' ' });
                    Slider1 = int.Parse(values[0]);
                    Slider2 = int.Parse(values[1]);
                    Slider3 = int.Parse(values[2]);
                    Slider4 = int.Parse(values[3]);
                    Slider5 = int.Parse(values[4]);
                    Slider6 = int.Parse(values[5]);
                    Rotary1 = int.Parse(values[6]);
                    Press1 = int.Parse(values[7]);
                    Rotary2 = int.Parse(values[8]);
                    Press2 = int.Parse(values[9]);
                    Rotary3 = int.Parse(values[10]);
                    Press3 = int.Parse(values[11]);
                }
                catch (Exception)
                {
                }

            }
        }

        //sends value to the Arduino
        public void SendMessage(int id, int value)
        {

            try
            {
                if (value >= 0 && value <= 9)
                {
                    string message = id.ToString() + ",000" + value.ToString();
                    port.WriteLine(message);
                }
                if (value >= 10 && value <= 99)
                {
                    string message = id.ToString() + ",00" + value.ToString();
                    port.WriteLine(message);
                }
                if (value >= 100 && value <= 999)
                {
                    string message = id.ToString() + ",0" + value.ToString();
                    port.WriteLine(message);
                }
                if (value >= 1000 && value <= 1023)
                {
                    string message = id.ToString() + "," + value.ToString();
                    port.WriteLine(message);
                }
                
            }
            catch (Exception)
            {
            }

        }

    }
}