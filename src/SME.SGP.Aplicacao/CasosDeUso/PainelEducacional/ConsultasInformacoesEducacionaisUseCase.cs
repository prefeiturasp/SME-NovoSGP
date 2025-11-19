using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterInformacoesEducacionais;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasInformacoesEducacionaisUseCase : IConsultasInformacoesEducacionaisUseCase
    {
        private readonly IMediator mediator;

        public ConsultasInformacoesEducacionaisUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public Task<InformacoesEducacionaisRetornoDto> ObterInformacoesEducacionais(FiltroInformacoesEducacionais filtro)
        {
            return mediator.Send(new PainelEducacionalRegistroInformacoesEducacionaisQuery(filtro));
        }
    }
}
