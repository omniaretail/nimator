using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    public interface ICheck
    {
        string ShortName { get; }

        Task<ICheckResult> RunAsync();
    }
}
