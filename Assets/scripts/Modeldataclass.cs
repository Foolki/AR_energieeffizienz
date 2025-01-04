[System.Serializable]
public class HeatingPumpModel
{
    public string modelName; // "1_model name"
    public string description; // "2_description"
    public string priceRange; // "3_price"
    public string threeDModelUrl; // "4_3D_model_url"
    public Size size; // "5_size"
    public string capacities; // "6_capacities"
}

[System.Serializable]
public class Size
{
    public string depth;
    public string height;
    public string width;
}
