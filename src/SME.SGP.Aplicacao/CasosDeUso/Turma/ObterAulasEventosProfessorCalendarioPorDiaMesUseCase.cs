using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorDiaMesUseCase
    {
        public static async Task<EventosAulasNoDiaCalendarioDto> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes, int dia, int anoLetivo,
            IServicoUsuario servicoUsuario, IServicoEOL servicoEOL)
        {

            var dataConsulta = new DateTime(anoLetivo, mes, dia);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var eventosDaUeSME = await mediator.Send(new ObterEventosCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                DataConsulta = dataConsulta
            });

            var aulasDoDia = await mediator.Send(new ObterAulasCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                TipoCalendarioId = tipoCalendarioId,
                DiaConsulta = dataConsulta
            });

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery()
            {
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo
            });


            var retorno = new EventosAulasNoDiaCalendarioDto();

            var podeCadastrarAulaEMensagem = await PodeCadastrarAulaHoje(dataConsulta, tipoCalendarioId, turma, mediator, filtroAulasEventosCalendarioDto.UeCodigo, filtroAulasEventosCalendarioDto.DreCodigo);

            retorno.PodeCadastrarAula = podeCadastrarAulaEMensagem.Item1;
            retorno.MensagemPeriodoEncerrado = podeCadastrarAulaEMensagem.Item2;

            var aulasParaVisualizar = await ObterAulasParaVisualizacao(usuarioLogado, aulasDoDia, servicoEOL, servicoUsuario, filtroAulasEventosCalendarioDto);

            if (aulasParaVisualizar.Any())
            {
                var atividadesAvaliativas = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQuery()
                {
                    UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                    DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                    TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,                    
                    DataReferencia = dataConsulta
                });


                var idsComponentesCurricularesParaVisualizar = aulasParaVisualizar.Select(a => long.Parse(a.DisciplinaId))
                              .Distinct()
                              .ToArray();

                var componentesCurricularesParaVisualizacao = await servicoEOL.ObterDisciplinasPorIdsAsync(idsComponentesCurricularesParaVisualizar);

                foreach (var aulaParaVisualizar in aulasParaVisualizar)
                {
                    var componenteCurricular = componentesCurricularesParaVisualizacao.FirstOrDefault(a => a.CodigoComponenteCurricular == long.Parse(aulaParaVisualizar.DisciplinaId));

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

                    var atividadesAvaliativasDaAula = (from avaliacao in atividadesAvaliativas
                                                       from disciplina in avaliacao.Disciplinas
                                                       where disciplina.DisciplinaId == aulaParaVisualizar.DisciplinaId || avaliacao.ProfessorRf == usuarioLogado.CodigoRf
                                                       select avaliacao);

                    if (atividadesAvaliativasDaAula.Any())
                    {
                        foreach (var atividadeAvaliativa in atividadesAvaliativasDaAula)
                        {
                            eventoAulaDto.AtividadesAvaliativas.Add(new AtividadeAvaliativaParaEventoAulaDto() { Descricao = atividadeAvaliativa.NomeAvaliacao, Id = atividadeAvaliativa.Id });
                        }
                    }

                    eventoAulaDto.MostrarBotaoFrequencia = await mediator.Send(new ObterAulaPossuiFrequenciaQuery()
                    {
                        AulaId = aulaParaVisualizar.Id
                    });

                    retorno.EventosAulas.Add(eventoAulaDto);

                }
            }


            if (eventosDaUeSME.Any())
            {
                foreach (var evento in eventosDaUeSME)
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

                    retorno.EventosAulas.Add(eventoParaAdicionar);
                }
            }

            return retorno;
        }

        private static async Task<(bool, string)> PodeCadastrarAulaHoje(DateTime dataAula, long tipoCalendarioId, Turma turma, IMediator mediator, string ueCodigo, string dreCodigo)
        {
            var hoje = DateTime.Today;

            var temEventoLetivoNoDia = await mediator.Send(new ObterTemEventoLetivoPorCalendarioEDiaQuery()
            {
                DataParaVerificar = hoje,
                TipoCalendarioId = tipoCalendarioId,
                DreCodigo = dreCodigo,
                UeCodigo = ueCodigo
            });

            if (temEventoLetivoNoDia)
                return (true, string.Empty);

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery()
            {
                DataParaVerificar = hoje,
                TipoCalendarioId = tipoCalendarioId
            });



            if (hoje.DayOfWeek == DayOfWeek.Sunday)
                return (false, string.Empty);

            else
            {
                var temEventoNaoLetivoNoDia = await mediator.Send(new ObterTemEventoNaoLetivoPorCalendarioEDiaQuery()
                {
                    DataParaVerificar = hoje,
                    TipoCalendarioId = tipoCalendarioId
                });

                if (temEventoNaoLetivoNoDia)
                {
                    return (false, string.Empty);
                }

                if (dataAula.Year == hoje.Year)
                {
                    if (dataAula <= hoje)
                    {
                        if (periodoEscolar != null)
                            return (true, string.Empty);
                        else
                        {

                            var periodoFechamento = await mediator.Send(new ObterExistePeriodoPorUeDataBimestreQuery()
                            {
                                Bimestre = periodoEscolar.Bimestre,
                                DataParaVerificar = hoje,
                                UeId = turma.UeId
                            });

                            if (periodoFechamento != null)
                            {
                                if (periodoFechamento.ExisteFechamentoEmAberto(hoje))
                                    return (true, string.Empty);
                            }
                            else
                            {
                                FechamentoReabertura periodoFechamentoReabertura = await ObterPeriodoFechamentoReabertura(tipoCalendarioId, turma, mediator, hoje);
                                if (periodoFechamentoReabertura != null)
                                    return (true, string.Empty);
                                else return (false, "Apenas é possível consultar este registro pois o período deste bimestre não está aberto.");
                            }

                        }
                    }
                    else
                        return (true, string.Empty);

                }
                else
                {
                    FechamentoReabertura periodoFechamentoReabertura = await ObterPeriodoFechamentoReabertura(tipoCalendarioId, turma, mediator, hoje);
                    if (periodoFechamentoReabertura != null)
                        return (true, string.Empty);
                    else return (false, "Apenas é possível consultar este registro pois o período deste bimestre não está aberto.");
                }
            }
            return (true, string.Empty);
        }

        private static async Task<FechamentoReabertura> ObterPeriodoFechamentoReabertura(long tipoCalendarioId, Turma turma, IMediator mediator, DateTime hoje)
        {
            return await mediator.Send(new ObterFechamentoReaberturaPorDataTurmaQuery()
            {
                TipoCalendarioId = tipoCalendarioId,
                DataParaVerificar = hoje,
                UeId = turma.UeId
            });
        }

        private static async Task<IEnumerable<Aula>> ObterAulasParaVisualizacao(Usuario usuarioLogado, IEnumerable<Aula> aulas, IServicoEOL servicoEOL,
    IServicoUsuario servicoUsuario, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto)
        {
            IEnumerable<Aula> aulasParaVisualizar;


            if (usuarioLogado.TemPerfilGestaoUes())
            {
                aulasParaVisualizar = aulas.ToList();
            }
            else
            {
                if (usuarioLogado.EhProfessorCj())
                {
                    aulasParaVisualizar = aulas.Where(a => a.ProfessorRf == usuarioLogado.CodigoRf).ToList();
                }
                else
                {
                    aulasParaVisualizar = await ObterAulasParaVisualizarDoProfessor(aulas, usuarioLogado, servicoUsuario, servicoEOL, filtroAulasEventosCalendarioDto);
                }
            }

            return aulasParaVisualizar;
        }
        private static async Task<IEnumerable<Aula>> ObterAulasParaVisualizarDoProfessor(IEnumerable<Aula> aulas, Usuario usuarioLogado, IServicoUsuario servicoUsuario, IServicoEOL servicoEOL, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto)
        {

            string[] componentesCurricularesParaVisualizar = new string[0];

            if (usuarioLogado.EhProfessor())
                componentesCurricularesParaVisualizar = await servicoUsuario.ObterComponentesCurricularesQuePodeVisualizarHoje(filtroAulasEventosCalendarioDto.TurmaCodigo, usuarioLogado);

            var componentesCurricularesDasAulas = aulas.Select(a => a.DisciplinaId);

            return aulas.Where(a => componentesCurricularesParaVisualizar.Contains(a.DisciplinaId) || a.ProfessorRf == usuarioLogado.CodigoRf);
        }
    }
}
