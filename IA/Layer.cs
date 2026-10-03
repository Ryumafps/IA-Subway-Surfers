[System.Serializable]

public class Layer
{
    [UnityEngine.SerializeField]
    private Neuron[] neurons;
    private int length;
    public Layer(Neuron[] n)
    {
        neurons = n;
        length = neurons.Length;
    }

    public Neuron[] GetNeurons()
    {
        return neurons;
    }

    public Neuron GetNeuronAt(int i)
    {
        return neurons[i];
    }

    public int GetLength()
    {
        return length;
    }
}
