﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarDresConsolidacaoMatriculaUseCase : ICarregarDresConsolidacaoMatriculaUseCase
    {
        private readonly IMediator mediator;

        public CarregarDresConsolidacaoMatriculaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            if (!await ExecutarConsolidacaoMatricula())
                return false;

            var anoAtual = DateTime.Now.Year;
            await AtualizarDataExecucao(anoAtual);
            await mediator.Send(new LimparConsolidacaoMatriculaTurmaPorAnoCommand(anoAtual));

            var anosLetivosParaConsolidar = new List<int>();
            for (var ano = 2014; ano < DateTime.Now.Year; ano++)
            {
                if (!await mediator.Send(new ExisteConsolidacaoMatriculaTurmaPorAnoQuery(ano))
                    && await mediator.Send(new ObterQuantidadeUesPorAnoLetivoQuery(ano)) > 0)
                {
                    await AtualizarDataExecucao(ano);
                    anosLetivosParaConsolidar.Add(ano);
                }
            }

            var dres = await mediator.Send(ObterIdsDresQuery.Instance);
            foreach (var dreId in dres)
            {
                var dre = new FiltroConsolidacaoMatriculaDreDto(dreId, anosLetivosParaConsolidar);
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpInstitucional.SincronizarDresMatriculasTurmas, dre, Guid.NewGuid(), null));
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Carregar Dres Consolidacao Matricula UseCase", LogNivel.Critico, LogContexto.ConsolidacaoMatricula, ex.Message));                    
                }
                

            }
            return true;
        }

        private async Task<bool> ExecutarConsolidacaoMatricula()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoInformacoesEscolares, DateTime.Now.Year));
            if (parametroExecucao.NaoEhNulo())
                return parametroExecucao.Ativo;

            return false;
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoInformacoesEscolares, ano));
            if (parametroSistema.NaoEhNulo())
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");

                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}
