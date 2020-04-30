using SiecNeuronowa2.ActivationFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiecNeuronowa2.NeuralNetwork
{
    class Neuron
    {
        public List<Synapse> Inputs { get; set; } = new List<Synapse>();
        public List<Synapse> Outputs { get; set; } = new List<Synapse>();

        public IActivationFunction ActivationFunction { get; set; }

        public double InputValue { get; set; }
        public double OutputValue { get; set; }

        public Neuron(IActivationFunction activationFunction)
        {
            ActivationFunction = activationFunction;
        }

        public void CalculateOutputs(double sum)
        {
            InputValue = sum;
            OutputValue = ActivationFunction.Calculate(InputValue);
        }
    }
}
