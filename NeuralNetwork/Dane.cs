using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SiecNeuronowa2.NeuralNetwork
{
    class Dane
    {
        public double[][] Pobierz(string nazwa_pliku)
        {
            string[] lines = File.ReadAllLines(nazwa_pliku);
            double[][] data = new double[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');
                data[i] = new double[tmp.Length + 2];
                for (int j = 0; j < tmp.Length - 1; j++)
                {
                    data[i][j] = Convert.ToDouble(tmp[j].Replace('.', ','));
                    if (tmp[4] == "Iris-setosa")
                    {
                        data[i][4] = 1;
                        data[i][5] = 0;
                        data[i][6] = 0;
                    }
                    else if (tmp[4] == "Iris-versicolor")
                    {
                        data[i][4] = 0;
                        data[i][5] = 1;
                        data[i][6] = 0;
                    }
                    else
                    {
                        data[i][4] = 0;
                        data[i][5] = 0;
                        data[i][6] = 1;
                    }


                }
            }

            return data;
        }

        public double[][] Tasowanie(double[][] tablica)
        {
            double[][] tab2 = new double[tablica.Length][];
            for (int i = 0; i < tablica.Length; i++)
            {
                tab2[i] = new double[tablica[0].Length];
            }

            int n = tab2.Length;
            Random x = new Random();

            List<int> wylosowane = new List<int>();

            for (int i = 0; i < n; i++)
            {
                int wylosowana = x.Next(0, n);
                while (wylosowane.Contains(wylosowana))
                {
                    wylosowana = x.Next(0, n);
                }

                wylosowane.Add(wylosowana);

                tab2[i] = tablica[wylosowana];
            }

            return tab2;
        }

        public double[][] Normalizuj(double[][] tablica,params double[][] przyklad)
        {
            double nmin = 0;
            double nmax = 1;

            for (int i = 0; i < 4; i++)
            {
                double max = tablica[0][i];
                double min = tablica[0][i];
                for (int j = 0; j < tablica.Length; j++)
                {
                    if (max < tablica[j][i])
                        max = tablica[j][i];
                    if (min > tablica[j][i])
                        min = tablica[j][i];
                }

                for (int j = 0; j < tablica.Length; j++)
                {
                    tablica[j][i] = ((tablica[j][i] - min) / (max - min)) * (nmax - nmin) + nmin;
                }

                for(int j=0; j<przyklad.Length; j++)
                    przyklad[j][i] = ((przyklad[j][i] - min) / (max - min)) * (nmax - nmin) + nmin;
            }

            return tablica;
        }
    }
}
