using System.Collections.Generic;

namespace SharpBlitz.Common.Assembly
{
    public class AssemblyDefinition
    {
        public byte[] Source { get; set; }
        public byte[] Debug { get; set; }
        public string Name { get; set; }
        public AssemblySource AssemblySource { get; set; }
        public string Version { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AssemblyDefinition definition &&
                   Name == definition.Name &&
                   AssemblySource == definition.AssemblySource &&
                   Version == definition.Version;
        }

        public override int GetHashCode()
        {
            var hashCode = 688405525;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + AssemblySource.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Version);
            return hashCode;
        }
    }

    public enum AssemblySource
    {
        InPlace = 0,
        GAC = 1,
        NuGet = 2
    }
}
