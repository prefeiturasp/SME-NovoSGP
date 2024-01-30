using System.Threading.Tasks;
using SME.SGP.Infra.Interface;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoMensageriaApiEOLFake  : IServicoMensageriaApiEOL
    {
        public async Task<bool> Publicar(object mensagem, string rota, string exchange, string nomeAcao, RabbitMQ.Client.IModel canalRabbit = null)
        {
            return await Task.FromResult(true);
        }
    }
}