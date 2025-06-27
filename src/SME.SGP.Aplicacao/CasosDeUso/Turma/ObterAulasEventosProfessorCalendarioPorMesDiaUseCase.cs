using MediatR;
using MongoDB.Bson.Serialization.IdGenerators;
using Npgsql.Replication;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json;
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
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance) ?? throw new NegocioException("Não foi possível localizar o Usuário logado.");

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

            
            if (await UsuarioCJPodeEditarRegistroTitular(usuarioLogado, anoLetivo))
            {
                aulasParaVisualizar = aulasDoDia;
                retorno.PodeCadastrarAula = false;
            }
            else
            {
                componentesCurricularesEolProfessor = await ObterComponentesCurricularesProfessor(filtroAulasEventosCalendarioDto.TurmaCodigo,
                                                                                 usuarioLogado.Login,
                                                                                 usuarioLogado.PerfilAtual,
                                                                                 turma.EhTurmaInfantil,
                                                                                 dataConsulta);

                if (usuarioLogado.EhSomenteProfessorCj())
                    componentesCurricularesEolProfessor.AddRange(await ObterComponentesCurricularesProfessorCJ(usuarioLogado.Login));          
                
                var jsonAulasTest = JsonSerializer.Serialize(aulasDoDia, new JsonSerializerOptions { WriteIndented = true });
                var jsonComponentesCurricularesTest = JsonSerializer.Serialize(componentesCurricularesEolProfessor, new JsonSerializerOptions { WriteIndented = true });

                aulasParaVisualizar = usuarioLogado.ObterAulasQuePodeVisualizar(aulasDoDia, componentesCurricularesEolProfessor);
            }

            IEnumerable<DisciplinaDto> componentesCurriculares = Enumerable.Empty<DisciplinaDto>();

            if (aulasParaVisualizar.PossuiRegistros())
            {
                var codigosComponentesConsiderados = aulasParaVisualizar.Select(a => long.Parse(a.DisciplinaId)).ToList();      
                if (usuarioLogado.EhProfessorCjInfantil())
                {
                    var componentesCurricularesDoProfessorCJInfantil = await ObterComponentesCurricularesProfessorCJ(usuarioLogado.Login, turma.CodigoTurma);
                    codigosComponentesConsiderados.AddRange(componentesCurricularesDoProfessorCJInfantil.Select(c => c.Codigo));
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
            };

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

        private async Task<List<ComponenteCurricularEol>> ObterComponentesCurricularesProfessorCJ(string codigoRf, string codigoTurma = null)
        {
            var componentesCurricularesDoProfessorCJ = await mediator
                        .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(codigoRf));

            if (!string.IsNullOrEmpty(codigoTurma))
                componentesCurricularesDoProfessorCJ = componentesCurricularesDoProfessorCJ.Where(cc => cc.TurmaId == codigoTurma);

            return componentesCurricularesDoProfessorCJ.Select(cc => new ComponenteCurricularEol()
                                                                    {
                                                                        Codigo = cc.DisciplinaId,
                                                                        Professor = cc.ProfessorRf
                                                                    }).ToList();
        }
        private async Task<List<ComponenteCurricularEol>> ObterComponentesCurricularesProfessor(string codigoTurma, string login, Guid perfilUsuario, bool ehTurmaInfantil, DateTime dataConsulta)
        {
           var componentesCurricularesEolProfessor = (await mediator
                                                        .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(codigoTurma,
                                                                                 login,
                                                                                 perfilUsuario,
                                                                                 ehTurmaInfantil))).ToList();
            var componentesCurricularesAgrupamentoTerritorioSaber = componentesCurricularesEolProfessor.Where(cc => cc.Codigo.EhIdComponenteCurricularTerritorioSaberAgrupado());
            if (componentesCurricularesAgrupamentoTerritorioSaber.Any())
                componentesCurricularesEolProfessor.AddRange(await mediator.Send(new ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery(componentesCurricularesAgrupamentoTerritorioSaber.Select(cc => cc.Codigo).ToArray(), dataConsulta)));

            return componentesCurricularesEolProfessor;
        }

        private async Task<bool> UsuarioCJPodeEditarRegistroTitular(Usuario usuario, int anoLetivo)
        {
            bool podeEditarRegistroTitular = await VerificaCJPodeEditarRegistroTitular(anoLetivo);
            return (usuario.EhProfessorCjInfantil() && podeEditarRegistroTitular);
        }

        public async Task<bool> VerificaCJPodeEditarRegistroTitular(int anoLetivo)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.CJInfantilPodeEditarAulaTitular, anoLetivo));

            return dadosParametro?.Ativo ?? false;
        }
    }
}