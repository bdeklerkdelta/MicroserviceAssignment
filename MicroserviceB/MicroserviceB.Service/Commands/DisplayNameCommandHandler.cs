using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceB.Service.Commands
{
    public class DisplayNameCommandHandler : IRequestHandler<DisplayNameCommand>
    {
        public async Task<Unit> Handle(DisplayNameCommand request, CancellationToken cancellationToken)
        {
            var validator = new DisplayNameCommandValidator();
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                Console.WriteLine(string.Format("Hello {0}, I am your father!", request.Name));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return Unit.Value;
        }
    }
}
