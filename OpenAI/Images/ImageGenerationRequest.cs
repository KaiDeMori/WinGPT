namespace WinGPT.OpenAI.Images;

public class ImageGenerationRequest
{
    // The AI model to use for image generation, e.g., "dall-e-3".
    public string model { get; set; }

    // The text prompt to guide the image generation.
    public string prompt { get; set; }

    // The size of the generated image, represented by the ImageSize enum.
    public ImageSize size { get; set; }

    // The quality of the generated image, represented by the ImageQuality enum.
    public ImageQuality quality { get; set; }

    // The number of images to generate.
    public int n { get; set; }

    public ImageGenerationRequest(string model, string prompt, ImageSize size, ImageQuality quality, int n)
    {
        this.model = model;
        this.prompt = prompt;
        this.size = size;
        this.quality = quality;
        this.n = n;
    }
}