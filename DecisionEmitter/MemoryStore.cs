using System.Text.Json;

public class MemoryStore<T>
{
    private readonly string _path;

    public MemoryStore(string name)
    {
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

        var memoryDir = Path.Combine(root, ".memory");
        Directory.CreateDirectory(memoryDir);

        _path = Path.Combine(memoryDir, $"{name}.json");
    }

    public T Load(Func<T> fallback)
    {
        if (!File.Exists(_path))
            return fallback();

        try
        {
            return JsonSerializer.Deserialize<T>(
                File.ReadAllText(_path)
            ) ?? fallback();
        }
        catch
        {
            return fallback();
        }
    }

    public void Save(T data)
    {
        File.WriteAllText(
            _path,
            JsonSerializer.Serialize(data)
        );
    }
}
