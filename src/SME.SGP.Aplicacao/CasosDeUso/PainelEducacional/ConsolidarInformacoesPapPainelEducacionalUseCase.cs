using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirConsolidacaoPap;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDadosBrutosParaConsolidacaoIndicadoresPap;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterInformacoesPapUltimoAnoConsolidado;
using SME.SGP.Dominio.Interfaces.Servicos;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesPapPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesPapPainelEducacionalUseCase
    {
        private readonly IServicoPainelEducacionalConsolidacaoIndicadoresPap _servicoConsolidacaoIndicadoresPap;
        public ConsolidarInformacoesPapPainelEducacionalUseCase(
            IMediator mediator, IServicoPainelEducacionalConsolidacaoIndicadoresPap servicoIndicadoresPap)
            : base(mediator)
        {
            _servicoConsolidacaoIndicadoresPap = servicoIndicadoresPap ?? throw new ArgumentNullException(nameof(servicoIndicadoresPap));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoInicioConsolidacao = await DeterminarAnoInicioConsolidacao();
            var anoFimConsolidacao = DateTime.Now.Year;

            for (int anoLetivo = anoInicioConsolidacao; anoLetivo <= anoFimConsolidacao; anoLetivo++)
            {
                await ConsolidarAno(anoLetivo);
            }

            return true;
        }
        private async Task<int> DeterminarAnoInicioConsolidacao()
        {
            var ultimoAnoConsolidado = await mediator.Send(new ObterInformacoesPapUltimoAnoConsolidadoQuery());

            if (ultimoAnoConsolidado == 0)
                return PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE;

            if (ultimoAnoConsolidado == DateTime.Now.Year)
                return DateTime.Now.Year;

            return ultimoAnoConsolidado + 1;
        }

        private async Task ConsolidarAno(int anoLetivo)
        {
            var (dadosAlunos, indicadores, frequencia) = await mediator
                .Send(new ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery(anoLetivo));

            if (dadosAlunos == null || !dadosAlunos.Any())
                return;

            var (consolidacaoSme, consolidacaoDre, consolidacaoUe) =
                _servicoConsolidacaoIndicadoresPap.ConsolidarDados(dadosAlunos, indicadores, frequencia);

            await mediator.Send(new ExcluirConsolidacaoPapCommand(anoLetivo));
            await mediator.Send(new SalvarConsolidacaoPapSmeCommand(consolidacaoSme));
            await mediator.Send(new SalvarConsolidacaoPapDreCommand(consolidacaoDre));
            await mediator.Send(new SalvarConsolidacaoPapUeCommand(consolidacaoUe));
        }
    }
}