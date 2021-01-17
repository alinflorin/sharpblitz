using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpBlitz.Runner.Abstractions
{
    public interface IRunner
    {
        Task LoadAndRun(RunnerInput input);
    }
}