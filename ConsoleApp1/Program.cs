using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreScanner;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace ConsoleApp1
{
    class Program
    {
        //Declare CoreScaner class
        static CCoreScanner coreScanner;
        static string barcode;
        static Boolean barcodeIsRead = false;

        static void Main(string[] args)
        {
            //Instantiate Core Scanner Class
            coreScanner = new CoreScanner.CCoreScanner();
            open();
            subscribeBarcode();

            scan();
        }

      


        public static void open()
        {
            short[] scannerTypes = new short[1];
            scannerTypes[0] = 1;
            short numScanTypes = 1;
            int status;

            coreScanner.Open(0, scannerTypes, numScanTypes, out status);

        }

        public static void getScanners()
        {
            short numScanners;
            int[] connectedScanners = new int[255];
            int status;

            string outXML;

            coreScanner.GetScanners(out numScanners, connectedScanners, out outXML, out status);
            Console.WriteLine(outXML);

        }

        public static void subscribeBarcode()
        {
            coreScanner.BarcodeEvent += new _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEvent);

        }

        public static void printScan()
        {

            int opCode = 1001;
            string outXML;
            int status;
            string inXML = "<inArgs>" +
                                "<cmdArgs>" +
                                    "<arg-int>1</arg-int>" + // Number of events you want to subscribe
                                    "<arg-int>1</arg-int>" + // Comma separated event IDs
                                "</cmdArgs>" +
                            "</inArgs>";

            coreScanner.ExecCommand(opCode, ref inXML, out outXML, out status);
            //Console.WriteLine(outXML);
            //Console.WriteLine(barcode);
        }

        public static void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            barcode = pscanData;
            barcodeIsRead = true;
        }

        public static void pullTrigger()
        {
            int opCode = 2011; //DEVICE_PULL_TRIGGER value
            int status;
            string outXML;
            string inXML = "<inArgs>" +
                               "<scannerID >1</scannerID>" +
                           "</inArgs>";
            coreScanner.ExecCommand(opCode, ref inXML, out outXML, out status);
            //Console.WriteLine(outXML);
        }

        public static void releaseTrigger()
        {
            int opCode = 2012; //DEVICE_RELEASE_TRIGGER value
            int status;
            string outXML;
            string inXML = "<inArgs>" +
                               "<scannerID >1</scannerID>" +
                           "</inArgs>";
            coreScanner.ExecCommand(opCode, ref inXML, out outXML, out status);
            Console.WriteLine(outXML);
        }

        public static void aim()
        {
            int opCode = 2003; //AIM_ON value
            int status;
            string outXML;
            string inXML = "<inArgs>" +
                               "<scannerID >1</scannerID>" +
                           "</inArgs>";
            coreScanner.ExecCommand(opCode, ref inXML, out outXML, out status);
            //Console.WriteLine(outXML);
        }

        public static void aimOff()
        {
            int opCode = 2002; //AIM_ON value
            int status;
            string outXML;
            string inXML = "<inArgs>" +
                               "<scannerID >1</scannerID>" +
                           "</inArgs>";
            coreScanner.ExecCommand(opCode, ref inXML, out outXML, out status);
            //Console.WriteLine(outXML);
        }
        
        public static void scan()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            do
            {
                pullTrigger();
                printScan();
            } while (!barcodeIsRead);
            releaseTrigger();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var seconds = elapsedMs / 1000;
            Console.WriteLine(elapsedMs);
            //Console.WriteLine(barcode);
            //Console.WriteLine("Scan completed");

        }

        public static void runTests()
        {

            int numTests = 20;
            for (int i =0; i < numTests; i++)
            {
                scan();
                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}
