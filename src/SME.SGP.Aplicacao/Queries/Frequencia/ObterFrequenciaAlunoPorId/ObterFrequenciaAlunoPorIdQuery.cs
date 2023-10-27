using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoPorIdQuery : IRequest<FrequenciaAluno>
    {
        public ObterFrequenciaAlunoPorIdQuery(long frequenciaAlunoId)
        {
            FrequenciaAlunoId = frequenciaAlunoId;
        }

        public long FrequenciaAlunoId { get; set; }
    }

    public class ObterFrequenciaAlunoPorIdQueryValidator : AbstractValidator<ObterFrequenciaAlunoPorIdQuery>
    {
        public ObterFrequenciaAlunoPorIdQueryValidator()
        {
            RuleFor(x => x.FrequenciaAlunoId)
                .GreaterThan(0)
                .WithMessage("O id de frequência aluno deve ser informado");
        }
    }
}
