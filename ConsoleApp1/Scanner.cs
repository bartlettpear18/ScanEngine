using System;
using System.Collections;
using CoreScanner;

namespace ConsoleApp1
{
    internal class Scanner
    {
        //Core Scanner stuff
        static CCoreScanner coreScanner;
        static string barcode;
        static Boolean barcodeIsRead = false;


        //Properties for Scanner Object
        private String model;
        private ArrayList runtime = new ArrayList();
        private static ArrayList consistency = new ArrayList();

        public Scanner(String modelName)
        {
            model = modelName;
            coreScanner = new CoreScanner.CCoreScanner();
            open();
            subscribeBarcode();

        }


        //Scanner Object Accessor and Mutator methods
        /// <summary>
        ///  returns the model name of scan engine
        /// </summary>
        /// <returns>model name</returns>
        public String getModel()
        {
            return model;
        }

        /// <summary>
        /// prints the execution time for each run
        /// </summary>
        public void printRuntime()
        {
            foreach (int i in runtime)
            {
                Console.WriteLine(i);
            }
        }

        /// <summary>
        /// prints the accuracy for each run - this is a stupid feature right now
        /// </summary>
        public void printConsistency()
        {
            foreach (int i in consistency)
            {
                Console.WriteLine(i);
            }
        }

        //ignore outliers
        /// <summary>
        /// determines the average execution time for the scan engine
        /// </summary>
        /// <returns>average execution time</returns>
        public double averageRuntime()
        {
            int total = 0;
            foreach (int i in runtime)
            {
                total += i;
            }

            return total / (runtime.Count);
        }

        /// <summary>
        /// determines the percentage success of scan engine
        /// </summary>
        /// <returns>average success</returns>
        public double averageConsistency()
        {
            int total = 0;
            foreach (int i in runtime)
            {
                total += i;
            }

            return total / (consistency.Count);
        }

        /// <summary>
        /// adds an execution time to array list
        /// </summary>
        /// <param name="ms"></param>
        public void addRuntime(int ms)
        {
            runtime.Add(ms);
        }

        /// <summary>
        /// add a consistency elemeent to array list
        /// </summary>
        /// <param name="i"></param>
        public void addConsistency(int i)
        {
            consistency.Add(i);
        }


        //Scan Engine methods

        //Open API
        public void open()
        {
            short[] scannerTypes = new short[1];
            scannerTypes[0] = 1;
            short numScanTypes = 1;
            int status;

            coreScanner.Open(0, scannerTypes, numScanTypes, out status);

        }

        //Subscribe to Barcode events
        public void subscribeBarcode()
        {
            coreScanner.BarcodeEvent += new _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEvent);

        }

        //Print Barcode Scan Data
        public void readScan()
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
        }

        public void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            barcode = pscanData;
            barcodeIsRead = true;
        }

        //Soft Trigger Pull
        public void pullTrigger()
        {
            int opCode = 2011; //DEVICE_PULL_TRIGGER value
            int status;
            string outXML;
            string inXML = "<inArgs>" +
                               "<scannerID >1</scannerID>" +
                           "</inArgs>";
            coreScanner.ExecCommand(opCode, ref inXML, out outXML, out status);
        }

        //Soft Trigger Release
        public void releaseTrigger()
        {
            int opCode = 2012; //DEVICE_RELEASE_TRIGGER value
            int status;
            string outXML;
            string inXML = "<inArgs>" +
                               "<scannerID >1</scannerID>" +
                           "</inArgs>";
            coreScanner.ExecCommand(opCode, ref inXML, out outXML, out status);
        }

        //Scan a barcode
        public void scan()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            do
            {
                pullTrigger();
                readScan();
            } while (!barcodeIsRead);
            releaseTrigger();

            watch.Stop();
            int elapsedMs = (int) watch.ElapsedMilliseconds;
            addConsistency(1);
            addRuntime(elapsedMs);
            Console.WriteLine(elapsedMs);
            //Console.WriteLine(barcode);

        }

        //Run tests on scan engine
        public void runTests()
        {

            int numTests = 5;
            for (int i = 0; i < numTests; i++)
            {
                scan();
                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}

