using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorFiltroQuery : IRequest<IEnumerable<AlunoSimplesDto>>
    {
        public ObterAlunosPorFiltroQuery(string codigoUe,
                                         int anoLetivo,
                                         string nomeAluno,
                                         string codigoEol)
        {
            this.CodigoUe = codigoUe;
            this.AnoLetivo = anoLetivo;
            this.NomeAluno = nomeAluno;
            this.CodigoEol = codigoEol;
        }

        public string CodigoUe { get; set; }

        public int AnoLetivo { get; set; }

        public string NomeAluno { get; set; }

        public string CodigoEol { get; set; }
    }

    public class ObterAlunosPorFiltroQueryValidator : AbstractValidator<ObterAlunosPorFiltroQuery>
    {
        public ObterAlunosPorFiltroQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado.");

            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
