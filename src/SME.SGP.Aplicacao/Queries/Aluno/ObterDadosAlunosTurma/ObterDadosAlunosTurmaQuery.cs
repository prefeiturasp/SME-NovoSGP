using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosAlunosTurmaQuery : IRequest<IEnumerable<AlunoDadosBasicosDto>>
    {
        public ObterDadosAlunosTurmaQuery(string turmaCodigo, int anoLetivo, PeriodoEscolar periodoEscolar = null, bool ehInfantil = false)
        {
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
            PeriodoEscolar = periodoEscolar;
            EhInfantil = ehInfantil;
        }

        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public bool EhInfantil { get; set; }
    }

    public class ObterDadosAlunosTurmaQueryValidator : AbstractValidator<ObterDadosAlunosTurmaQuery>
    {
        public ObterDadosAlunosTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.AnoLetivo)
               .NotNull()
               .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
