using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase : AbstractUseCase, IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase
    {

        public ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var anoAtual = DateTime.Now.Year;
            var parametrosGeracaoPendenciaAvaliacao = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.DiasGeracaoPendenciaAvaliacao, anoAtual));

            await ExecutarVerificacaoPendenciaProfessor(parametrosGeracaoPendenciaAvaliacao);
            await ExecutarVerificacaoPendenciaCP(parametrosGeracaoPendenciaAvaliacao);

            return true;
        }

        private async Task ExecutarVerificacaoPendenciaProfessor(IEnumerable<ParametrosSistema> parametrosGeracaoPendenciaAvaliacao)
        {
            var parametroDiasProfessor = parametrosGeracaoPendenciaAvaliacao.FirstOrDefault(c => c.Ativo && c.Nome == "DiasGeracaoPendenciaAvaliacaoProfessor");
            if (parametroDiasProfessor != null)
                await mediator.Send(new ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand(int.Parse(parametroDiasProfessor.Valor)));
        }

        private async Task ExecutarVerificacaoPendenciaCP(IEnumerable<ParametrosSistema> parametrosGeracaoPendenciaAvaliacao)
        {
            var parametroDiasCP = parametrosGeracaoPendenciaAvaliacao.FirstOrDefault(c => c.Ativo && c.Nome == "DiasGeracaoPendenciaAvaliacaoCP");
            if (parametroDiasCP != null)
                await mediator.Send(new ExecutarVerificacaoPendenciaAvaliacaoCPCommand(int.Parse(parametroDiasCP.Valor)));
        }
    }
}
