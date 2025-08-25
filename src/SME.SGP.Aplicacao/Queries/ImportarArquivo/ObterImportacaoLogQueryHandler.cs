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
    public class ObterImportacaoLogQueryHandler : ConsultasBase, IRequestHandler<ObterImportacaoLogQuery, PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>>
    {
        private readonly IRepositorioImportacaoLog repositorioImportacaoLog;

        public ObterImportacaoLogQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioImportacaoLog repositorioImportacaoLog) : base(contextoAplicacao)
        {
            this.repositorioImportacaoLog = repositorioImportacaoLog ?? throw new ArgumentNullException(nameof(repositorioImportacaoLog));
        }

        public async Task<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>> Handle(
            ObterImportacaoLogQuery request,
            CancellationToken cancellationToken)
        {
            var importacoesLog = await repositorioImportacaoLog.ObterImportacaoLogPaginada(Paginacao);

            var arquivos = importacoesLog.Items.Select(arquivo => new ImportacaoLogQueryRetornoDto()
            {
                Id = arquivo.Id,
                NomeArquivo = arquivo.NomeArquivo,
                StatusImportacao = arquivo.StatusImportacao,
                RegistrosProcessados = arquivo.RegistrosProcessados.Value,
                TotalRegistros = arquivo.TotalRegistros.Value
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
