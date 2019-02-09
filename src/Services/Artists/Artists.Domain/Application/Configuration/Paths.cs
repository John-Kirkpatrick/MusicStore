namespace Artists.Domain.Application.Config {
    public static class Paths {
        #region Static Fields

        private const string VersionPath = "api/v1";

        public static readonly string ConstituentsApiPath = $"{VersionPath}/constituents";

        public static readonly string EventsApiPath = $"{VersionPath}/events";

        public static readonly string EffortsApiPath = $"{VersionPath}/efforts";

        public static readonly string EventConstituentsPath = $"{VersionPath}/event-constituent";

        #endregion
    }
}