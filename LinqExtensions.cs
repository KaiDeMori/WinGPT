namespace WinGPT;

public static class LinqExtensions {
   public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source) where T : class {
      return source.Where(x => x != null)!;
   }

   //another variant
   public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items) {
      return items.OfType<object>().Cast<T>();
   }
}