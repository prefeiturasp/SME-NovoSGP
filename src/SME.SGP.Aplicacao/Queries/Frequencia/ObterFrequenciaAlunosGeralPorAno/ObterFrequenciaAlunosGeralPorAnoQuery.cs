using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosGeralPorAnoQuery : IRequest<IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>>
    {
        public ObterFrequenciaAlunosGeralPorAnoQuery(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; set; }
    }

    public class ObterFrequenciaAlunosGeralPorAnoQueryValidator : AbstractValidator<ObterFrequenciaAlunosGeralPorAnoQuery>
    {
        public ObterFrequenciaAlunosGeralPorAnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano precisa ser informada");
        }
    }
}
