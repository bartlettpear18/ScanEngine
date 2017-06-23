using System;
using System.Collections;
using CoreScanner;

public class Scanner
{
    //Core Scanner stuff
    static CCoreScanner coreScanner;
    static string barcode;
    static Boolean barcodeIsRead = false;


    //Properties for Scanner Object
    private String model;
    private ArrayList runtime = new ArrayList();
    private ArrayList accuracy = new ArrayList();
     
	public Scanner(String modelName) 
	{
        model = modelName;
        coreScanner = new CoreScanner.CCoreScanner();
        open();
        subscribeBarcode();

    }

    public String getModel()
    {
        return model;
    }

    public void printRuntime()
    {
        foreach (int i in runtime)
        {
            Console.WriteLine(i);
        }
    }

    public void printAccuracy()
    {
        foreach (int i in accuracy)
        {
            Console.WriteLine(i);
        }
    }

    public double averageRuntime()
    {
        int total = 0;
        foreach (int i in runtime)
        {
            total += i;
        }

        return total / (runtime.length);
    }

    public double accuracy()
    {
        int total = 0;
        foreach (int i in runtime)
        {
            total += i;
        }

        return total / (accuracy.length);
    }


    public void addRuntime(int ms)
    {
        runtime.Add(ms);
    }

    public void addAccuracy(int i)
    {
        accuracy.addAccuracy(i);
    }





}
