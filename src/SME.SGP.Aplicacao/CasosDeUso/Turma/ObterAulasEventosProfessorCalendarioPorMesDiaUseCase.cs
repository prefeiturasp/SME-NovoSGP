using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorMesDiaUseCase
    {
        public static async Task<EventosAulasNoDiaCalendarioDto> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes, int dia, int anoLetivo)
        {
            var dataConsulta = new DateTime(anoLetivo, mes, dia);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o Usuário logado.");

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

            retorno.MensagemPeriodoEncerrado = podeCadastrarAulaEMensagem.MensagemPeriodo;

            string[] componentesCurricularesDoProfessor = new string[0];

            IEnumerable<Aula> aulasParaVisualizar = null;
            IEnumerable<AtividadeAvaliativa> atividadesAvaliativas = Enumerable.Empty<AtividadeAvaliativa>();

            atividadesAvaliativas = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                DataReferencia = dataConsulta
            });

            if (usuarioLogado.EhProfessorCjInfantil() && DateTimeExtension.EhAnoAtual(filtroAulasEventosCalendarioDto.AnoLetivo))
            {
                var professoresTitularesComponentesCJ = new List<ProfessorTitularDisciplinaEol>();

                var componentesAtribuidosCJ = await mediator.Send(new ObterAtribuicaoCJPorDreUeTurmaRFQuery(filtroAulasEventosCalendarioDto.TurmaCodigo,
                    filtroAulasEventosCalendarioDto.DreCodigo, filtroAulasEventosCalendarioDto.UeCodigo, usuarioLogado.CodigoRf));

                foreach (var dados in componentesAtribuidosCJ)
                    professoresTitularesComponentesCJ.Add(await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(filtroAulasEventosCalendarioDto.TurmaCodigo,
                        dados.DisciplinaId.ToString())));


                aulasParaVisualizar = from aula in aulasDoDia
                                      join profTitular in professoresTitularesComponentesCJ
                                      on new { DisciplinaId = long.Parse(aula.DisciplinaId), aula.ProfessorRf } equals new { profTitular.DisciplinaId, profTitular.ProfessorRf }
                                      select aula;

                atividadesAvaliativas = from avaliacao in atividadesAvaliativas
                                        join profTitular in professoresTitularesComponentesCJ
                                        on avaliacao.ProfessorRf equals profTitular.ProfessorRf
                                        where avaliacao.Disciplinas.Select(s => long.Parse(s.DisciplinaId)).Contains(profTitular.DisciplinaId)
                                        select avaliacao;

                retorno.PodeCadastrarAula = false;
            }
            else
            {
                if (usuarioLogado.EhProfessor())
                    componentesCurricularesDoProfessor = await mediator
                                                              .Send(new ObterComponentesCurricularesQuePodeVisualizarHojeQuery(usuarioLogado.CodigoRf,
                                                                                                                               usuarioLogado.PerfilAtual,
                                                                                                                               filtroAulasEventosCalendarioDto.TurmaCodigo));
                aulasParaVisualizar = usuarioLogado.ObterAulasQuePodeVisualizar(aulasDoDia, componentesCurricularesDoProfessor);
                atividadesAvaliativas = usuarioLogado.ObterAtividadesAvaliativasQuePodeVisualizar(atividadesAvaliativas, componentesCurricularesDoProfessor);
            }
            
            IEnumerable<DisciplinaDto> componentesCurriculares = Enumerable.Empty<DisciplinaDto>();

            if (aulasParaVisualizar.Any())
            {
                componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(aulasParaVisualizar.Select(a => long.Parse(a.DisciplinaId)).ToArray(), aulasParaVisualizar.Any(a => a.DisciplinaId.Length > 5)));

                atividadesAvaliativas = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQuery()
                {
                    UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                    DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                    TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                    DataReferencia = dataConsulta
                });
            }

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
    }
}