using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

public class Gloves5DT
{
    //Fingers
    private string Path { get; set; }
    //Right hand
    private string[] Filenames { get; set; }
    private double[][][] Means { get; set; }
    private int[] InfoRight { get; set; }
    private double[] TupleRight { get; set; }
    //Left hand
    private string[] FilenamesLeft { get; set; }
    private double[][][] MeansLeft { get; set; }
    private int[] InfoLeft { get; set; }
    private double[] TupleLeft { get; set; }
    //Past InfoRight
    private int[] InfoLeftAnt { get; set; }
    private int[] InfoRightAnt { get; set; }

    public Gloves5DT(string Path)
    {
        this.Path = Path;
        InitializeClass();
    }

    private void InitializeClass()
    {
        Filenames = new string[5];
        //TODO
        Filenames[0] = Path + "finger13.txt";
        Filenames[1] = Path + "finger23.txt";
        Filenames[2] = Path + "finger33.txt";
        Filenames[3] = Path + "finger43.txt";
        Filenames[4] = Path + "finger53.txt";

        FilenamesLeft = new string[5];
        FilenamesLeft[0] = Path + "finger13l.txt";
        FilenamesLeft[1] = Path + "finger23l.txt";
        FilenamesLeft[2] = Path + "finger33l.txt";
        FilenamesLeft[3] = Path + "finger43l.txt";
        FilenamesLeft[4] = Path + "finger53l.txt";

        TupleRight = new double[14];
        TupleLeft = new double[14];

        InfoLeftAnt = new int[5];
        InfoRightAnt = new int[5];
    }

    public void GetFingersData(string nameLeft, string nameRight)
    {
        //Left Glove
        TupleLeft[0] = VRPN.vrpnAnalog(nameLeft, 0);
        TupleLeft[1] = VRPN.vrpnAnalog(nameLeft, 1);
        TupleLeft[2] = VRPN.vrpnAnalog(nameLeft, 2);
        TupleLeft[3] = VRPN.vrpnAnalog(nameLeft, 3);
        TupleLeft[4] = VRPN.vrpnAnalog(nameLeft, 4);
        TupleLeft[5] = VRPN.vrpnAnalog(nameLeft, 5);
        TupleLeft[6] = VRPN.vrpnAnalog(nameLeft, 6);
        TupleLeft[7] = VRPN.vrpnAnalog(nameLeft, 7);
        TupleLeft[8] = VRPN.vrpnAnalog(nameLeft, 8);
        TupleLeft[9] = VRPN.vrpnAnalog(nameLeft, 9);
        TupleLeft[10] = VRPN.vrpnAnalog(nameLeft, 10);
        TupleLeft[11] = VRPN.vrpnAnalog(nameLeft, 11);
        TupleLeft[12] = VRPN.vrpnAnalog(nameLeft, 12);
        TupleLeft[13] = VRPN.vrpnAnalog(nameLeft, 13);
        MeansLeft = GetMeansFromFile(FilenamesLeft, 2); //TODO
        InfoLeft = TestFingers(TupleLeft, MeansLeft);
        //Right Glove
        TupleRight[0] = VRPN.vrpnAnalog(nameRight, 0);
        TupleRight[1] = VRPN.vrpnAnalog(nameRight, 1);
        TupleRight[2] = VRPN.vrpnAnalog(nameRight, 2);
        TupleRight[3] = VRPN.vrpnAnalog(nameRight, 3);
        TupleRight[4] = VRPN.vrpnAnalog(nameRight, 4);
        TupleRight[5] = VRPN.vrpnAnalog(nameRight, 5);
        TupleRight[6] = VRPN.vrpnAnalog(nameRight, 6);
        TupleRight[7] = VRPN.vrpnAnalog(nameRight, 7);
        TupleRight[8] = VRPN.vrpnAnalog(nameRight, 8);
        TupleRight[9] = VRPN.vrpnAnalog(nameRight, 9);
        TupleRight[10] = VRPN.vrpnAnalog(nameRight, 10);
        TupleRight[11] = VRPN.vrpnAnalog(nameRight, 11);
        TupleRight[12] = VRPN.vrpnAnalog(nameRight, 12);
        TupleRight[13] = VRPN.vrpnAnalog(nameRight, 13);
        Means = GetMeansFromFile(Filenames, 2); //TODO
        InfoRight = TestFingers(TupleRight, Means); //TODO

        //Old data
        if (InfoRight != null && InfoLeft != null)
        {
            InfoRightAnt = InfoRight;
            InfoLeftAnt = InfoLeft;
        }

        /*
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            Debug.Log("Glove raw left = " + String.Join(", ",
                new List<double>(TupleLeft)
                .ConvertAll(i => i.ToString())
                .ToArray()));

            Debug.Log("GloveL = " + String.Join(", ",
                new List<int>(InfoLeft)
                .ConvertAll(i => i.ToString())
                .ToArray()));
            Debug.Log("Glove raw right= " + String.Join(", ",
                new List<double>(TupleRight)
                .ConvertAll(i => i.ToString())
                .ToArray()));

            Debug.Log("GloveR = " + String.Join(", ",
                new List<int>(InfoRight)
                .ConvertAll(i => i.ToString())
                .ToArray()));
        }
         */

    }

