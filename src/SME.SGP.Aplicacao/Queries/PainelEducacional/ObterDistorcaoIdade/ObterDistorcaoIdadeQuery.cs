using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDistorcaoIdade
{
    public class ObterDistorcaoIdadeQuery : IRequest<IEnumerable<PainelEducacionalDistorcaoIdadeDto>>
    {
        public ObterDistorcaoIdadeQuery(FiltroPainelEducacionalDistorcaoIdade filtro)
        {
            Filtro = filtro;
        }
        public FiltroPainelEducacionalDistorcaoIdade Filtro { get; set; }
    }

    public class ObterDistorcaoIdadeQueryValidator : AbstractValidator<ObterDistorcaoIdadeQuery>
    {
        public ObterDistorcaoIdadeQueryValidator()
        {
            RuleFor(c => c.Filtro.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
