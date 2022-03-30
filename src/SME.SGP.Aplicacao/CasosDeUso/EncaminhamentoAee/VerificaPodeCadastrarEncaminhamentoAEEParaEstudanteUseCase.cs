using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class VerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase : AbstractUseCase, IVerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase
    {
        public VerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroEncaminhamentoAeeDto filtroEncaminhamentoAee)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorEstudanteQuery(filtroEncaminhamentoAee.EstudanteCodigo,
                filtroEncaminhamentoAee.UeCodigo));

            if (encaminhamentoAEE != null && encaminhamentoAEE.SituacaoTipo != SituacaoAEE.Indeferido)
                throw new NegocioException("Estudante/Criança já possui encaminhamento AEE em aberto");

            return true;
        }
    }
}
