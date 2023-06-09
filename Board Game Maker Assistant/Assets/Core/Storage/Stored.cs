using System;

public interface Stored<T>
{
    T Get();
    void Write(Func<T, T> transform);
}

public static class StoredExtensions
{
    public static void Write<T>(this Stored<T> s, Action<T> update) => s.Write(item => { update(item); return item; });

    public static bool TryWrite<T>(this Stored<T> s, Func<T, T> transform)
    {
        try
        {
            s.Write(transform);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
