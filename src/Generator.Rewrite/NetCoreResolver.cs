// See LICENSE file

using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Collections.Generic;
using System.Linq;
using System.IO;

namespace OpenTK.Rewrite
{
    internal class NetCoreResolver : IAssemblyResolver
    {
        readonly List<string> directories;

        public NetCoreResolver()
        {
            directories = new List<string>();
        }

        public void AddSearchDirectory(string dirPath)
        {
            directories.Add(dirPath);
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            return Resolve(name, new ReaderParameters());
        }
        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            foreach(var path in directories)
            {
                string filePath = Path.Combine(path, name.Name + ".dll");

                if (File.Exists(filePath))
                {
                    return ModuleDefinition.ReadModule(filePath, parameters).Assembly;
                }
            }

            throw new AssemblyResolutionException(name);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}