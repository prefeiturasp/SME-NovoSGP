using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasGeralAlunoPorCodigoAnoSemestreQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciasGeralAlunoPorCodigoAnoSemestreQuery(string codigoAluno, int anoTurma, long tipoCalendarioId = 0)
        {
            CodigoAluno = codigoAluno;
            AnoTurma = anoTurma;
            TipoCalendarioId = tipoCalendarioId;
        }

        public string CodigoAluno { get; set; }
        public int AnoTurma { get; set; }
        public long TipoCalendarioId { get; set; }
    }

    public class ObterFrequenciasGeralAlunoPorCodigoAnoSemestreQueryValidator : AbstractValidator<ObterFrequenciasGeralAlunoPorCodigoAnoSemestreQuery>
    {
        public ObterFrequenciasGeralAlunoPorCodigoAnoSemestreQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Necessário informar o Código do Aluno");

            RuleFor(a => a.AnoTurma)
                .NotEmpty()
                .WithMessage("Necessário informar o Ano da Turma");
        }
    }
}
