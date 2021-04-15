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

            if (cicloEOL == null)
                throw new NegocioException($"Não foi possível inserir o ciclo. A mensagem enviada é inválida.");

            var auditoria =  await mediator.Send(new SalvarCicloEnsinoCommand(cicloEOL));          

            return auditoria.Id != 0;
        }
    }
}
