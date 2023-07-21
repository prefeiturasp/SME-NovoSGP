using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaTurmaComponenteSemAulasPorUeUseCase : AbstractUseCase, IPendenciaTurmaComponenteSemAulasPorUeUseCase
    {
        public PendenciaTurmaComponenteSemAulasPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<DreUeDto>();

            var tiposTurma = new List<int>()
            {
                (int)TipoTurma.Regular,
                (int)TipoTurma.EdFisica
            };

            var tiposItinerarioEM = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());
            tiposTurma.AddRange(tiposItinerarioEM.Select(t => t.Id));

            var turmasUe = await mediator.Send(new ObterTurmasPorUeAnoTiposTurmaQuery(filtro.UeId, DateTimeExtension.HorarioBrasilia().Year, tiposTurma.ToArray()));

            if (!turmasUe.Any())
                return false;

            foreach (var turma in turmasUe)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAula, turma));

            return true;
        }
    }
}
