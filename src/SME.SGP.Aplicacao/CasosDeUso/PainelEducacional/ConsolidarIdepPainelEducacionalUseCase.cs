using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdep;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarIdepPainelEducacionalUseCase : AbstractUseCase, IConsolidarIdepPainelEducacionalUseCase
    {

        public ConsolidarIdepPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param) 
        {
            var registrosIdep = await mediator.Send(new ObterIdepQuery());

            return await mediator.Send(new InserirConsolidacaoIdepCommand(registrosIdep));
        }
    }
}
