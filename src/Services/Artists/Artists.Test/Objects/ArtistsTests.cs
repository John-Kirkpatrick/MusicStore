using Artists.Domain.Application.Config;
using Artists.Domain.Objects;
using Artists.Domain.Objects.DTO;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artists.Test.Objects
{
    [TestClass]
    public class ArtistsTests
    {
        private IMapper _mapper;

        [TestInitialize]
        public void Init()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
        }

        [TestMethod]
        public void dto_maps_correct()
        {
            Artist artist = new Artist() { Id = Guid.NewGuid(), Name = "Some Artist" };

            ArtistDTO dto = _mapper.Map<Artist, ArtistDTO>(artist);

            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual("Some Artist", dto.Name);

        }
    }
}
