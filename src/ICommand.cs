using System.Threading.Tasks;

namespace QLeatherMan
{
    internal interface ICommand
    {
        Task RunAsync();
    }
}