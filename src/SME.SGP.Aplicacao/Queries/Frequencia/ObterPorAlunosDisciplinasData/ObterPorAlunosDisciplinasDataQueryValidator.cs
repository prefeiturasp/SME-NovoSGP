using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterPorAlunosDisciplinasDataQueryValidator : AbstractValidator<ObterPorAlunosDisciplinasDataQuery>
    {
        public ObterPorAlunosDisciplinasDataQueryValidator()
        {
            RuleFor(a => a.CodigosAlunos)
                .NotNull()
                .WithMessage("É preciso informar os códigos dos alunos para consultar as frequências.");
            RuleFor(a => a.DisciplinasIds)
                .NotNull()
                .WithMessage("É preciso informar os ids das disciplinas para consultar a frequências.");
            RuleFor(a => a.DataAtual)
                .NotEmpty()
                .WithMessage("É preciso informar a data atual para consultar a frequência.");
        }
    }
}
