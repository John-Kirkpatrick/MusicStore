using System;
using System.Collections.Generic;
using System.Text;

namespace Artists.Domain.Application.Constants
{
    public static class Caching
    {
        #region Static Fields

        public const int ShortDuration = 10;
        public const int MediumDuration = 60;
        public const int LongDuration = 300;
        public const string CachingTable = "SqlCache";
        public const string CachingSchema = "dbo";
        public const string GlobalBatchCacheKey = "GlobalBatchCacheKey";
        public const string EffortCacheKey = "EffortCacheKey";

        #endregion
    }
}
