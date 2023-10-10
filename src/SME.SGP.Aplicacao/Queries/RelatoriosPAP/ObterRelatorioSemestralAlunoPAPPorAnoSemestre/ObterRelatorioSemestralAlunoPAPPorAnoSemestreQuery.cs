using MediatR;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralAlunoPAPPorAnoSemestreQuery : IRequest<IEnumerable<long>>
    {
        public ObterRelatorioSemestralAlunoPAPPorAnoSemestreQuery(int anoLetivo, int semestre)
        {
            AnoLetivo = anoLetivo;
            Semestre = semestre;

        }
        public int AnoLetivo { get; set; }
        public int Semestre { get; set; }
    }
    
    public class ObterRelatorioSemestralTurmaPAPPorAnoSemestreQueryValidator : AbstractValidator<ObterRelatorioSemestralAlunoPAPPorAnoSemestreQuery>
    {
        public ObterRelatorioSemestralTurmaPAPPorAnoSemestreQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para a busca de relatório semestral turma PAP.");

            RuleFor(x => x.Semestre)
                .NotEmpty()
                .WithMessage("O semestre deve ser informado para a busca de relatório semestral turma PAP.");
        }
    }
}
