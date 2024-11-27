namespace Cine.Shared.Domain
{
    public static class Utc
    {
        private static DateTime? _utcNowOverride;

        public static DateTime Now
        {
            get
            {
                if (_utcNowOverride.HasValue)
                {
                    return _utcNowOverride.Value;
                }

                return DateTime.UtcNow;
            }
        }

        public static void Override(DateTime value) => _utcNowOverride = value;

        public static void Rollback() => _utcNowOverride = null;
    }
}
