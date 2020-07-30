using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorCodigoEolNomeQuery : IRequest<IEnumerable<AlunoSimplesDto>>
    {
        public ObterAlunosPorCodigoEolNomeQuery(FiltroBuscaAlunosDto dto)
        {
            CodigoUe = dto.CodigoUe;
            AnoLetivo = dto.AnoLetivo;
            CodigoEOL = dto.Codigo;
            Nome = dto.Nome;
        }

        public string CodigoUe { get; set; }
        public string AnoLetivo { get; set; }
        public string CodigoEOL { get; set; }
        public string Nome { get; set; }
    }

    public class ObterAlunosPorCodigoEolNomeQueryValidator : AbstractValidator<ObterAlunosPorCodigoEolNomeQuery>
    {
        public ObterAlunosPorCodigoEolNomeQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da ue deve ser informado.");
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
