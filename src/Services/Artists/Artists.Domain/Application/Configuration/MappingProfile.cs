#region references

using AutoMapper;

#endregion

namespace Artists.Domain.Application.Config
{
    public class MappingProfile : Profile
    {
        #region Public Constructors

        public MappingProfile() : this("DefaultProfile")
        {
        }

        #endregion

        #region Private Constructors

        protected MappingProfile(string profileName) : base(profileName)
        {
            //MapEfforts();
        }

        #endregion


    }
}