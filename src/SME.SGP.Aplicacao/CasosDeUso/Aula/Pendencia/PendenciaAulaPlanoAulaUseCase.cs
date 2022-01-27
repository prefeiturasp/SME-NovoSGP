using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaPlanoAulaUseCase : AbstractUseCase, IPendenciaAulaPlanoAulaUseCase
    {
        public PendenciaAulaPlanoAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.PlanoAula, "plano_aula",
                new long[] { (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio }));

            if (aulas != null && aulas.Any())
                await RegistraPendencia(aulas, TipoPendencia.PlanoAula);

            return true;
        }

        private async Task RegistraPendencia(IEnumerable<Aula> aulas, TipoPendencia tipoPendenciaAula)
        {
            await mediator.Send(new SalvarPendenciaAulasPorTipoCommand(aulas, tipoPendenciaAula));
        }
    }
}
