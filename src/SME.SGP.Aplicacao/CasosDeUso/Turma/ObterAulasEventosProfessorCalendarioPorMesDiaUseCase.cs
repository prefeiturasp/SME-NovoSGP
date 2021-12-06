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

            string rf = usuarioLogado.CodigoRf;

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

            if (usuarioLogado.EhProfessor())
                componentesCurricularesDoProfessor = await mediator
                                                          .Send(new ObterComponentesCurricularesQuePodeVisualizarHojeQuery(usuarioLogado.CodigoRf, 
                                                                                                                           usuarioLogado.PerfilAtual, 
                                                                                                                           filtroAulasEventosCalendarioDto.TurmaCodigo));

            IEnumerable<Aula> aulasParaVisualizar = usuarioLogado.ObterAulasQuePodeVisualizar(aulasDoDia, componentesCurricularesDoProfessor);


            IEnumerable<AtividadeAvaliativa> atividadesAvaliativas = Enumerable.Empty<AtividadeAvaliativa>();
            
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

                atividadesAvaliativas = usuarioLogado.ObterAtividadesAvaliativasQuePodeVisualizar(atividadesAvaliativas, componentesCurricularesDoProfessor);
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

