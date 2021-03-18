using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorCodigoAnoSemestreQuery : IRequest<FrequenciaAluno>
    {
        public ObterFrequenciaGeralAlunoPorCodigoAnoSemestreQuery(string codigoAluno, int anoTurma,long tipoCalendarioId = 0)
        {
            CodigoAluno = codigoAluno;
            AnoTurma = anoTurma;
            TipoCalendarioId = tipoCalendarioId;
        }

        public string CodigoAluno { get; set; }
        public int AnoTurma { get; set; }   
        public long TipoCalendarioId { get; set; }
    }

    public class ObterFrequenciaGeralAlunoPorCodigoAnoSemestreQueryValidator : AbstractValidator<ObterFrequenciaGeralAlunoPorCodigoAnoSemestreQuery>
    {
        public ObterFrequenciaGeralAlunoPorCodigoAnoSemestreQueryValidator()
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
