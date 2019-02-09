#region references

using Microsoft.AspNetCore.Http;
using System;

#endregion

namespace Artists.Domain.Services
{
    public interface IIdentityService
    {
        #region Public Methods

        //string GetUserIdentity();

        string GetUserName();

        #endregion
    }

    public class IdentityService : IIdentityService
    {
        #region Private Fields

        private readonly IHttpContextAccessor _context;

        #endregion

        #region Public Constructors

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Public Methods

        //public string GetUserIdentity()
        //{
        //    return _context.HttpContext.User.FindFirst("sub").Value;
        //}

        public string GetUserName()
        {
            return _context.HttpContext.User.Identity.Name;
        }

        #endregion
    }
}