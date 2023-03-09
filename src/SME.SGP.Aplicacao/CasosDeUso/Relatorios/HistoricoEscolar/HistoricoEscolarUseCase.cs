﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class HistoricoEscolarUseCase : IHistoricoEscolarUseCase
    {
        private readonly IMediator mediator;

        public HistoricoEscolarUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroHistoricoEscolarDto filtroHistoricoEscolarDto)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtroHistoricoEscolarDto.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtroHistoricoEscolarDto.UeCodigo));
            await mediator.Send(new ValidaSeExisteTurmaPorCodigoQuery(filtroHistoricoEscolarDto.TurmaCodigo));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtroHistoricoEscolarDto.Usuario = usuarioLogado ?? throw new NegocioException("Não foi possível localizar o usuário.");

            var tipoRelatorio = filtroHistoricoEscolarDto.Modalidade == Modalidade.Fundamental ? TipoRelatorio.HistoricoEscolarFundamentalRazor : TipoRelatorio.HistoricoEscolarFundamental;

            return await mediator.Send(new GerarRelatorioCommand(tipoRelatorio, filtroHistoricoEscolarDto, usuarioLogado,rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosHistoricoEscolar));

        }
    }
}
