using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQueryValidator : AbstractValidator<ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQuery>
    {
        public ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQueryValidator()
        {
            RuleFor(x => x.ComunicadoId)
                .NotEmpty()
                .WithMessage("O comunicado é obrigatório.");

            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma é obrigatório.");
        }
    }
}