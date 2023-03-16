﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorMesUseCase : AbstractUseCase, IObterAulasEventosProfessorCalendarioPorMesUseCase
    {
        public ObterAulasEventosProfessorCalendarioPorMesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<EventoAulaDiaDto>> Executar(FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes)
        {
            var eventosDaUeSME = await mediator.Send(new ObterEventosDaUeSMEPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                Mes = mes
            });

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o Usuário logado.");

            var aulas = await mediator.Send(new ObterAulasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes
            });

            aulas = aulas.Where(a => a.TipoCalendarioId == tipoCalendarioId).ToList();
            var avaliacoes = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes,
                AnoLetivo = filtroAulasEventosCalendarioDto.AnoLetivo
            });

            IList<(string codigo, string codigoTerritorioSaber)> componentesCurricularesDoProfessor = new List<(string, string)>();

            bool verificaCJPodeEditar = await VerificaCJPodeEditarRegistroTitular(filtroAulasEventosCalendarioDto.AnoLetivo);

            IEnumerable<Aula> aulasParaVisualizar;

            if (usuarioLogado.EhProfessorCjInfantil() && verificaCJPodeEditar)
                aulasParaVisualizar = aulas;
            else
            {
                if (usuarioLogado.EhProfessor())
                {
                    componentesCurricularesDoProfessor = await mediator
                        .Send(new ObterComponentesCurricularesQuePodeVisualizarHojeQuery(usuarioLogado.CodigoRf,
                                                                                         usuarioLogado.PerfilAtual,
                                                                                         filtroAulasEventosCalendarioDto.TurmaCodigo,
                                                                                         usuarioLogado.EhProfessorInfantilOuCjInfantil()));
                }

                aulasParaVisualizar = usuarioLogado
                    .ObterAulasQuePodeVisualizar(aulas, componentesCurricularesDoProfessor);

                avaliacoes = usuarioLogado
                    .ObterAtividadesAvaliativasQuePodeVisualizar(avaliacoes, componentesCurricularesDoProfessor
                        .Select(c => c.codigo)
                            .ToArray());
            }

            return await mediator.Send(new ObterAulaEventoAvaliacaoCalendarioProfessorPorMesQuery()
            {
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Aulas = aulasParaVisualizar,
                EventosDaUeSME = eventosDaUeSME,
                Avaliacoes = avaliacoes,
                UsuarioCodigoRf = usuarioLogado.CodigoRf,
                Mes = mes,
                AnoLetivo = filtroAulasEventosCalendarioDto.AnoLetivo
            });
        }

        public async Task<bool> VerificaCJPodeEditarRegistroTitular(int anoLetivo)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.CJInfantilPodeEditarAulaTitular, anoLetivo));

            return dadosParametro?.Ativo ?? false;
        }
    }
}