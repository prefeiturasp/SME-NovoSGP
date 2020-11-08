using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaDiasNaoLetivosCommandHandler : IRequestHandler<PendenciaDiasNaoLetivosCommand, bool>
    {
        private readonly IMediator mediator;
        public PendenciaDiasNaoLetivosCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PendenciaDiasNaoLetivosCommand request, CancellationToken cancellationToken)
        {
            var data = DateTime.Today;

            long componenteCurricularId = 0;  // temporario

            bool professorCj = false;  // temporario

            var Eventos = await mediator.Send(new ObterEventosPorTipoCalendarioIdTurmaIdEDataQuery(request.TipoCalendarioId, data, data, request.TurmaId));

            var aulas = await mediator.Send(new ObterAulaReduzidaPorTurmaComponenteEBimestreQuery(request.TurmaId, professorCj, request.TipoCalendarioId, componenteCurricularId, request.Bimestre));

            var diasComEventosNaoLetivos = MapearParaDiaLetivo(Eventos).Where(e => e.EhNaoLetivo);

            //var aulasDiasNaoLetivos = new List<AulaDiasNaoLetivosControleGradeDto>();
            //if (aulas != null)
            //{
            //    aulas.Where(a => diasComEventosNaoLetivos.Any(d => d.Data == a.Data)).ToList()
            //        .ForEach(aula =>
            //        {
            //            foreach (var eventoNaoLetivo in diasComEventosNaoLetivos.Where(d => d.Data == aula.Data))
            //            {
            //                aulasDiasNaoLetivos.Add(new AulaDiasNaoLetivosControleGradeDto()
            //                {
            //                    Data = $"{aula.Data:dd/MM/yyyy}",
            //                    Professor = $"{aula.Professor} ({aula.ProfessorRf})",
            //                    QuantidadeAulas = aula.Quantidade,
            //                    Motivo = eventoNaoLetivo.Motivo
            //                });
            //            }
            //        });
            //}

            return true;
        }


        private List<DiaLetivoDto> MapearParaDiaLetivo(IEnumerable<Evento> eventos)
        {
            var DiasLetivos = new List<DiaLetivoDto>();

            if (eventos != null)
            {
                foreach (var evento in eventos)
                {
                    foreach (var data in evento.ObterIntervaloDatas())
                    {
                        DiasLetivos.Add(new DiaLetivoDto
                        {
                            Data = data,
                            Motivo = evento.TipoEvento.Descricao,
                            EhLetivo = evento.EhEventoLetivo(),
                            EhNaoLetivo = evento.NaoEhEventoLetivo(),
                            UesIds = string.IsNullOrWhiteSpace(evento.UeId) ? new List<string>() : new List<string> { evento.UeId },
                            PossuiEvento = true
                        });
                    }
                }
            }

            return DiasLetivos;
        }
    }
}
