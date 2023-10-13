using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCase : IIdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCase
    {
        private readonly IMediator mediator;

        public IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroIdentificarFrequenciaAlunoPresencasMaiorQuantidadeAulasDto>();

            var registrosIrregularesUeAtual = await mediator
                    .Send(new ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery(filtro.UeId, filtro.AnoLetivo));

            if (registrosIrregularesUeAtual.Any())
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RegularizarFrequenciaAlunoPresencasMaiorTotalAulas, registrosIrregularesUeAtual, param.CodigoCorrelacao));

            return true;
        }
    }
}
