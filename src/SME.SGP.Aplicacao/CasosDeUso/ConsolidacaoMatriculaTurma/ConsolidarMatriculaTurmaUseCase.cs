using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarMatriculaTurmaUseCase : AbstractUseCase, IConsolidarMatriculaTurmaUseCase
    {
        public ConsolidarMatriculaTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                if (!await ExecutarConsolidacaoMatricula())
                    return false;
                var ue = mensagem.ObterObjetoMensagem<UeMatriculaDto>();
                await ConsolidarMatriculasTurmasAnoAtual(ue.UeCodigo);
                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private async Task<bool> ExecutarConsolidacaoMatricula()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoInformacoesEscolares, DateTime.Now.Year));
            if (parametroExecucao != null)
                return parametroExecucao.Ativo;

            return false;
        }

        private async Task ConsolidarMatriculasTurmasAnoAtual(string codigoUe)
        {
            var anoAtual = DateTime.Now.Year;
            await mediator.Send(new LimparConsolidacaoMatriculaTurmaPorAnoCommand(anoAtual));
            var matriculasConsolidadas = await mediator.Send(new ObterMatriculasPorTurmaConsolidacaoQuery(anoAtual, codigoUe));
            foreach(var matricula in matriculasConsolidadas)
            {
                var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(matricula.TurmaCodigo));
                await mediator.Send(new RegistraConsolidacaoMatriculaTurmaCommand(turmaId, matricula.Quantidade));
            }
        }
    }
}
