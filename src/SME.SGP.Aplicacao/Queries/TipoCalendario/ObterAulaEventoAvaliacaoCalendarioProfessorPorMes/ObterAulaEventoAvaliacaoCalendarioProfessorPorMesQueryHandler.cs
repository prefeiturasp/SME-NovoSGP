using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQueryHandler : IRequestHandler<ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQuery, IEnumerable<EventoAulaDiaDto>>
    {
        private readonly IMediator mediator;
        public ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<EventoAulaDiaDto>> Handle(ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQuery request, CancellationToken cancellationToken)
        {
            var qntDiasMes = DateTime.DaysInMonth(request.AnoLetivo, request.Mes);

            var listaRetorno = new List<EventoAulaDiaDto>();

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));

            for (int i = 1; i < qntDiasMes + 1; i++)
            {
                var eventoAula = new EventoAulaDiaDto() { Dia = i };

                if (request.EventosDaUeSME.Any(a => i >= a.DataInicio.Day && i <= a.DataFim.Day))
                    eventoAula.TemEvento = true;

                var aulasDoDia = request.Aulas.Where(a => a.DataAula.Day == i);
                if (aulasDoDia.Any())
                {
                    if (aulasDoDia.Any(a => a.AulaCJ))
                        eventoAula.TemAulaCJ = true;

                    if (aulasDoDia.Any(a => a.AulaCJ == false))
                        eventoAula.TemAula = true;

                    if (eventoAula.TemAula || eventoAula.TemAulaCJ)
                    {

                        var avaliacoesDoDia = request.Avaliacoes.Where(a => a.DataAvaliacao.Day == i);
                        var componentesCurricularesDoDia = aulasDoDia.Select(a => a.DisciplinaId);
                        if (avaliacoesDoDia.Any())
                        {
                            var temAvaliacaoComComponente = (from avaliacao in avaliacoesDoDia
                                                             from disciplina in avaliacao.Disciplinas
                                                             where componentesCurricularesDoDia.Contains(disciplina.DisciplinaId.ToString()) || avaliacao.ProfessorRf == request.UsuarioCodigoRf
                                                             select true);

                            if (temAvaliacaoComComponente.Any())
                                eventoAula.TemAvaliacao = true;
                        }

                    }

                    eventoAula.PossuiPendencia = await mediator.Send(new ObterPendenciasAulaPorAulaIdsQuery(aulasDoDia.Select(a => a.Id).ToArray(), turma.ModalidadeCodigo));
                }
                listaRetorno.Add(eventoAula);
            }

            return listaRetorno.AsEnumerable();
        }
    }
}
