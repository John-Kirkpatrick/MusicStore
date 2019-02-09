#region references

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Artists.Domain.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ArtistDomainException : Exception
    {
        #region Public Constructors

        public ArtistDomainException()
        {
        }

        public ArtistDomainException(string message) : base(message)
        {
        }

        public ArtistDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}