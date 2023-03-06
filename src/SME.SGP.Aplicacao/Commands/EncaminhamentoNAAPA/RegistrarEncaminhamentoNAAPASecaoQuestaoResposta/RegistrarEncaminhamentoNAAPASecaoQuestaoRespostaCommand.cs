using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand : IRequest<long>
    {
        public string Resposta { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(string resposta, long questaoId, TipoQuestao tipoQuestao)
        {
            Resposta = resposta;
            QuestaoId = questaoId;
            TipoQuestao = tipoQuestao;
        }
    }
    public class RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommandValidator : AbstractValidator<RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand>
    {
        public RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommandValidator()
        {           
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Encaminhamento NAAPA deve ser informada!");
        }
    }
}
