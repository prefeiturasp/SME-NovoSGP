using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaMesCommand : IRequest<bool>
    {
        public IncluirFilaConciliacaoFrequenciaTurmaMesCommand(string turmaCodigo, int mes)
        {
            TurmaCodigo = turmaCodigo;
            Mes = mes;
        }

        public string TurmaCodigo { get; }
        public int Mes { get; }
    }

    public class IncluirFilaConciliacaoFrequenciaTurmaMesCommandValidator : AbstractValidator<IncluirFilaConciliacaoFrequenciaTurmaMesCommand>
    {
        public IncluirFilaConciliacaoFrequenciaTurmaMesCommandValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para inclusão da fila de conciliação de frequência");

            RuleFor(a => a.Mes)
                .NotEmpty()
                .WithMessage("O mês da turma deve ser informado para inclusão da fila de conciliação de frequência");

        }
    }
}
