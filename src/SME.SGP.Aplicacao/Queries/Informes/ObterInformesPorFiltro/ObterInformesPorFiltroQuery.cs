using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Informes;

namespace SME.SGP.Aplicacao
{
    public class ObterInformesPorFiltroQuery : IRequest<PaginacaoResultadoDto<InformeResumoDto>>
    {
        public ObterInformesPorFiltroQuery(InformeFiltroDto filtro)
        {
            Filtro = filtro;
        }

        public InformeFiltroDto Filtro {  get; set; }
    }

    public class ObterInformesPorFiltroQueryValidator : AbstractValidator<ObterInformesPorFiltroQuery>
    {
        public ObterInformesPorFiltroQueryValidator()
        {
            RuleFor(c => c.Filtro)
                .NotEmpty()
                .WithMessage("O filtro deve ser informado para a pesquisa dos informes.");
        }
    }
}
