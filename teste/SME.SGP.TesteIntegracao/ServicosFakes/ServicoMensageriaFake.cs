using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoMensageriaFake : IServicoMensageria
    {
        public async Task<bool> Publicar(MensagemRabbit mensagem, string rota, string exchange, string nomeAcao)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> Publicar<T>(T mensagem, string rota, string exchange, string nomeAcao)
        {
            return await Task.FromResult(true);
        }
    }
}