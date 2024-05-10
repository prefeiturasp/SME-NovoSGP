using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoNAAPASecaoQuestaoCommand : IRequest<long>
    {
        public long SecaoId { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarEncaminhamentoNAAPASecaoQuestaoCommand(long secaoId, long questaoId)
        {
            SecaoId = secaoId;
            QuestaoId = questaoId;
        }
    }
    public class RegistrarEncaminhamentoNAAPASecaoQuestaoCommandValidator : AbstractValidator<RegistrarEncaminhamentoNAAPASecaoQuestaoCommand>
    {
        public RegistrarEncaminhamentoNAAPASecaoQuestaoCommandValidator()
        {
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Seção do Encaminhamento deve ser informada para registrar encaminhamento naapa!");
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Encaminhamento deve ser informada para registrar encaminhamento naapa!");
        }
    }
}
