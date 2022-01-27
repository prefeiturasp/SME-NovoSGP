using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaAvaliacaoUseCase : AbstractUseCase, IPendenciaAulaAvaliacaoUseCase
    {
        public PendenciaAulaAvaliacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var aulas = await mediator.Send(new ObterPendenciasAtividadeAvaliativaQuery());
            if (aulas != null && aulas.Any())
                await RegistraPendencia(aulas, TipoPendencia.Avaliacao);

            return true;
        }

        private async Task RegistraPendencia(IEnumerable<Aula> aulas, TipoPendencia tipoPendenciaAula)
        {
            await mediator.Send(new SalvarPendenciaAulasPorTipoCommand(aulas, tipoPendenciaAula));
        }

    }
}
