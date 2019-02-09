namespace Infrastructure.Extensions
{
    public static class ByteExtensions
    {
        #region Public Methods
        
        public static bool HasValue(this byte[] value)
        {
            //jak: found out the cache manager does return null, at least when running functional tests
            //this can be confirmed by removing null check and running functional test
            //can_get_efforts_and_response_ok_status_code
            return value != null && value.Length > 0;
        }

        #endregion
    }
}