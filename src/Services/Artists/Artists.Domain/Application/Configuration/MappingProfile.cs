﻿#region references

using Artists.Domain.Objects;
using Artists.Domain.Objects.DTO;
using AutoMapper;

#endregion

namespace Artists.Domain.Application.Configuration
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
            MapArtists();
        }

        private void MapArtists()
        {
            CreateMap<Artist, ArtistDTO>()
              .ForMember(x => x.Name, conf => conf.MapFrom(d => $"{d.FirstName} {d.LastName}"));
        }

        #endregion


    }
}