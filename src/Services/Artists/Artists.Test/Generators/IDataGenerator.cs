using Artists.Domain.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artists.Test.Generators
{
    public interface IDataGenerator : IDisposable
    {
        ArtistContext GenerateMockContext(IGeneratorTemplate template);
    }
}
