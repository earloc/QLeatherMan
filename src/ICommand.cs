using System.Threading.Tasks;

namespace QLeatherMan
{
    public interface ICommand
    {
        Task RunAsync();
    }
}