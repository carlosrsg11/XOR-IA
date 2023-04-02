using System;

namespace BackPropagationXor{
    class Program{
        static void Main(string[] args){
            train();
        }

        class sigmoid{
            public static double salida(double x){
                return 1.0 / (1.0 + Math.Exp(-x));
            }

            public static double derivative(double x){
                return x * (1 - x);
            }
        }

        class Neuron{
            public double[] entradas = new double[2];
            public double[] pesos = new double[2];
            public double error;

            private double biasWeight;
            private Random r = new Random();

            public double salida{
                get { return sigmoid.salida(pesos[0] * entradas[0] + pesos[1] * entradas[1] + biasWeight); }
            }

            public void pesoRandom(){
                pesos[0] = r.NextDouble();
                pesos[1] = r.NextDouble();
                biasWeight = r.NextDouble();
            }

            public void ajustePeso(){
                pesos[0] += error * entradas[0];
                pesos[1] += error * entradas[1];
                biasWeight += error;
            }
        }

        private static void train(){            
            double[,] entradas = {
                { 0, 0},
                { 0, 1},
                { 1, 0},
                { 1, 1}
            };
          
            double[] resultados = { 0, 1, 1, 0 };
           
            Neuron ocultarNeuron1 = new Neuron();
            Neuron ocultarNeuron2 = new Neuron();
            Neuron salidaNeuron = new Neuron();          
            ocultarNeuron1.pesoRandom();
            ocultarNeuron2.pesoRandom();
            salidaNeuron.pesoRandom();

            int cont = 0;

        Retry:
            cont++;
            for (int i = 0; i < 4; i++){                
                ocultarNeuron1.entradas = new double[] { entradas[i, 0], entradas[i, 1] };
                ocultarNeuron2.entradas = new double[] { entradas[i, 0], entradas[i, 1] };
                salidaNeuron.entradas = new double[] { ocultarNeuron1.salida, ocultarNeuron2.salida };
                Console.WriteLine("{0} xor {1} = {2}", entradas[i, 0], entradas[i, 1], salidaNeuron.salida);
          
                salidaNeuron.error = sigmoid.derivative(salidaNeuron.salida) * (resultados[i] - salidaNeuron.salida);
                salidaNeuron.ajustePeso();
            
                ocultarNeuron1.error = sigmoid.derivative(ocultarNeuron1.salida) * salidaNeuron.error * salidaNeuron.pesos[0];
                ocultarNeuron2.error = sigmoid.derivative(ocultarNeuron2.salida) * salidaNeuron.error * salidaNeuron.pesos[1];

                ocultarNeuron1.ajustePeso();
                ocultarNeuron2.ajustePeso();
            }

            if (cont < 2000)
                goto Retry;      
        
            Console.ReadLine();
        }
    }
}