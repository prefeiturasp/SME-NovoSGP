using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaAusenciasPorMotivoUseCase : AbstractUseCase, IObterDashboardFrequenciaAusenciasPorMotivoUseCase
    {
        public ObterDashboardFrequenciaAusenciasPorMotivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(int anoLetivo, long dreId, long ueId, Modalidade? modalidade = null, string ano = "", long turmaId = 0, int semestre = 0)
            => await mediator.Send(new ObterDashboardFrequenciaAusenciasPorMotivoQuery(anoLetivo, dreId, ueId, modalidade, ano, turmaId, semestre));
    }
}
