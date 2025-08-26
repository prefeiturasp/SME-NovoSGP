using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Queries.ImportarArquivo
{
    public class ObterImportacaoLogQuery : IRequest<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>>
    {
        public ObterImportacaoLogQuery(Paginacao paginacao, FiltroPesquisaImportacaoDto filtros) 
        {
            NumeroPagina = paginacao.QuantidadeRegistrosIgnorados;
            NumeroRegistros = paginacao.QuantidadeRegistros;
            Filtros = filtros;
        }
        public int NumeroPagina { get; set; }
        public int NumeroRegistros { get; set; }
        public FiltroPesquisaImportacaoDto Filtros { get; }
    }
}
