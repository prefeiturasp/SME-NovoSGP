﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase : AbstractUseCase, IConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase
    {
        public ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                if (!await ExecutarConsolidacao())
                    return false;

                await ConsolidarAcompanhamentoAprendizagemAluno();
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Consolidacao Acompanhamento Aprendizagem Alunos Sync UseCase", LogNivel.Critico, LogContexto.AcompanhamentoAprendizagem, ex.Message));
                throw;
            }
        }

        private async Task ConsolidarAcompanhamentoAprendizagemAluno()
        {
            var anoAtual = DateTime.Now.Year;
            await mediator.Send(new LimparConsolidacaoAcompanhamentoAprendizagemCommand(anoAtual));

            var ues = await mediator.Send(ObterCodigosUEsQuery.Instance);
            foreach (var ue in ues)
                await PublicarConsolidacaoPorUe(ue);

            await AtualizarDataExecucao(anoAtual);
        }

        private async Task PublicarConsolidacaoPorUe(string ue)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoPorUE, new FiltroUEDto(ue), Guid.NewGuid(), null));
        }

        private async Task<bool> ExecutarConsolidacao()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma, DateTime.Now.Year));
            if (parametroExecucao.NaoEhNulo())
                return parametroExecucao.Ativo;

            return false;
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma, ano));
            if (parametroSistema.NaoEhNulo())
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");
                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}
