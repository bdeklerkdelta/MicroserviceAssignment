using MediatR;
using MicroserviceB.Service.Commands;
using MicroserviceB.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceB.Service.Services
{
    public class DisplayNameService : IDisplayNameService
    {
        private readonly IMediator _mediator;

        public DisplayNameService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async void DisplayName(string name)
        {
            try
            {
                await _mediator.Send(new DisplayNameCommand
                {
                    Name = name
                });
            }
            catch (Exception ex)
            {
                //do some logging
            }
        }
    }
}
