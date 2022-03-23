using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarPendenciasFechamentoTurmaDisciplinaUseCase : AbstractUseCase, IVerificarPendenciasFechamentoTurmaDisciplina
    {
        private readonly IServicoPendenciaFechamento servicoFechamentoTurmaDisciplina;

        public VerificarPendenciasFechamentoTurmaDisciplinaUseCase(IMediator mediator, IServicoPendenciaFechamento servicoFechamentoTurmaDisciplina) : base(mediator)
        {
            this.servicoFechamentoTurmaDisciplina = servicoFechamentoTurmaDisciplina;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var data = param.ObterObjetoMensagem<PendenciaFechamentoCompletoDto>();
            servicoFechamentoTurmaDisciplina.VerificaPendenciasFechamento(data.FechamentoId);
            return true;
        }
    }
}
