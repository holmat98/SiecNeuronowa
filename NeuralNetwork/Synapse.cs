using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiecNeuronowa2.NeuralNetwork
{
    class Synapse
    {
        private Neuron fromNeuron;
        private Neuron toNeuron;

        public double Weight { get; set; }

        public Synapse(double weight, Neuron fromNeuron, Neuron toNeuron)
        {
            Weight = weight;
            this.fromNeuron = fromNeuron;
            this.toNeuron = toNeuron;
        }

        public void UpdateWeight(double delta, double learningRate)
        {
            Weight += delta * learningRate;
        }
    }
}
