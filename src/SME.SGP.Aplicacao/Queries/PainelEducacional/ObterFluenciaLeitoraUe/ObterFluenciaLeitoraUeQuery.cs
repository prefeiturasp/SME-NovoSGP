using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitoraUe
{
    public class ObterFluenciaLeitoraUeQuery : IRequest<IEnumerable<PainelEducacionalFluenciaLeitoraUeDto>>
    {
        public ObterFluenciaLeitoraUeQuery(FiltroPainelEducacionalFluenciaLeitoraUe filtro)
        {
            Filtro = filtro;
        }
        public FiltroPainelEducacionalFluenciaLeitoraUe Filtro { get; set; }
    }

    public class ObterFluenciaLeitoraUeQueryValidator : AbstractValidator<ObterFluenciaLeitoraUeQuery>
    {
        public ObterFluenciaLeitoraUeQueryValidator()
        {
            RuleFor(c => c.Filtro.AnoLetivo)
            .NotEmpty()
            .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Filtro.TipoAvaliacao)
            .NotEmpty()
            .WithMessage("O tipo de avaliação deve ser informado.");

            RuleFor(c => c.Filtro.CodigoUe)
            .NotEmpty()
            .WithMessage("O codigoUe deve ser informado.");
        }
    }
}
