using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPreDefinicaoFrequenciaCommand : IRequest<bool>
    {
        public ExcluirPreDefinicaoFrequenciaCommand(long turmaId, long componenteCurricularId, string[] alunosComFrequenciaRegistrada)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AlunosComFrequenciaRegistrada = alunosComFrequenciaRegistrada;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string[] AlunosComFrequenciaRegistrada { get; set; }
    }

    public class ExcluirPreDefinicaoFrequenciaCommandValidator : AbstractValidator<ExcluirPreDefinicaoFrequenciaCommand>
    {
        public ExcluirPreDefinicaoFrequenciaCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O Id da Turma deve ser informado");
            
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O Id do componente curricular deve ser informado");

            RuleForEach(a => a.AlunosComFrequenciaRegistrada)
                .NotEmpty()
                .WithMessage("O código dos alunos com frequencia deve ser infomado para limpar as frequencias dos alunos");
        }
    }
}
