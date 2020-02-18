namespace QLeatherMan
{
    public interface ICommandFactoryOptions
    {
        public ICommandFactoryOptions Add<T>(string? name = null) where T : class, ICommand;
    }
}

