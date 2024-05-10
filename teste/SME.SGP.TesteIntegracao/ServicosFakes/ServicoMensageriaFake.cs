using System.Threading.Tasks;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoMensageriaFake : IServicoMensageriaSGP
    {
        public async Task<bool> Publicar(MensagemRabbit mensagem, string rota, string exchange, string nomeAcao, IModel canalRabbit = null)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> Publicar<T>(T mensagem, string rota, string exchange, string nomeAcao, IModel canalRabbit = null)
        {
            return await Task.FromResult(true);
        }
    }
}