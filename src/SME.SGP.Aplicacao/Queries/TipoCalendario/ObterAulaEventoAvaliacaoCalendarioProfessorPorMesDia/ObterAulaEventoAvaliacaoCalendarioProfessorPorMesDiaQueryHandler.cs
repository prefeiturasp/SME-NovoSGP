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
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterAulaEventoAvaliacaoCalendarioProfessorPorMesDiaQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }
        public async Task<IEnumerable<EventoAulaDto>> Handle(ObterAulaEventoAvaliacaoCalendarioProfessorPorMesDiaQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<EventoAulaDto>();

            if (request.Aulas.Any())
            {

                foreach (var aulaParaVisualizar in request.Aulas)
                {
                    var componenteCurricular = request.ComponentesCurricularesParaVisualizacao.FirstOrDefault(a => a.CodigoComponenteCurricular == long.Parse(aulaParaVisualizar.DisciplinaId));

                    var eventoAulaDto = new EventoAulaDto()
                    {
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
                                                       where disciplina.DisciplinaId == aulaParaVisualizar.DisciplinaId || avaliacao.ProfessorRf == request.UsuarioCodigoRf
                                                       select avaliacao);

                    if (atividadesAvaliativasDaAula.Any())
                    {
                        foreach (var atividadeAvaliativa in atividadesAvaliativasDaAula)
                        {
                            eventoAulaDto.AtividadesAvaliativas.Add(new AtividadeAvaliativaParaEventoAulaDto() { Descricao = atividadeAvaliativa.NomeAvaliacao, Id = atividadeAvaliativa.Id });
                        }
                    }

                    eventoAulaDto.MostrarBotaoFrequencia = await repositorioFrequencia.FrequenciaAulaRegistrada(aulaParaVisualizar.Id);


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

                    if (evento.TipoEvento.TipoData == EventoTipoData.InicioFim)
                    {
                        eventoParaAdicionar.DataInicio = evento.DataInicio;
                        eventoParaAdicionar.DataFim = evento.DataFim;
                    }

                    retorno.Add(eventoParaAdicionar);
                }
            }

            return retorno.AsEnumerable();
        }
    }
}
