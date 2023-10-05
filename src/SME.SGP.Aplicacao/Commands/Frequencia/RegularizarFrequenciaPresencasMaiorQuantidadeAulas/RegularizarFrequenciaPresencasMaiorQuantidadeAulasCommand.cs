using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand : IRequest<bool>
    {
        public RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand(long frequenciaAlunoId)
        {
            FrequenciaAlunoId = frequenciaAlunoId;
        }

        public long FrequenciaAlunoId { get; set; }
    }

    public class RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandValidator : AbstractValidator<RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand>
    {
        public RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandValidator()
        {
            RuleFor(x => x.FrequenciaAlunoId)
                .GreaterThan(0)
                .WithMessage("O Id da frequência aluno deve ser informado");
        }
    }
}
