using UnityEngine;
[System.Serializable]
public class IA
{
    [UnityEngine.SerializeField]
    public static int id = 0;
    [UnityEngine.SerializeField]
    private Input[] inputs;
    [UnityEngine.SerializeField]
    private Layer[] layers;
    [UnityEngine.SerializeField]
    private Output[] outputs;
    [UnityEngine.SerializeField]
    public int myID;
    [UnityEngine.SerializeField]
    private float score;

    public IA() { }

    public IA(Input[] i, Layer[] l, Output[] o)
    {
        inputs = i;
        layers = l;
        outputs = o;
        score = 0;
        id++;
        myID = id;
    }

    public void SetScore(float newScore)
    {
        score = newScore;
    }

    public void AddToScore(float points)
    {
        score += points;
    }

    public void ResetScore()
    {
        SetScore(0);
    }

    public float GetScore()
    {
        return score;
    }

    public Input[] GetInputs()
    {
        return inputs;
    }

    public Layer[] GetLayers()
    {
        return layers;
    }

    public void SetInputValue(int index, float value)
    {
        inputs[index].SetValue(value);
    }

    public void ResetInputsValues()
    {
        foreach (Input input in inputs)
        {
            input.SetValue(0);
        }
    }

    public void Calcule()
    {
        CalculeFirstLayer();
        CalculeLayers();
        CalculeOutputs();
    }

    public void CalculeFirstLayer()
    {
        Layer firstLayer = layers[0];
        int neuronsPerLayer = firstLayer.GetLength();
        int inputsCount = inputs.Length;
        for (int i = 0; i < neuronsPerLayer; i++) // boucle pour les neurones
        {
            float somme = 0;
            for (int j = 0; j < inputsCount; j++) // boucle pour chaque poids
            {
                Input input = inputs[j];
                somme += input.GetValue() * input.GetWeightAt(i);
            }
            Neuron neuronToSetValue = firstLayer.GetNeuronAt(i);
            somme += neuronToSetValue.GetBias();
            neuronToSetValue.SetValue(Sigmoid(somme));
        }
    }

    public void CalculeLayers()
    {
        int layersCount = layers.Length;
        int neuronsPerLayer = layers[0].GetLength();
        for (int i = 1; i < layersCount; i++) // boucle pour les layers
        {
            for (int j = 0; j < neuronsPerLayer; j++) // boucle pour les neurones
            {
                float somme = 0;
                for (int k = 0; k < neuronsPerLayer; k++) // boucle pour chaque poids
                {
                    Neuron neuron = layers[i - 1].GetNeuronAt(k);
                    somme += neuron.GetValue() * neuron.GetWeightAt(j);
                }
                Neuron neuronToSetValue = layers[i].GetNeuronAt(j);
                somme += neuronToSetValue.GetBias();
                neuronToSetValue.SetValue(Sigmoid(somme));
            }
        }
    }

    public void CalculeOutputs()
    {
        int neuronsPerLayer = layers[0].GetLength();
        int layersCount = layers.Length;
        int outputsCount = outputs.Length;
        for (int i = 0; i < outputsCount; i++) // boucle pour les outputs
        {
            float somme = 0;
            for (int j = 0; j < neuronsPerLayer; j++) // boucle pour chaque poids
            {
                Neuron neuron = layers[layersCount - 1].GetNeuronAt(j);
                somme += neuron.GetValue() * neuron.GetWeightAt(i);
            }
            outputs[i].SetValue(somme);
        }
    }

    public Output[] GetOutputs()
    {
        return outputs;
    }

    public bool IsBetterThan(IA ia)
    {
        return score > ia.score;
    }

    public float Sigmoid(float value)
    {
        if (value <= 0)
        {
            return 0;
        }
        else
        {
            return 1 / (1 + Mathf.Exp(-value));
        }
    }

    public IA Copy()
    {
        int neuronsPerLayer = layers[0].GetLength();
        Input[] inputsCopy = new Input[inputs.Length];
        Layer[] layersCopy = new Layer[layers.Length];
        Output[] outputsCopy = GetNOutputs(outputs.Length);
        for (int i = 0; i < inputs.Length; i++)
        {
            float[] weights = inputs[i].GetWeights();
            inputsCopy[i] = new Input((float[])weights.Clone());
        }
        for (int i = 0; i < layers.Length; i++)
        {
            Neuron[] neuronsLayer = new Neuron[neuronsPerLayer];
            for (int j = 0; j < neuronsPerLayer; j++)
            {
                Neuron neuron = layers[i].GetNeuronAt(j);
                float[] weights = neuron.GetWeights();
                neuronsLayer[j] = new Neuron((float[])weights.Clone(), neuron.GetBias());
            }
            layersCopy[i] = new Layer(neuronsLayer);
        }
        IA res = new IA(inputsCopy, layersCopy, outputsCopy);
        res.SetScore(score);
        res.myID = myID;
        return res;
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
}
