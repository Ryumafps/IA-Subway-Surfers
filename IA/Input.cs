[System.Serializable]
public class Input
{
    [UnityEngine.SerializeField]
    private float value;
    [UnityEngine.SerializeField]
    private float[] weights;

    // Constructor
    public Input(float[] initialweights)
    {
        weights = initialweights;
    }
    public void SetValue(float newValue)
    {
        value = newValue;
    }

    public float GetValue()
    {
        return value;
    }

    public float[] GetWeights()
    {
        return weights;
    }

    public float GetWeightAt(int i)
    {
        return weights[i];
    }
}
