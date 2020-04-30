using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiecNeuronowa2.ActivationFunction
{
    interface IActivationFunction
    {
        double Calculate(double value);
        double Derivative(double value);
    }
}
