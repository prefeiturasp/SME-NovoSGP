using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutarSincronizacaoObjetivosComJuremaUseCase : IExecutarSincronizacaoObjetivosComJuremaUseCase
    {
        private readonly IServicoObjetivosAprendizagem servico;

        public ExecutarSincronizacaoObjetivosComJuremaUseCase(IServicoObjetivosAprendizagem servico)
        {
            this.servico = servico;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await servico.SincronizarObjetivosComJurema();
            return true;
        }
    }
}
