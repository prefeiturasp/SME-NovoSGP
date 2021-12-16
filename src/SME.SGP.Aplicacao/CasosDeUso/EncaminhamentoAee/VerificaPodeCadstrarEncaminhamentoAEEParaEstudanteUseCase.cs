using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class VerificaPodeCadstrarEncaminhamentoAEEParaEstudanteUseCase : AbstractUseCase, IVerificaPodeCadstrarEncaminhamentoAEEParaEstudanteUseCase
    {
        public VerificaPodeCadstrarEncaminhamentoAEEParaEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(string codigoEstudante)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorEstudanteQuery(codigoEstudante));

            if (encaminhamentoAEE != null && encaminhamentoAEE.SituacaoTipo != SituacaoAEE.Indeferido)
                throw new NegocioException("Estudante/Criança já possui encaminhametno AEE em aberto");

            return true;
        }
    }
}
