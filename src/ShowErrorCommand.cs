using CommandLine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QLeatherMan
{
    internal class ShowErrorCommand : ICommand
    {
        private readonly IEnumerable<Error> errors;

        public ShowErrorCommand(IEnumerable<Error> errors)
        {
            this.errors = errors;
        }

        public Task RunAsync()
        {
            foreach (var error in errors)
                Console.WriteLine(error);

            return Task.CompletedTask;
        }
    }
}