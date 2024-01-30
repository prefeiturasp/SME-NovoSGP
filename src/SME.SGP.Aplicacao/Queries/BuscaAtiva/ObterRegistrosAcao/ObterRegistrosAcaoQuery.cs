using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAcaoQuery : IRequest<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>>
    {
        public ObterRegistrosAcaoQuery(FiltroRegistrosAcaoDto filtros)
        {
            Filtros = filtros;
        }
        public FiltroRegistrosAcaoDto Filtros { get; set; }
        
    }

    public class ObterRegistrosAcaoQueryValidator : AbstractValidator<ObterRegistrosAcaoQuery>
    {
        public ObterRegistrosAcaoQueryValidator()
        {
            RuleFor(c => c.Filtros).NotNull().WithMessage("Os filtros devem sem informados para pesquisa de Registros de Ação");
        }
    }
}
