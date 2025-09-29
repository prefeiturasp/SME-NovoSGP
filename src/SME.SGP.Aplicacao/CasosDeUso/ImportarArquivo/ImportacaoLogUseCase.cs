using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Queries.ImportarArquivo;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo
{
    public class ImportacaoLogUseCase : IImportacaoLogUseCase
    {
        private readonly IMediator mediator;

        public ImportacaoLogUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>> Executar(FiltroPesquisaImportacaoDto filtro)
        {
            var numeroPagina = filtro.NumeroPagina < 1 ? 1 : filtro.NumeroPagina;
            var numeroRegistros = filtro.NumeroRegistros < 1 ? 10 : filtro.NumeroRegistros;

            var paginacao = new Paginacao(numeroPagina, numeroRegistros);

            if (paginacao.QuantidadeRegistrosIgnorados < 0)
                paginacao = new Paginacao(1, numeroRegistros);

            return mediator.Send(new ObterImportacaoLogQuery(paginacao, filtro));
        }
    }
}
