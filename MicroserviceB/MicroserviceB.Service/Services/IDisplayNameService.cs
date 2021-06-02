using MicroserviceB.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceB.Service.Services
{
    public interface IDisplayNameService
    {
        void DisplayName(string name);
    }
}
