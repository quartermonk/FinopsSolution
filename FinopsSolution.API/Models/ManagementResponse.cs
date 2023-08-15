namespace FinopsSolution.API.Models;

public class ManagementResponse<T>
{
    public Uri? NextLink { get; set; }
    public T[] Value { get; set; } = Array.Empty<T>();
}
