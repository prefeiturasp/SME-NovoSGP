using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaEventoAvaliacaoCalendarioProfessorPorMesDiaQueryHandler : IRequestHandler<ObterAulaEventoAvaliacaoCalendarioProfessorPorMesDiaQuery, IEnumerable<EventoAulaDto>>
    {
        private readonly IMediator mediator;
        public ObterAulaEventoAvaliacaoCalendarioProfessorPorMesDiaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<EventoAulaDto>> Handle(ObterAulaEventoAvaliacaoCalendarioProfessorPorMesDiaQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<EventoAulaDto>();
            var professoresTitulares = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(request.TurmaCodigo));
            if (request.Aulas.Any())
            {
                foreach (var aulaParaVisualizar in request.Aulas)
                {
                    var componenteCurricular = request.ComponentesCurricularesParaVisualizacao.FirstOrDefault(a => a.CodigoComponenteCurricular == long.Parse(aulaParaVisualizar.DisciplinaId));
                    var professorTitular = professoresTitulares != null ? professoresTitulares.FirstOrDefault(p => p.DisciplinaId == long.Parse(aulaParaVisualizar.DisciplinaId)) : null;

                    var eventoAulaDto = new EventoAulaDto()
                    {
                        AulaId = aulaParaVisualizar.Id,
                        Titulo = componenteCurricular?.Nome,
                        EhAula = true,
                        EhReposicao = aulaParaVisualizar.TipoAula == TipoAula.Reposicao,
                        EstaAguardandoAprovacao = aulaParaVisualizar.Status == EntidadeStatus.AguardandoAprovacao,
                        EhAulaCJ = aulaParaVisualizar.AulaCJ,
                        Quantidade = aulaParaVisualizar.Quantidade,
                        ComponenteCurricularId = long.Parse(aulaParaVisualizar.DisciplinaId)
                    };

                    var atividadesAvaliativasDaAula = (from avaliacao in request.Avaliacoes
                                                       from disciplina in avaliacao.Disciplinas
                                                       where avaliacao.EhCj == aulaParaVisualizar.AulaCJ &&
                                                             disciplina.DisciplinaId == aulaParaVisualizar.DisciplinaId &&
                                                             (avaliacao.ProfessorRf == aulaParaVisualizar.ProfessorRf ||
                                                             (professorTitular != null && professorTitular.ProfessorRf == avaliacao.ProfessorRf && !avaliacao.EhCj))
                                                       select avaliacao);

                    if (atividadesAvaliativasDaAula.Any())
                    {
                        foreach (var atividadeAvaliativa in atividadesAvaliativasDaAula)
                        {
                            eventoAulaDto.AtividadesAvaliativas.Add(new AtividadeAvaliativaParaEventoAulaDto() { Descricao = atividadeAvaliativa.NomeAvaliacao, Id = atividadeAvaliativa.Id });
                        }
                    }

                    if (componenteCurricular != null)
                    {
                        eventoAulaDto.MostrarBotaoFrequencia = componenteCurricular.RegistraFrequencia;
                        eventoAulaDto.PodeCadastrarAvaliacao = ObterPodeCadastrarAvaliacao(atividadesAvaliativasDaAula, componenteCurricular);
                    }

                    eventoAulaDto.Pendencias = await mediator.Send(new ObterPendenciasAulaPorAulaIdQuery(aulaParaVisualizar.Id, atividadesAvaliativasDaAula.Any()));

                    retorno.Add(eventoAulaDto);
                }
            }

            if (request.EventosDaUeSME.Any())
            {
                foreach (var evento in request.EventosDaUeSME)
                {
                    var eventoParaAdicionar = new EventoAulaDto()
                    {
                        TipoEvento = evento.TipoEvento.Descricao,
                        Titulo = evento.Nome,
                        Descricao = evento.Descricao
                    };

                    if (evento.DataInicio.Date != evento.DataFim.Date)
                    {
                        eventoParaAdicionar.DataInicio = evento.DataInicio;
                        eventoParaAdicionar.DataFim = evento.DataFim;
                    }

                    eventoParaAdicionar.Dre = evento.EhEventoSME() ? "Todas" : evento.Dre.Abreviacao;
                    eventoParaAdicionar.Ue = (evento.EhEventoSME() || evento.EhEventoDRE()) ? "Todas" : evento.Ue.Nome;

                    retorno.Add(eventoParaAdicionar);
                }
            }

            return await Task.FromResult(retorno.AsEnumerable());
        }

        private bool ObterPodeCadastrarAvaliacao(IEnumerable<AtividadeAvaliativa> atividadesAvaliativasDaAula, DisciplinaDto componenteCurricular)
        {
            if (!componenteCurricular.LancaNota)
                return false;

            if (componenteCurricular.Regencia)
                return true;

            if (atividadesAvaliativasDaAula.Any())
                return false;

            return true;
        }
    }
}
