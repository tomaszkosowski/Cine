namespace Cine.ArchitectureTests
{
    public static class CommonExtensions
    {
        public static bool ContainsAny(this string pattern, string[] source)
        {
            if (source is null || string.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }

            return source.Any(item => item.Contains(pattern));
        }
    }
}
