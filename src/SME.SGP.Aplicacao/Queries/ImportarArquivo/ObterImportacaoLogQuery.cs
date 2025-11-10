using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Queries.ImportarArquivo
{
    public class ObterImportacaoLogQuery : IRequest<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>>
    {
        public ObterImportacaoLogQuery(Paginacao paginacao, FiltroPesquisaImportacaoDto filtros) 
        {
            Paginacao = paginacao;
            Filtros = filtros;
        }
        public Paginacao Paginacao { get; set; }
        public FiltroPesquisaImportacaoDto Filtros { get; }
    }
}
