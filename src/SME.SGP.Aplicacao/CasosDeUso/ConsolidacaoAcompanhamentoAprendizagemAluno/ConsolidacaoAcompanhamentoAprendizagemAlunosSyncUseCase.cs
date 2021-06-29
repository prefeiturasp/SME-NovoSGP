using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private async Task ConsolidarAcompanhamentoAprendizagemAluno()
        {
            var anoAtual = DateTime.Now.Year;

            var turmas = await mediator.Send(new ObterTurmasInfantilQuery(anoAtual));

            await mediator.Send(new LimparConsolidacaoAcompanhamentoAprendizagemCommand(anoAtual));

            await PublicarMensagemConsolidar(turmas, anoAtual);

            await AtualizarDataExecucao(anoAtual);
        }

        private async Task PublicarMensagemConsolidar(IEnumerable<TurmaDTO> turmas, int anoLetivo)
        {
            if (turmas == null && !turmas.Any())
                throw new NegocioException("Não foi possível localizar turmas para consolidar dados de Média de Registros Individuais");

            var turmasTeste = new List<TurmaDTO>
            {
                new TurmaDTO() { TurmaId = 621623 },
                new TurmaDTO() { TurmaId = 621624 },
                new TurmaDTO() { TurmaId = 621625 },
                new TurmaDTO() { TurmaId = 621626 },
                new TurmaDTO() { TurmaId = 626178 },
                new TurmaDTO() { TurmaId = 631597 }
            };

            foreach (var turma in turmasTeste)
            {
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar, new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(turma.TurmaId, anoLetivo, 1), Guid.NewGuid(), null));
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar, new FiltroAcompanhamentoAprendizagemAlunoTurmaDTO(turma.TurmaId, anoLetivo, 2), Guid.NewGuid(), null));
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
        }

        private async Task<bool> ExecutarConsolidacao()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma, DateTime.Now.Year));
            if (parametroExecucao != null)
                return parametroExecucao.Ativo;

            return false;
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoAcompanhamentoAprendizagemAlunoTurma, ano));
            if (parametroSistema != null)
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");
                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}
