using System.Threading.Tasks;

namespace Nimator
{
    public interface ICheck
    {
        string ShortName { get; }

        Task<ICheckResult> RunAsync();
    }
}
