using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand : IRequest<long>
    {
        public string Resposta { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand(string resposta, long questaoId, TipoQuestao tipoQuestao)
        {
            Resposta = resposta;
            QuestaoId = questaoId;
            TipoQuestao = tipoQuestao;
        }
    }
    public class RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandValidator : AbstractValidator<RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand>
    {
        public RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandValidator()
        {           
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Encaminhamento deve ser informada!");
        }
    }
}
