using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoTurmaPorIdQueryValidator : AbstractValidator<ObterAcompanhamentoTurmaPorIdQuery>
    {
        public ObterAcompanhamentoTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id do acompanhamento da turma deve ser informado para consulta.");
        }
    }
}