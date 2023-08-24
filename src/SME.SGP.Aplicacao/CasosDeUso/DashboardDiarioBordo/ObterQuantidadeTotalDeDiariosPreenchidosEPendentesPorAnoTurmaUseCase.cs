using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase : IObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO>> Executar(FiltroDasboardDiarioBordoDto filtro)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            bool ehPerfilDRESME = usuario.PossuiPerfilSmeOuDre();

            return await mediator.Send(new ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery(filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.Modalidade, ehPerfilDRESME));
        }
    }
}
