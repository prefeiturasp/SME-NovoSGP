using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosAlunosFechamentoQuery : IRequest<IEnumerable<AlunoDadosBasicosDto>>
    {
        public ObterDadosAlunosFechamentoQuery(string turmaCodigo, int anoLetivo, int semestre)
        {
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
            Semestre = semestre;
        }

        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public int Semestre { get; set; }
    }

    public class ObterDadosAlunosFechamentoQueryValidator : AbstractValidator<ObterDadosAlunosFechamentoQuery>
    {
        public ObterDadosAlunosFechamentoQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.AnoLetivo)
               .NotNull()
               .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Semestre)
               .NotNull()
               .WithMessage("O semestre deve ser informado.");
        }
    }
}
