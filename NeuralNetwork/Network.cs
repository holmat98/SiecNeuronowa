using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using SiecNeuronowa2.ActivationFunction;

namespace SiecNeuronowa2.NeuralNetwork
{
    class Network
    {
        private List<Layer> layers = new List<Layer>();
        private double learningRate;

        #region Creating Neural Network
        public Network(double layersNumber, double inputNeuronsNumber, double outputNeuronsNumber, double learningRate, IActivationFunction activationFunction)
        {
            this.learningRate = learningRate;
            for(int i=0; i<layersNumber-1; i++)
            {
                layers.Add(new Layer());
                for(int j=0; j<inputNeuronsNumber; j++)
                {
                    layers[i].Neurons.Add(new Neuron(activationFunction));
                }
            }
            layers.Add(new Layer());
            for(int i=0; i<outputNeuronsNumber; i++)
            {
                layers.Last().Neurons.Add(new Neuron(activationFunction));
            }

            CreateSynapses();
        }

        private void CreateSynapses()
        {
            var rnd = new Random();
            for(int i=1; i<layers.Count; i++)
            {
                for(int j=0; j<layers[i].Neurons.Count; j++)
                {
                    for(int k=0; k<layers[i-1].Neurons.Count; k++)
                    {
                        Synapse newSynapse = new Synapse(Math.Round(rnd.NextDouble(), 2), layers[i - 1].Neurons[k], layers[i].Neurons[j]);
                        layers[i - 1].Neurons[k].Outputs.Add(newSynapse);
                        layers[i].Neurons[j].Inputs.Add(newSynapse);
                    }
                }
            }
        }

        #endregion

        #region Neural network training and getting output:
        public void Train(double[][] inputData, int iterations)
        {
            int iteration = 0;
            double[][] errors = new double[layers.Count][];

            for (int i = 0; i < layers.Count; i++)
            {
                errors[i] = new double[layers[i].Neurons.Count];
            }

            while (iteration < iterations)
            {
                //int accuracySum = 0;
                for(int i=0; i < inputData.Length; i++)
                {
                    double[] input = new double[4] { inputData[i][0], inputData[i][1], inputData[i][2], inputData[i][3] };
                    double[] expected = new double[3] { inputData[i][4], inputData[i][5], inputData[i][6] };

                    double[] output = Calculate(input);

                    CalculateError(output, expected, errors);
                    UpdateWeights(errors);

                    /*int currentSum = 0;
                    for(int j=0; j<output.Length; j++)
                    {
                        if (output[j] >= 0.6)
                            output[j] = 1;
                        else
                            output[j] = 0;

                        if (output[j] == expected[j])
                            currentSum++;
                    }
                    Console.WriteLine("Przewidywany wynik: " + expected[0] + "" + expected[1] + "" + expected[2] + "| Uzykany wynik: | " + output[0] + "" + output[1] + "" + output[2]);
                    if (currentSum == output.Length)
                        accuracySum++;*/
                }
                iteration++;
                //Console.WriteLine($"AccuracySum: {accuracySum} inputData.Length: {inputData.Length}");
                //Console.WriteLine($"{iteration}) Zgodność: {(accuracySum / (double)inputData.Length)*100}%");
            }
        }

        public void GetOutput(double[] inputData)
        {
            double[] output = Calculate(inputData);
            string wynik = "";
            for(int i=0; i<output.Length; i++)
            {
                if (output[i] >= 0.6)
                    output[i] = 1;
                else
                    output[i] = 0;
                wynik = wynik + output[i].ToString();
            }
            if (wynik == "100")
                Console.WriteLine($"Decyzja: {wynik}, Iris-Setosa");
            else if (wynik == "010")
                Console.WriteLine($"Decyzja: {wynik}, Iris-Versicolor");
            else if (wynik == "001")
                Console.WriteLine($"Decyzja: {wynik}, Iris-Virginica");
            else
                Console.WriteLine($"Decyzja: {wynik}, To nie jest Iris");
        }

        #endregion

        #region Calculating output values
        private void AddInputValues(double[] input)
        {
            for(int i=0; i<input.Length; i++)
            {
                layers[0].Neurons[i].InputValue = layers[0].Neurons[i].OutputValue = input[i];
            }

        }

        private double[] Calculate(double[] input)
        {
            AddInputValues(input);

            for (int i = 1; i < layers.Count; i++)
            {
                for (int j = 0; j < layers[i].Neurons.Count; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < layers[i].Neurons[j].Inputs.Count; k++)
                    {
                        sum += layers[i].Neurons[j].Inputs[k].Weight * layers[i - 1].Neurons[k].OutputValue;
                    }
                    layers[i].Neurons[j].CalculateOutputs(sum);
                }
            }

            double[] output = new double[layers.Last().Neurons.Count];
            for(int i=0; i<output.Length; i++)
            {
                output[i] = layers.Last().Neurons[i].OutputValue;
            }

            return output;
        }

        #endregion

        #region Calculating Error and Updating Weights
        private void CalculateError(double[] output, double[] expected, double[][] errors)
        {
            for (int i = 0; i < output.Length; i++)
            {
                Neuron currentNeuron = layers.Last().Neurons[i];
                errors.Last()[i] = currentNeuron.ActivationFunction.Derivative(currentNeuron.InputValue) * (output[i] - expected[i]);
            }

            for (int i = layers.Count - 2; i > 0; i--)
            {
                for (int j = 0; j < layers[i].Neurons.Count; j++)
                {
                    errors[i][j] = 0;
                    
                    for (int k = 0; k < layers[i + 1].Neurons.Count; k++)
                    {
                        errors[i][j] += errors[i + 1][k] * layers[i + 1].Neurons[k].Inputs[j].Weight;
                    }

                    Neuron currentNeuron = layers[i].Neurons[j];
                    errors[i][j] *= currentNeuron.ActivationFunction.Derivative(currentNeuron.InputValue);
                }
            }
        }

        private void UpdateWeights(double[][] errors)
        {
            for(int i=layers.Count-1; i>0; i--)
            {
                for(int j=0; j<layers[i].Neurons.Count; j++)
                {
                    for(int k=0; k<layers[i-1].Neurons.Count; k++)
                    {
                        double delta = errors[i][j] * layers[i - 1].Neurons[k].OutputValue * (-1);
                        layers[i].Neurons[j].Inputs[k].UpdateWeight(delta, learningRate);
                    }
                }
            }
        }

        #endregion

    }
}
