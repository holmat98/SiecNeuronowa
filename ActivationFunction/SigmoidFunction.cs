using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiecNeuronowa2.ActivationFunction
{
    class SigmoidFunction : IActivationFunction
    {
        public double Calculate(double value)
        {
            return 1 / (1 + Math.Exp(-value));
        }

        public double Derivative(double value)
        {
            return Calculate(value) * (1 - Calculate(value));
        }
    }
}
