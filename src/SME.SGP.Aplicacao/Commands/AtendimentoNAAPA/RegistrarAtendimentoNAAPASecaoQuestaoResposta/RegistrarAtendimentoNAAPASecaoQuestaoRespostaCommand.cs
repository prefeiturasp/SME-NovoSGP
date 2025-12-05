using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand : IRequest<long>
    {
        public string Resposta { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand(string resposta, long questaoId, TipoQuestao tipoQuestao)
        {
            Resposta = resposta;
            QuestaoId = questaoId;
            TipoQuestao = tipoQuestao;
        }
    }
    public class RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommandValidator : AbstractValidator<RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand>
    {
        public RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommandValidator()
        {           
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Atendimento NAAPA deve ser informado!");
        }
    }
}
