namespace WinGPT.OpenAI.Images;

// Enum for the size of the generated image.
public enum ImageSize
{
    [System.Runtime.Serialization.EnumMember(Value = "1024x1024")]
    _1024x1024, // Represents a square image of 1024x1024 pixels.

    [System.Runtime.Serialization.EnumMember(Value = "1024x1792")]
    _1024x1792, // Represents a rectangular image of 1024x1792 pixels.

    [System.Runtime.Serialization.EnumMember(Value = "1792x1024")]
    _1792x1024 // Represents a rectangular image of 1792x1024 pixels.
}

// Enum for the quality of the generated image.
public enum ImageQuality
{
    standard, // Represents the standard quality of the generated image.
    hd        // Represents the high definition quality of the generated image.
}