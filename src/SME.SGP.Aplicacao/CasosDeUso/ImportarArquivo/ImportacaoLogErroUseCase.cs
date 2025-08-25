using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Queries.ImportarArquivo;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ImportarArquivo
{
    public class ImportacaoLogErroUseCase : ConsultasBase, IImportacaoLogErroUseCase
    {
        private readonly IMediator mediator;
        public ImportacaoLogErroUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator;
        }
        public Task<PaginacaoResultadoDto<ImportacaoLogErroQueryRetornoDto>> Executar(FiltroPesquisaImportacaoDto filtro)
        {
            return mediator.Send(new ObterImportacaoLogErroQuery(Paginacao, filtro));
        }
    }
}
