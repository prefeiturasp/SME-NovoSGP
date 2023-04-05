using MediatR;
using Npgsql.Replication;
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

            var componentesCurricularesDoProfessor = new List<(string codigo, string codigoTerritorioSaber)>();

            IEnumerable<Aula> aulasParaVisualizar = null;
            IEnumerable<AtividadeAvaliativa> atividadesAvaliativas = Enumerable.Empty<AtividadeAvaliativa>();
            IEnumerable<ComponenteCurricularEol> componentesCurricularesEolProfessor = new List<ComponenteCurricularEol>();

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
                componentesCurricularesEolProfessor = (await mediator
                   .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(filtroAulasEventosCalendarioDto.TurmaCodigo,
                                                                                 usuarioLogado.Login,
                                                                                 usuarioLogado.PerfilAtual,
                                                                                 usuarioLogado.EhProfessorInfantilOuCjInfantil()))).ToList();

                if (usuarioLogado.EhSomenteProfessorCj())
                {
                    var componentesCurricularesDoProfessorCJ = await mediator
                        .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

                    if (componentesCurricularesDoProfessorCJ.Any())
                    {
                        var dadosComponentes = await mediator
                            .Send(new ObterDisciplinasPorIdsQuery(componentesCurricularesDoProfessorCJ.Select(c => c.DisciplinaId).ToArray()));

                        if (dadosComponentes.Any())
                        {
                            componentesCurricularesDoProfessor.AddRange(dadosComponentes
                                .Select(d => (d.CodigoComponenteCurricular.ToString(), d.TerritorioSaber ? d.CodigoComponenteCurricular.ToString() : "0")));
                        }
                    }
                }

                if (componentesCurricularesEolProfessor != null && componentesCurricularesEolProfessor.Any())
                {
                    componentesCurricularesEolProfessor.ToList()
                        .ForEach(cc => componentesCurricularesDoProfessor
                            .Add((cc.Codigo.ToString(), cc.CodigoComponenteTerritorioSaber.ToString())));
                }

                aulasParaVisualizar = usuarioLogado.ObterAulasQuePodeVisualizar(aulasDoDia, componentesCurricularesDoProfessor);
                atividadesAvaliativas = usuarioLogado.ObterAtividadesAvaliativasQuePodeVisualizar(atividadesAvaliativas, componentesCurricularesDoProfessor.Select(c => c.codigo).ToArray());
            }

            IEnumerable<DisciplinaDto> componentesCurriculares = Enumerable.Empty<DisciplinaDto>();

            if (aulasParaVisualizar != null && aulasParaVisualizar.Any())
            {
                if (componentesCurricularesDoProfessor != null && componentesCurricularesDoProfessor.Any(c => !string.IsNullOrWhiteSpace(c.codigoTerritorioSaber) && c.codigoTerritorioSaber != "0") && !usuarioLogado.EhProfessorCj())
                {
                    var componentesCorrelatos = !(usuarioLogado.TemPerfilGestaoUes() || usuarioLogado.TemPerfilAdmUE()) ?
                                                (from c in componentesCurricularesDoProfessor
                                                 from a in aulasParaVisualizar
                                                 where (c.codigo == a.DisciplinaId || c.codigoTerritorioSaber == a.DisciplinaId) && !a.AulaCJ
                                                 select (long.Parse(c.codigo), (long?)long.Parse(c.codigoTerritorioSaber))).Distinct().ToList() :
                                                 aulasParaVisualizar
                                                    .Select(a => (long.Parse(a.DisciplinaId), componentesCurricularesEolProfessor
                                                        .FirstOrDefault(cp => cp.Codigo.ToString() == a.DisciplinaId)?.CodigoComponenteTerritorioSaber ?? (long?)0))
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
                UsuarioCodigoRf = usuarioLogado.CodigoRf ?? usuarioLogado.Login,
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