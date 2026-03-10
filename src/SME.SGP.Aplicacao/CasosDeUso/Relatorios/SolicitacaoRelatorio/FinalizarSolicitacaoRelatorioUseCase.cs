using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio;
using SME.SGP.Dominio.Dtos;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Relatorios.SolicitacaoRelatorio
{
    public class FinalizarSolicitacaoRelatorioUseCase : AbstractUseCase, IFinalizarSolicitacaoRelatorioUseCase
    {
        private readonly IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio;

        public FinalizarSolicitacaoRelatorioUseCase(IMediator mediator, IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio) : base(mediator)
        {
            this.repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio;
        }

        public async Task Executar(FinalizarSolicitacaoRelatorioDto finalizarSolicitacaoRelatorioDto)
        {
            var solicitacaoRelatorio = await mediator.Send(new ObterSolicitacaoRelatorioPorIdQuery(finalizarSolicitacaoRelatorioDto.SolicitacaoRelatorioId));

            if (solicitacaoRelatorio == null) return;

            await mediator.Send(new FinalizarSolicitacaoRelatorioCommand(solicitacaoRelatorio));
            var relatorioCorrelacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery (finalizarSolicitacaoRelatorioDto.CodigoCorrelacao));

            if (relatorioCorrelacao == null) return;

            relatorioCorrelacao.UrlRelatorio = finalizarSolicitacaoRelatorioDto.UrlRelatorio;
            await repositorioCorrelacaoRelatorio.SalvarAsync(relatorioCorrelacao);
        }
    }
}


