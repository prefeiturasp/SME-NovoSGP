using MediatR;
using Npgsql.Replication;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuarioLogado.EhNulo())
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

            IEnumerable<Aula> aulasParaVisualizar = null;
            IEnumerable<AtividadeAvaliativa> atividadesAvaliativas = Enumerable.Empty<AtividadeAvaliativa>();
            var componentesCurricularesEolProfessor = new List<ComponenteCurricularEol>();

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
                                                                                 turma.EhTurmaInfantil))).ToList();
                var componentesCurricularesAgrupamentoTerritorioSaber = componentesCurricularesEolProfessor.Where(cc => cc.Codigo >= TerritorioSaberConstants.COMPONENTE_AGRUPAMENTO_TERRITORIO_SABER_ID_INICIAL);
                if (componentesCurricularesAgrupamentoTerritorioSaber.Any())
                    componentesCurricularesEolProfessor.AddRange(await mediator.Send(new ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery(componentesCurricularesAgrupamentoTerritorioSaber.Select(cc => cc.Codigo).ToArray(), dataConsulta)));

                if (usuarioLogado.EhSomenteProfessorCj())
                {
                    var componentesCurricularesDoProfessorCJ = await mediator
                        .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

                    if (componentesCurricularesDoProfessorCJ.Any())
                    {
                        componentesCurricularesDoProfessorCJ.ToList().ForEach(c =>
                        {
                            componentesCurricularesEolProfessor.Add(new ComponenteCurricularEol()
                            {
                                Codigo = c.DisciplinaId,
                                Professor = c.ProfessorRf
                            });
                        });
                    }
                }

                aulasParaVisualizar = usuarioLogado.ObterAulasQuePodeVisualizar(aulasDoDia, componentesCurricularesEolProfessor);
            }

            IEnumerable<DisciplinaDto> componentesCurriculares = Enumerable.Empty<DisciplinaDto>();

            if (aulasParaVisualizar.NaoEhNulo() && aulasParaVisualizar.Any())
            {
                var codigosComponentesConsiderados = new List<long>();
                codigosComponentesConsiderados.AddRange(aulasParaVisualizar.Select(a => long.Parse(a.DisciplinaId)));
                
                if (usuarioLogado.EhProfessorCjInfantil())
                {
                    var componentesCurricularesDoProfessorCJInfantil = await mediator
                        .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

                    codigosComponentesConsiderados
                        .AddRange(componentesCurricularesDoProfessorCJInfantil
                            .Where(c => c.TurmaId == turma.CodigoTurma)
                                .Select(c => c.DisciplinaId));
                }
                
                componentesCurriculares = await mediator
                    .Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(codigosComponentesConsiderados.ToArray(), filtroAulasEventosCalendarioDto.TurmaCodigo));

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