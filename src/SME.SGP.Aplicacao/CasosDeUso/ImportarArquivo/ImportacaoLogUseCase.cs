using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Queries.ImportarArquivo;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo
{
    public class ImportacaoLogUseCase : ConsultasBase, IImportacaoLogUseCase
    {
        private readonly IMediator mediator;
        public ImportacaoLogUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator;
        }
        public Task<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>> Executar(FiltroPesquisaImportacaoDto filtro)
        {
            return mediator.Send(new ObterImportacaoLogQuery(Paginacao, filtro));
        }
    }
}
