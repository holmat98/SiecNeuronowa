using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiecNeuronowa2
{
    using SiecNeuronowa2.ActivationFunction;
    using SiecNeuronowa2.NeuralNetwork;
    class Program
    {
        static void Main(string[] args)
        {
            Dane dane = new Dane();
            double[] przyklad = new double[4] { 5, 3.4, 1.5, 0.3 };
            double[] przyklad2 = new double[4] { 6.5, 2.8, 4.7, 1.4 };
            double[] przyklad3 = new double[4] { 6.5, 3.0, 5.2, 2.0 };

            double[][] inputData = dane.Pobierz("baza_irysow.txt");
            inputData = dane.Normalizuj(inputData, przyklad, przyklad2, przyklad3);
            inputData = dane.Tasowanie(inputData);

            Network network = new Network(3, 4, 3, 1, new SigmoidFunction());
            network.Train(inputData, 4000);

            Console.WriteLine("Przykład 1:");
            network.GetOutput(przyklad);
            Console.WriteLine("Przykład 2:");
            network.GetOutput(przyklad2);
            Console.WriteLine("Przykład 3:");
            network.GetOutput(przyklad3);

            Console.ReadKey();
        }
    }
}
