using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Queries.ImportarArquivo;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Interfaces;
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
            var paginacao = new Paginacao(1, 10); 
            return mediator.Send(new ObterImportacaoLogQuery(paginacao, filtro));
        }

        public Task<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>> Executar(FiltroPesquisaImportacaoDto filtro, int numeroPagina, int numeroRegistros)
        {
            var paginacao = new Paginacao(numeroPagina, numeroRegistros);
            return mediator.Send(new ObterImportacaoLogQuery(paginacao, filtro));
        }
    }
}
