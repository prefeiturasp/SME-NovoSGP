using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ImportarArquivo
{
    public class ObterImportacaoLogQueryHandler : IRequestHandler<ObterImportacaoLogQuery, PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>>
    {
        private readonly IRepositorioImportacaoLog repositorioImportacaoLog;

        public ObterImportacaoLogQueryHandler(IRepositorioImportacaoLog repositorioImportacaoLog)
        {
            this.repositorioImportacaoLog = repositorioImportacaoLog ?? throw new ArgumentNullException(nameof(repositorioImportacaoLog));
        }

        public async Task<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>> Handle(
            ObterImportacaoLogQuery request,
            CancellationToken cancellationToken)
        {
            var numeroPagina = request.NumeroPagina < 1 ? 1 : request.NumeroPagina;
            var numeroRegistros = request.NumeroRegistros < 1 ? 10 : request.NumeroRegistros;

            var paginacao = new Paginacao(numeroPagina, numeroRegistros);

            var importacoesLog = await repositorioImportacaoLog.ObterImportacaoLogPaginada(paginacao);

            var arquivos = importacoesLog.Items.Select(arquivo => new ImportacaoLogQueryRetornoDto()
            {
                Id = arquivo.Id,
                NomeArquivo = arquivo.NomeArquivo,
                StatusImportacao = arquivo.StatusImportacao,
                RegistrosProcessados = arquivo.RegistrosProcessados ?? 0,
                TotalRegistros = arquivo.TotalRegistros ?? 0
            }).ToList();

            return new PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>
            {
                TotalPaginas = importacoesLog.TotalPaginas,
                TotalRegistros = importacoesLog.TotalRegistros,
                Items = arquivos
            };
        }
    }
}
