using SharpBlitz.Common.Assembly;
using SharpBlitz.Common.Http;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBlitz.Common
{
    public sealed class IdeStore
    {
        private readonly CachingHttpClient _http;
        private readonly Config.Config _config;
        private readonly ConcurrentDictionary<string, AssemblyDefinition> _gac = new ConcurrentDictionary<string, AssemblyDefinition>();
        private readonly ConcurrentDictionary<string, AssemblyDefinition> _uploaded = new ConcurrentDictionary<string, AssemblyDefinition>();


        private readonly IEnumerable<AssemblyDefinition> _requiredAsms = new HashSet<AssemblyDefinition>()
        {
            new AssemblyDefinition { Name = "mscorlib.dll", AssemblySource = AssemblySource.GAC },
            new AssemblyDefinition { Name = "netstandard.dll", AssemblySource = AssemblySource.GAC },
            new AssemblyDefinition { Name = "Microsoft.CodeAnalysis.CSharp.Features.dll", AssemblySource = AssemblySource.GAC }
        };

        public IdeStore(CachingHttpClient http, Common.Config.Config config)
        {
            _config = config;
            _http = http;
        }

        public async Task PreloadRequiredAsms()
        {
            foreach (var req in _requiredAsms)
            {
                if (!_gac.ContainsKey(req.Name) || _gac[req.Name].Source == null)
                {
                    await PreloadGacAssembly(req);
                }
            }
        }

        public IEnumerable<AssemblyDefinition> GetRequiredAsms()
        {
            return _gac.Values.Where(x => x.AssemblySource == AssemblySource.GAC && _requiredAsms.Any(z => z.Name == x.Name)).ToList();
        }

        public async Task<PreloadUploadedAssemblyResult> PreloadUploadedAssembly(AssemblyDefinition def)
        {
            var result = new PreloadUploadedAssemblyResult
            {
                Name = def.Name
            };
            if (!_uploaded.ContainsKey(def.Name))
            {
                _uploaded[def.Name] = def;
            }
            await Task.CompletedTask;
            return result;
        }

        public async Task<UnloadUploadedAssemblyResult> UnloadUploadedAssembly(AssemblyDefinition def)
        {
            var result = new UnloadUploadedAssemblyResult
            {
                Name = def.Name
            };
            if (_uploaded.ContainsKey(def.Name))
            {
                result.Success = _uploaded.TryRemove(def.Name, out _);
            }
            await Task.CompletedTask;
            return result;

        }

        public AssemblyDefinition GetFromUploaded(AssemblyDefinition def)
        {
            if (_uploaded.ContainsKey(def.Name))
            {
                return _uploaded[def.Name];
            }
            return null;
        }

        public IEnumerable<AssemblyDefinition> GetAllFromUploaded()
        {
            return _uploaded.Values.ToList();
        }

        public async Task<PreloadGacAssemblyResult> PreloadGacAssembly(AssemblyDefinition def)
        {
            var result = new PreloadGacAssemblyResult
            {
                Name = def.Name
            };
            if (!_gac.ContainsKey(def.Name))
            {
                try
                {
                    def.Source = await _http.GetStreamAsync($"{_config.BaseUrl}_framework/_bin/{def.Name}");
                    try
                    {
                        def.Debug = await _http.GetStreamAsync($"{_config.BaseUrl}_framework/_bin/{def.Name.Replace(".dll", ".pdb")}");
                    } catch
                    {

                    }
                    _gac[def.Name] = def;
                } catch
                {
                    result.Success = false;
                }
            }
            return result;
        }

        public async Task<UnloadGacAssemblyResult> UnloadGacAssembly(AssemblyDefinition def)
        {
            var result = new UnloadGacAssemblyResult
            {
                Name = def.Name
            };
            if (_gac.ContainsKey(def.Name))
            {
                result.Success = _gac.TryRemove(def.Name, out _);
            }
            await Task.CompletedTask;
            return result;
        }

        public AssemblyDefinition GetFromGac(AssemblyDefinition def)
        {
            if (_gac.ContainsKey(def.Name))
            {
                return _gac[def.Name];
            }
            return null;
        }

        public IEnumerable<AssemblyDefinition> GetAllFromGac()
        {
            return _gac.Values.ToList();
        }
    }
}
