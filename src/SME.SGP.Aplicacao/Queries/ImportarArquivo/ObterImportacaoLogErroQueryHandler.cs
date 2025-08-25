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
    public class ObterImportacaoLogErroQueryHandler : ConsultasBase, IRequestHandler<ObterImportacaoLogErroQuery, PaginacaoResultadoDto<ImportacaoLogErroQueryRetornoDto>>
    {
        private readonly IRepositorioImportacaoLogErro repositorioImportacaoLogErro;

        public ObterImportacaoLogErroQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioImportacaoLogErro repositorioImportacaoLogErro) : base(contextoAplicacao)
        {
            this.repositorioImportacaoLogErro = repositorioImportacaoLogErro ?? throw new ArgumentNullException(nameof(repositorioImportacaoLogErro));
        }

        public async Task<PaginacaoResultadoDto<ImportacaoLogErroQueryRetornoDto>> Handle(
            ObterImportacaoLogErroQuery request,
            CancellationToken cancellationToken)
        {
            var importacaoLogErros = await repositorioImportacaoLogErro.ObterImportacaoLogErroPaginada(Paginacao, request.Filtros);
            var erros = importacaoLogErros.Items.Select(erro =>
                            new ImportacaoLogErroQueryRetornoDto()
                            {
                                LinhaArquivo = erro.LinhaArquivo, 
                                MotivoFalha = erro.MotivoFalha
                            }).ToList();

            return new PaginacaoResultadoDto<ImportacaoLogErroQueryRetornoDto>
            {
                TotalPaginas = importacaoLogErros.TotalPaginas,
                TotalRegistros = importacaoLogErros.TotalRegistros,
                Items = erros
            };
        }
    }
}
