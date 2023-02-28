using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorMesDiaUseCase : AbstractUseCase, IObterAulasEventosProfessorCalendarioPorMesDiaUseCase
    {
        public ObterAulasEventosProfessorCalendarioPorMesDiaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<EventosAulasNoDiaCalendarioDto> Executar(FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes, int dia, int anoLetivo)
        {
            var dataConsulta = new DateTime(anoLetivo, mes, dia);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o Usuário logado.");

            var eventosDaUeSME = await mediator.Send(new ObterEventosCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                DataConsulta = dataConsulta,
                TipoCalendarioId = tipoCalendarioId
            });

            var aulasDoDia = await mediator.Send(new ObterAulasCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                DiaConsulta = dataConsulta
            });

            aulasDoDia = aulasDoDia.Where(a => a.TipoCalendarioId == tipoCalendarioId).ToList();

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery()
            {
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo
            });

            string dadosTurma = turma.CodigoTurma + "-" + turma.Nome;

            var retorno = new EventosAulasNoDiaCalendarioDto();

            var podeCadastrarAulaEMensagem = await mediator.Send(new ObterPodeCadastrarAulaPorDataQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                DataAula = dataConsulta,
                Turma = turma
            });

            retorno.PodeCadastrarAula = podeCadastrarAulaEMensagem.PodeCadastrar;
            retorno.SomenteAulaReposicao = podeCadastrarAulaEMensagem.SomenteReposicao;
            retorno.MensagemPeriodoEncerrado = podeCadastrarAulaEMensagem.MensagemPeriodo;

            IList<(string codigo, string codigoTerritorioSaber)> componentesCurricularesDoProfessor = new List<(string, string)>();

            IEnumerable<Aula> aulasParaVisualizar = null;
            IEnumerable<AtividadeAvaliativa> atividadesAvaliativas = Enumerable.Empty<AtividadeAvaliativa>();
            IEnumerable<ComponenteCurricularEol> componentesCurricularesEolProfessor;
            bool ehCJEPossuiComponentesVinculados = false;

            atividadesAvaliativas = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                DataReferencia = dataConsulta
            });

            bool podeEditarRegistroTitular = await VerificaCJPodeEditarRegistroTitular(anoLetivo);

            if (usuarioLogado.EhProfessorCjInfantil() && podeEditarRegistroTitular)
            {
                aulasParaVisualizar = aulasDoDia;
                retorno.PodeCadastrarAula = false;
            }
            else
            {
                componentesCurricularesEolProfessor = await mediator
                   .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(filtroAulasEventosCalendarioDto.TurmaCodigo,
                                                                                 usuarioLogado.CodigoRf,
                                                                                 usuarioLogado.PerfilAtual,
                                                                                 usuarioLogado.EhProfessorInfantilOuCjInfantil()));

                if (usuarioLogado.EhSomenteProfessorCj())
                {
                    var componentesCurricularesDoProfessorCJ = await mediator
                   .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

                    if (componentesCurricularesDoProfessorCJ.Any())
                    {
                        var dadosComponentes = await mediator.Send(new ObterDisciplinasPorIdsQuery(componentesCurricularesDoProfessorCJ.Select(c => c.DisciplinaId).ToArray()));
                        if (dadosComponentes.Any())
                        {
                            componentesCurricularesDoProfessor = dadosComponentes.Select(d => (d.CodigoComponenteCurricular.ToString(), d.TerritorioSaber ? d.CodigoComponenteCurricular.ToString() : "0")).ToArray();
                            ehCJEPossuiComponentesVinculados = true;
                        }
                    }
                }

                if (componentesCurricularesEolProfessor != null || ehCJEPossuiComponentesVinculados)
                {
                    if (!ehCJEPossuiComponentesVinculados)
                        componentesCurricularesDoProfessor = componentesCurricularesEolProfessor
                        .Select(c => (c.Codigo.ToString(), c.CodigoComponenteTerritorioSaber.ToString())).ToArray();

                    aulasParaVisualizar = usuarioLogado.ObterAulasQuePodeVisualizar(aulasDoDia, componentesCurricularesDoProfessor);
                    atividadesAvaliativas = usuarioLogado.ObterAtividadesAvaliativasQuePodeVisualizar(atividadesAvaliativas, componentesCurricularesDoProfessor.Select(c => c.codigo).ToArray());
                }
            }

            IEnumerable<DisciplinaDto> componentesCurriculares = Enumerable.Empty<DisciplinaDto>();

            if (aulasParaVisualizar != null)
            {
                if (componentesCurricularesDoProfessor != null && componentesCurricularesDoProfessor.Any())
                {
                    var componentesCorrelatos = (from c in componentesCurricularesDoProfessor
                                                 from a in aulasParaVisualizar
                                                 where c.codigo == a.DisciplinaId || c.codigoTerritorioSaber == a.DisciplinaId
                                                 select (long.Parse(c.codigo), (long?)long.Parse(c.codigoTerritorioSaber)))
                                                 .ToList();

                    componentesCurriculares = await mediator
                        .Send(new ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQuery(componentesCorrelatos, componentesCurricularesDoProfessor.Any(c => !string.IsNullOrWhiteSpace(c.codigoTerritorioSaber) && c.codigoTerritorioSaber != "0"), filtroAulasEventosCalendarioDto.TurmaCodigo));
                }
                else
                {
                    componentesCurriculares = await mediator
                        .Send(new ObterComponentesCurricularesPorIdsQuery(aulasParaVisualizar.Select(a => long.Parse(a.DisciplinaId)).ToArray(), componentesCurricularesDoProfessor.Any(c => !string.IsNullOrWhiteSpace(c.codigoTerritorioSaber) && c.codigoTerritorioSaber != "0"), filtroAulasEventosCalendarioDto.TurmaCodigo));
                }

                atividadesAvaliativas = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQuery()
                {
                    UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                    DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                    TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                    DataReferencia = dataConsulta
                });
            }
            else
                aulasParaVisualizar = Enumerable.Empty<Aula>();

            retorno.EventosAulas = await mediator.Send(new ObterAulaEventoAvaliacaoCalendarioProfessorPorMesDiaQuery()
            {
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                UsuarioCodigoRf = usuarioLogado.CodigoRf,
                Aulas = aulasParaVisualizar,
                Avaliacoes = atividadesAvaliativas,
                ComponentesCurricularesParaVisualizacao = componentesCurriculares,
                EventosDaUeSME = eventosDaUeSME
            });

            return retorno;
        }

        public async Task<bool> VerificaCJPodeEditarRegistroTitular(int anoLetivo)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.CJInfantilPodeEditarAulaTitular, anoLetivo));

            return dadosParametro?.Ativo ?? false;
        }
    }
}