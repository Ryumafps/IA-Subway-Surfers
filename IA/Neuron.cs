// Classe Neuron
[System.Serializable]
public class Neuron
{
    [UnityEngine.SerializeField]
    private float value;
    [UnityEngine.SerializeField]
    private float[] weights;
    [UnityEngine.SerializeField]
    private float bias;
    public Neuron(float[] w, float b)
    {
        value = 0f;
        weights = w;
        bias = b;
    }

    public float GetValue()
    {
        return value;
    }

    public void SetValue(float val)
    {
        value = val;
    }

    public float[] GetWeights()
    {
        return weights;
    }

    public float GetWeightAt(int i)
    {
        return weights[i];
    }

    public float GetBias()
    {
        return bias;
    }

    public void SetBias(float b)
    {
        bias = b;
    }
}