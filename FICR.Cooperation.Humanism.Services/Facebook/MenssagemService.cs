using FICR.Cooperation.Humanism.Data.Contracts.Base;
using FICR.Cooperation.Humanism.Services.Contracts;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FICR.Cooperation.Humanism.Services.Twilio
{
    public class MenssagemService : IMenssagemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioWhatsAppNumber;

        // Construtor que recebe as credenciais como parâmetros
        public MenssagemService(IUnitOfWork unitOfWork, string accountSid, string authToken, string twilioWhatsAppNumber)
        {
            _unitOfWork = unitOfWork;
            _accountSid = accountSid;
            _authToken = authToken;
            _twilioWhatsAppNumber = twilioWhatsAppNumber;

            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task SendMessage(string recipientNumber, string messageBody, Dictionary<string, string> variables = null)
        {
            try
            {
                // Verifica se o número do destinatário e o corpo da mensagem não são nulos ou vazios
                if (string.IsNullOrEmpty(recipientNumber))
                    throw new ArgumentException("O número do destinatário não pode ser nulo ou vazio.", nameof(recipientNumber));
                
                if (string.IsNullOrEmpty(messageBody))
                    throw new ArgumentException("O corpo da mensagem não pode ser nulo ou vazio.", nameof(messageBody));

                var messageOptions = new CreateMessageOptions(new PhoneNumber($"whatsapp:{recipientNumber}"))
                {
                    From = new PhoneNumber($"whatsapp:{_twilioWhatsAppNumber}"),
                    Body = messageBody
                };

                // Adiciona variáveis se fornecidas
                if (variables != null && variables.Count > 0)
                {
                    var formattedVariables = string.Join(Environment.NewLine, variables.Select(v => $"{v.Key}: {v.Value}"));
                    messageOptions.Body = $"{messageBody}{Environment.NewLine}{formattedVariables}";
                }

                var message = await MessageResource.CreateAsync(messageOptions);
                Console.WriteLine("Message sent: " + message.Body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                // Lidar com outras exceções conforme necessário
                throw; // Re-lançar a exceção se necessário
            }
        }
    }
}
