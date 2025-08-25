using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Queries.ImportarArquivo
{
    public class ObterImportacaoLogErroQuery : IRequest<PaginacaoResultadoDto<ImportacaoLogErroQueryRetornoDto>>
    {
        public ObterImportacaoLogErroQuery(Paginacao paginacao, FiltroPesquisaImportacaoDto filtros) 
        {
            NumeroPagina = paginacao.QuantidadeRegistrosIgnorados;
            NumeroRegistros = paginacao.QuantidadeRegistros;
            Filtros = filtros;
        }
        public int NumeroPagina { get; set; }
        public int NumeroRegistros { get; set; }
        public FiltroPesquisaImportacaoDto Filtros { get; }

        public class ObterImportacaoLogErroQueryValidator : AbstractValidator<ObterImportacaoLogErroQuery>
        {
            public ObterImportacaoLogErroQueryValidator()
            {
                RuleFor(x => x.Filtros.ImportacaoLogId).NotEmpty().WithMessage("Informe o Id da importação para obter o log de falhas");
            }
        }
    }
}
