using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSemestresComReaberturaAtivaPAPQueryHandler : IRequestHandler<ObterSemestresComReaberturaAtivaPAPQuery, IEnumerable<SemestreAcompanhamentoDto>>
    {
        private readonly IMediator mediator;

        public ObterSemestresComReaberturaAtivaPAPQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<SemestreAcompanhamentoDto>> Handle(ObterSemestresComReaberturaAtivaPAPQuery request, CancellationToken cancellationToken)
        {
            var periodoFechamentoReaberturaVigente = await mediator.Send(new ObterFechamentoReaberturaPorDataTurmaQuery() { DataParaVerificar = request.DataReferencia, TipoCalendarioId = request.TipoCalendarioId, UeId = request.UeId });

            if (periodoFechamentoReaberturaVigente.NaoEhNulo())
            {
                var bimestresComReabertura = await mediator.Send(new ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQuery(periodoFechamentoReaberturaVigente.Id));

                if (bimestresComReabertura.Any())
                {
                    var semestresAjustadosComReabertura = request.Semestres.Select(s => new SemestreAcompanhamentoDto()
                    {
                        Descricao = s.Descricao,
                        Semestre = s.Semestre,
                        PodeEditar = s.Semestre == 1 && bimestresComReabertura.Any(b => b.Bimestre == 2) ? true
                   : s.Semestre == 2 && bimestresComReabertura.Any(b => b.Bimestre == 4) ? true : false
                    });

                    return semestresAjustadosComReabertura.ToList();
                }
            }

            return request.Semestres;
        }
    }
}
