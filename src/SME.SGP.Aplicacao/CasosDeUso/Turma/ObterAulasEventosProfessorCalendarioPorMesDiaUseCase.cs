﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorMesDiaUseCase
    {
        public static async Task<EventosAulasNoDiaCalendarioDto> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes, int dia, int anoLetivo,
            IServicoUsuario servicoUsuario)
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

            var retorno = new EventosAulasNoDiaCalendarioDto();

            var podeCadastrarAulaEMensagem = await mediator.Send(new ObterPodeCadastrarAulaPorDataQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                DataAula = dataConsulta,
                UeId = filtroAulasEventosCalendarioDto.UeId
            });

            retorno.PodeCadastrarAula = podeCadastrarAulaEMensagem.PodeCadastrar;
            retorno.MensagemPeriodoEncerrado = podeCadastrarAulaEMensagem.MensagemPeriodo;


            long[] componentesCurricularesDoProfessor = new long[0];
            if (usuarioLogado.EhProfessor())
            {
                //Transformar em query
                componentesCurricularesDoProfessor = await servicoUsuario.ObterComponentesCurricularesQuePodeVisualizarHoje(filtroAulasEventosCalendarioDto.TurmaCodigo, usuarioLogado);
            }
            
            IEnumerable<Aula> aulasParaVisualizar = usuarioLogado.ObterAulasQuePodeVisualizar(aulasDoDia, componentesCurricularesDoProfessor.Select(a => a.ToString()).ToArray());


            IEnumerable<AtividadeAvaliativa> atividadesAvaliativas = Enumerable.Empty<AtividadeAvaliativa>();
            IEnumerable<DisciplinaDto> componentesCurriculares = Enumerable.Empty<DisciplinaDto>();

            if (aulasParaVisualizar.Any())
            {

                componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(aulasParaVisualizar.Select(a => long.Parse(a.DisciplinaId)).ToArray()));

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