    public static double[][][] GetMeansFromFile(string[] Filenames, int numK)
    {
        double[][][] Means = new double[5][][];

        for (int i = 0; i < Filenames.Length; i++)
        {
            Means[i] = new double[numK][];
            string filename = Filenames[i];
            String line; try
            {
                StreamReader sr = new StreamReader(filename);
                line = sr.ReadLine();

                int j = 0;
                while (line != null)
                {
                    Regex reg = new Regex(@"([-+]?[0-9]*\.?[0-9]+)");
                    int tam = 0;
                    foreach (Match match in reg.Matches(line))
                    {
                        tam++;
                    }
                    Means[i][j] = new double[tam];

                    int k = 0;
                    foreach (Match match in reg.Matches(line))
                    {
                        Means[i][j][k] = double.Parse(match.Value, CultureInfo.InvariantCulture.NumberFormat);
                        k++;
                    }

                    j++;
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block files.");
            }
        }

        return Means;
    }

    private static double[] GetFingersTupleRight(double[] TupleRight, int finger)
    {
        double[] r = new double[2];
        finger++;
        switch (finger)
        {
            case 1:
                r[0] = TupleRight[0];
                r[1] = TupleRight[1];
                break;
            case 2:
                r[0] = TupleRight[3];
                r[1] = TupleRight[4];
                break;
            case 3:
                r[0] = TupleRight[6];
                r[1] = TupleRight[7];
                break;
            case 4:
                r[0] = TupleRight[9];
                r[1] = TupleRight[10];
                break;
            case 5:
                r[0] = TupleRight[12];
                r[1] = TupleRight[13];
                break;
            default:
                r[0] = -1;
                r[1] = -1;
                break;
        }
        return r;
    }

    private static int[] TestFingers(double[] TupleRight, double[][][] Means)
    {

        int[] r = new int[5];

        for (int i = 0; i < 5; i++) //num dedos
        {
            int minIndex = 0;
            double min = Distance(GetFingersTupleRight(TupleRight, i), Means[i][0]);
            for (int j = 1; j < Means[i].Length; j++)
            {
                if (Distance(GetFingersTupleRight(TupleRight, i), Means[i][j]) < min)
                {
                    minIndex = j;
                }
            }
            r[i] = TranslateSensor(minIndex, i);
        }
        return r;
    }

    private static int TranslateSensor(int num, int finger)
    {
        //Debug.Log(num);
        int r = 99;
        if (num == 0)
        {
            r = 0;
        }
        else if (num == 1) //TODO
        {
            r = 1;
        }
        else if (num == 2)
        {
            r = -1;
        }
        return r;
    }

    private static double Distance(double[] TupleRight, double[] mean)
    {
        double sumSquaredDiffs = 0.0;
        for (int j = 0; j < TupleRight.Length; ++j)
            sumSquaredDiffs += Math.Pow((TupleRight[j] - mean[j]), 2);
        return Math.Sqrt(sumSquaredDiffs);

    }
}
