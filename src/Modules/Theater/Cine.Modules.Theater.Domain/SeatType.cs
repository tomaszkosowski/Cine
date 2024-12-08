using Cine.Shared.Domain;

namespace Cine.Modules.Theater.Domain
{
    public record SeatType : ValueObject
    {
        #region Properties

        public string Value { get; }

        #endregion

        #region Constructor

        private SeatType(string type)
            => Value = type.ToLower();

        #endregion

        #region Public methods

        public static SeatType Of(string type) => new(type);

        public static SeatType Regular => new(nameof(Regular));

        public static SeatType Premium => new(nameof(Premium));

        public override string ToString() => Value;

        #endregion
    }
}