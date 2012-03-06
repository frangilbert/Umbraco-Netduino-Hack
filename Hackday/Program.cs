using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Gsiot.PachubeClient;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;


namespace Hackday
{
    public class Program
    {
        public static void Main()
        {
            var led = new OutputPort(Pins.ONBOARD_LED, false);
            while(true)
            {
                led.Write(false);
                var requestUri = "http://dev3.aquepreview.com/helicoptersurface";
                Debug.Print("Setup");

                using (var request = (HttpWebRequest)WebRequest.Create(requestUri))
                {
                    request.Method = "GET";
                    Debug.Print("Requesting");

                    // send request and receive response
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        HttpStatusCode status = response.StatusCode;
                        if (status == HttpStatusCode.OK)
                        {
                            var pwm = new PWM(Pins.GPIO_PIN_D5);
                            Debug.Print("200, all ok");
                            pwm.SetDutyCycle(1000);
                            led.Write(true);
                        }
                    }
                }

                Thread.Sleep(2000);
            }
        }
    }
}
