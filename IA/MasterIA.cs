//using System;
using UnityEngine;

// Classe MasterIA
public class MasterIA : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    /*[SerializeField]
    public IA bestIA;
    public IA bestIA2;
    public IA bestIA3;*

    void Start()
    {
        bestIA = GetRandomIA();
        bestIA2 = GetMutationIA(bestIA, 20);
        bestIA3 = bestIA.Copy();
    }

    void Update()
    {
        bestIA.Calcule();
        bestIA2.Calcule();
        bestIA3.Calcule();
    }
*/

    public static IA GetRandomIA()
    {
        Input[] randomInputs = GetRandomInputs(Constantes.NB_INPUTS);
        Layer[] randomLayers = new Layer[Constantes.NB_LAYERS];
        for (int i = 0; i < Constantes.NB_LAYERS - 1; i++)
        {
            randomLayers[i] = new Layer(GetRandomNeurons(Constantes.NEURON_PER_LAYER, Constantes.NEURON_PER_LAYER));
        }
        randomLayers[Constantes.NB_LAYERS - 1] = new Layer(GetRandomNeurons(Constantes.NEURON_PER_LAYER, Constantes.NB_OUTPUTS));
        Output[] outputs = GetNOutputs(Constantes.NB_OUTPUTS);
        return new IA(randomInputs, randomLayers, outputs);
    }


    public static float[] GetRandomWeights(int n)
    {
        float[] weights = new float[n];
        for (int i = 0; i < n; i++)
        {
            weights[i] = GetRandomWeight();
        }
        return weights;
    }

    public static float GetRandomWeight()
    {
        return rand.Next(-100, 101) * 0.01f;
    }

    public static Neuron[] GetRandomNeurons(int n, int nbWeights)
    {
        Neuron[] neurons = new Neuron[n];
        for (int i = 0; i < n; i++)
        {
            neurons[i] = GetRandomNeuron(nbWeights);
        }
        return neurons;
    }

    public static Neuron GetRandomNeuron(int n)
    {
        float[] weights = GetRandomWeights(n);
        float bias = GetRandomWeight();
        return new Neuron(weights, bias);
    }

    public static Input[] GetRandomInputs(int n)
    {
        Input[] inputs = new Input[n];
        for (int i = 0; i < n; i++)
        {
            inputs[i] = GetRandomInput(Constantes.NEURON_PER_LAYER);
        }
        return inputs;
    }

    public static Input GetRandomInput(int n)
    {
        float[] weights = GetRandomWeights(n);
        return new Input(weights);
    }

    public static Output[] GetNOutputs(int n)
    {
        Output[] outputs = new Output[n];
        for (int i = 0; i < n; i++)
        {
            outputs[i] = new Output();
        }
        return outputs;
    }
    public static IA GetMutationIA(IA ia, int t)
    {
        IA copiedIa = ia.Copy();
        Input[] mutatedInputs = GetMutationInputs(copiedIa.GetInputs(), t);
        Layer[] layersIA = copiedIa.GetLayers();
        Layer[] mutationLayers = new Layer[layersIA.Length];
        for (int i = 0; i < layersIA.Length; i++)
        {
            Neuron[] mutatedNeurons = GetMutationNeurons(layersIA[i].GetNeurons(), t);
            mutationLayers[i] = new Layer(mutatedNeurons);
        }
        Output[] outputs = GetNOutputs(Constantes.NB_OUTPUTS);
        return new IA(mutatedInputs, mutationLayers, outputs);
    }

    public static Neuron[] GetMutationNeurons(Neuron[] tab, int t)
    {
        Neuron[] mutatedNeurons = new Neuron[tab.Length];
        for (int i = 0; i < tab.Length; i++)
        {
            mutatedNeurons[i] = GetMutationNeuron(tab[i], t);
        }
        return mutatedNeurons;
    }

    public static Neuron GetMutationNeuron(Neuron neuron, int t)
    {
        float[] originalWeights = neuron.GetWeights();
        float[] mutatedWeights = GetMutationWeights(originalWeights, t);

        float originalBias = neuron.GetBias();
        if (Mutation(t))
        {
            originalBias = GetRandomWeight();
        }

        return new Neuron(mutatedWeights, originalBias);
    }

    public static Input[] GetMutationInputs(Input[] tab, int t)
    {
        Input[] mutatedInputs = new Input[tab.Length];
        for (int i = 0; i < tab.Length; i++)
        {
            mutatedInputs[i] = GetMutationInput(tab[i], t);
        }
        return mutatedInputs;
    }

    public static Input GetMutationInput(Input input, int t)
    {
        float[] originalWeights = input.GetWeights();
        float[] mutatedWeights = GetMutationWeights(originalWeights, t);
        return new Input(mutatedWeights);
    }

    public static float[] GetMutationWeights(float[] tab, int t)
    {
        float[] mutatedWeights = new float[tab.Length];
        for (int i = 0; i < tab.Length; i++)
        {
            if (Mutation(t))
            {
                mutatedWeights[i] = GetRandomWeight();
            }
            else
            {
                mutatedWeights[i] = tab[i];
            }
        }
        return mutatedWeights;
    }

    public static bool Mutation(int t)
    {
        return rand.Next(100) < t;
    }
}