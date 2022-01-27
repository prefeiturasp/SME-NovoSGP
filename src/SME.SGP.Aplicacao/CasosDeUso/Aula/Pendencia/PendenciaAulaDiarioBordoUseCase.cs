using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaDiarioBordoUseCase : AbstractUseCase, IPendenciaAulaDiarioBordoUseCase
    {
        public PendenciaAulaDiarioBordoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.DiarioBordo, "diario_bordo",
                new long[] { (int)Modalidade.EducacaoInfantil }));

            if (aulas != null && aulas.Any())
                await RegistraPendencia(aulas, TipoPendencia.DiarioBordo);

            return true;
        }

        private async Task RegistraPendencia(IEnumerable<Aula> aulas, TipoPendencia tipoPendenciaAula)
        {
            await mediator.Send(new SalvarPendenciaAulasPorTipoCommand(aulas, tipoPendenciaAula));
        }

    }
}
