using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
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
            var paginacao = new Paginacao(request.NumeroPagina, request.NumeroRegistros);
            var importacoesLog = await repositorioImportacaoLog.ObterImportacaoLogPaginada(paginacao);

            var arquivos = importacoesLog.Items.Select(arquivo => new ImportacaoLogQueryRetornoDto()
            {
                Id = arquivo.Id,
                NomeArquivo = arquivo.NomeArquivo,
                StatusImportacao = arquivo.StatusImportacao,
                RegistrosProcessados = arquivo.RegistrosProcessados.HasValue ? arquivo.RegistrosProcessados.Value : 0,
                TotalRegistros = arquivo.TotalRegistros.HasValue ? arquivo.TotalRegistros.Value : 0
            })
                .ToList();

            return new PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>
            {
                TotalPaginas = importacoesLog.TotalPaginas,
                TotalRegistros = importacoesLog.TotalRegistros,
                Items = arquivos
            };
        }
    }
}
