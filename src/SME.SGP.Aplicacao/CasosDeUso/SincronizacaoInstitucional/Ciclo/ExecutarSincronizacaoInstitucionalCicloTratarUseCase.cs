using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalCicloTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalCicloUseCase
    {
        public ExecutarSincronizacaoInstitucionalCicloTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {

            var cicloEOL = param.ObterObjetoMensagem<CicloRetornoDto>();

            //TODO: Continuar a task 37437

            //if (tipoEscolaEOL == null)
            //    throw new NegocioException($"Não foi possível fazer o tratamento do tipo de escola da mensagem {param.CodigoCorrelacao}");

            //var tipoEscolaSGP = await mediator.Send(new ObterTipoEscolaPorCodigoQuery(tipoEscolaEOL.Codigo));

            //await mediator.Send(new TrataSincronizacaoInstitucionalTipoEscolaCommand(tipoEscolaSGP, tipoEscolaEOL));


            return true;

        }
    }
}
