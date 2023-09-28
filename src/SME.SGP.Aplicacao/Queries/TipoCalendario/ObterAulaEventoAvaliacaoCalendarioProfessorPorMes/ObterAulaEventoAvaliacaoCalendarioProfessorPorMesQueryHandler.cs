﻿using MediatR;
using SME.SGP.Dominio;
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
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<EventoAulaDiaDto>> Handle(ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQuery request, CancellationToken cancellationToken)
        {
            var qntDiasMes = DateTime.DaysInMonth(request.AnoLetivo, request.Mes);

            var listaRetorno = new List<EventoAulaDiaDto>();
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo), cancellationToken);
            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma), cancellationToken);
            var periodoFechamento = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdQuery(tipoCalendarioId), cancellationToken);
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance, cancellationToken);

            var componentesCurriculares = await mediator
                .Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turma.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual), cancellationToken);

            var componentesCurricularesId = componentesCurriculares?.Where(b => b.RegistraFrequencia == true)
                .Select(x => x.TerritorioSaber && x.CodigoComponenteTerritorioSaber > 0 ? x.CodigoComponenteTerritorioSaber : x.Codigo)
                .ToArray();

            for (int i = 1; i < qntDiasMes + 1; i++)
            {
                var eventoAula = new EventoAulaDiaDto() { Dia = i };

                foreach (var item in periodoFechamento.FechamentosBimestres)
                {
                    var dataMes = Convert.ToDateTime($"{i}/{request.Mes}/{request.AnoLetivo}");

                    if (periodoFechamento == null)
                    {
                        if (request.EventosDaUeSME.Any(a => i >= a.DataInicio.Day && i <= a.DataFim.Day))
                        {
                            eventoAula.TemEvento = true;
                            break;
                        }
                    }
                    else if (dataMes >= item.InicioDoFechamento && dataMes <= item.FinalDoFechamento)
                    {
                        eventoAula.TemEvento = true;
                        break;
                    }
                }

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
                                                             where avaliacao.TipoAvaliacaoId != 18
                                                             select true);

                            if (temAvaliacaoComComponente.Any())
                                eventoAula.TemAvaliacao = true;
                        }
                    }

                    var aulasId = aulasDoDia.Select(a => a.Id).ToArray();
                    if (turma.ModalidadeCodigo == Modalidade.EJA)
                    {
                        var aulas = aulasDoDia.Where(a => !a.EhTecnologiaAprendizagem);
                        aulasId = aulas != null && aulas.Any() ? aulas.Select(a => a.Id).ToArray() : null;
                    }

                    if (aulasId != null && aulasId.Any() && componentesCurricularesId != null)
                        eventoAula.PossuiPendencia = await mediator.Send(new ObterPendenciasAulaPorAulaIdsQuery(aulasId, turma.ModalidadeCodigo, componentesCurricularesId.Union(aulasDoDia.Select(a => long.Parse(a.DisciplinaId))).ToArray()), cancellationToken);
                    else
                        eventoAula.PossuiPendencia = false;
                }
                listaRetorno.Add(eventoAula);
            }

            return listaRetorno.AsEnumerable();
        }
    }
}
