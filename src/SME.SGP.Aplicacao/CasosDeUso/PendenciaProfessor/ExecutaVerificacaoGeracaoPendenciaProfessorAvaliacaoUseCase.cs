using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase : AbstractUseCase, IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase
    {
        private readonly IServicoEol servicoEol;

        public ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase(IMediator mediator, IServicoEol servicoEol) : base(mediator)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task Executar(MensagemRabbit mensagem)
        {
            var anoAtual = DateTime.Now.Year;
            var parametrosGeracaoPendenciaAvaliacao = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.DiasGeracaoPendenciaAvaliacao, anoAtual));

            await ExecutarVerificacaoPendenciaProfessor(parametrosGeracaoPendenciaAvaliacao);
            await ExecutarVerificacaoPendenciaCP(parametrosGeracaoPendenciaAvaliacao);
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
