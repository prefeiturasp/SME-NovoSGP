using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoRegistroAcaoCommand : IRequest<bool>
    {
        public AlterarQuestaoRegistroAcaoCommand(QuestaoRegistroAcaoBuscaAtiva questao)
        {
            Questao = questao;
        }

        public QuestaoRegistroAcaoBuscaAtiva Questao { get; }
    }

    public class AlterarQuestaoRegistroAcaoCommandValidator : AbstractValidator<AlterarQuestaoRegistroAcaoCommand>
    {
        public AlterarQuestaoRegistroAcaoCommandValidator()
        {
            RuleFor(c => c.Questao)
            .NotEmpty()
            .WithMessage("A questão do registro de ação deve ser informada para alteração.");
        }
    }
}
