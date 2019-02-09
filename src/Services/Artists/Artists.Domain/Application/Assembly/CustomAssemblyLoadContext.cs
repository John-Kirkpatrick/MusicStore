#region references

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;

#endregion

namespace Artists.Domain.Application.Assembly
{
    [ExcludeFromCodeCoverage]
    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        #region Public Methods

        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        #endregion

        #region Private Methods

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override System.Reflection.Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}