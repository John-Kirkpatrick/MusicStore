#region references

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Artists.Domain.Application
{
    /// <summary>
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        #region Public Constructors

        /// <summary>
        /// </summary>
        /// <param name="error"></param>
        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        #endregion
    }
}