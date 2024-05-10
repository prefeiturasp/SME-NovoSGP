using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarRegistroAcaoSecaoQuestaoRespostaCommand : IRequest<long>
    {
        public string Resposta { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarRegistroAcaoSecaoQuestaoRespostaCommand(string resposta, long questaoId, TipoQuestao tipoQuestao)
        {
            Resposta = resposta;
            QuestaoId = questaoId;
            TipoQuestao = tipoQuestao;
        }
    }
    public class RegistrarRegistroAcaoSecaoQuestaoRespostaCommandValidator : AbstractValidator<RegistrarRegistroAcaoSecaoQuestaoRespostaCommand>
    {
        public RegistrarRegistroAcaoSecaoQuestaoRespostaCommandValidator()
        {           
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Registro de Ação deve ser informada!");
        }
    }
}
