using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FICR.Cooperation.Humanism.Services.Contracts
{
   public interface  IMenssagemService
    {
        Task SendMessage(string recipientNumber, string messageBody, Dictionary<string, string> variables = null);
    }
}
