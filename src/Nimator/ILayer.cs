namespace Nimator
{
    public interface ILayer
    {
        string Name { get; set; }

        LayerResult Run();
    }
}
