using System.Collections;

public class BiDictionary<T1, T2> : IEnumerable<KeyValuePair<T1, T2>> {
    private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
    private readonly Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

    public void Add(T1 first, T2 second) {
        _forward.Add(first, second);
        _reverse.Add(second, first);
    }

    public bool TryGetByFirst(T1 first, out T2 second) {
        return _forward.TryGetValue(first, out second);
    }

    public bool TryGetBySecond(T2 second, out T1 first) {
        return _reverse.TryGetValue(second, out first);
    }

    public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() {
        return _forward.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

}