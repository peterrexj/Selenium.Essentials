using Pj.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestAny.Essentials.Core.Dtos.Api
{
    public class ClientCertificatePathData
    {
        public string Path { get; set; }
        public string Password { get; set; }
        public bool IsRelativePath { get; set; }

        public bool IsPathValid => Path.HasValue() && File.Exists(Path);
    }
}
