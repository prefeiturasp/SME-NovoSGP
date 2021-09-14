using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Notificacao.NotificarAlunosFaltososBimestre;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Notificacao.NotificarAlunosFaltososBimestre
{
    public class ExecutarNotificacaoAlunosFaltososBimestreUseCase : IExecutarNotificacaoAlunosFaltososBimestreUseCase
    {
        private readonly IServicoNotificacaoFrequencia servico;

        public ExecutarNotificacaoAlunosFaltososBimestreUseCase(IServicoNotificacaoFrequencia servico)
        {
            this.servico = servico;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await servico.NotificarAlunosFaltososBimestre();
            return true;
        }
    }
}
