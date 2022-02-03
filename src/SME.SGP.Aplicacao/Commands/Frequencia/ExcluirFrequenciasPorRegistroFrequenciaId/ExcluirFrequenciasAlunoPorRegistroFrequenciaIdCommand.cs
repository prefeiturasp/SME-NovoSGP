using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand : IRequest<bool>
    {
        public ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand(long registroFrequenciaId, string[] alunosComFrequenciaRegistrada)
        {
            RegistroFrequenciaId = registroFrequenciaId;
            AlunosComFrequenciaRegistrada = alunosComFrequenciaRegistrada;
        }

        public long RegistroFrequenciaId { get; set; }
        public string[] AlunosComFrequenciaRegistrada { get; set; }
    }

    public class ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommandValidator : AbstractValidator<ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand>
    {
        public ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommandValidator()
        {
            RuleFor(a => a.RegistroFrequenciaId)
                .NotEmpty()
                .WithMessage("O Id do registro de frequência deve ser infomado para limpar as frequencias dos alunos");

            RuleForEach(a => a.AlunosComFrequenciaRegistrada)
                .NotEmpty()
                .WithMessage("O código dos alunos com frequencia deve ser infomado para limpar as frequencias dos alunos");
        }
    }
}
