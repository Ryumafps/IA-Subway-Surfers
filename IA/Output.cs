[System.Serializable]
public class Output
{
    [UnityEngine.SerializeField]
    private float value;

    // Constructor
    public Output()
    {
        value = 0;
    }
    public void SetValue(float newValue)
    {
        value = newValue;
    }

    public float GetValue()
    {
        return value;
    }
}
