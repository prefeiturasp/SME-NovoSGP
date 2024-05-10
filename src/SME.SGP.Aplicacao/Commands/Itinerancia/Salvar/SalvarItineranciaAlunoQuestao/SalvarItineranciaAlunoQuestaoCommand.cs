using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaAlunoQuestaoCommand : IRequest<AuditoriaDto>
    {
        public SalvarItineranciaAlunoQuestaoCommand(long questaoId, long itineranciaAlunoId, string resposta)
        {
            QuestaoId = questaoId;
            ItineranciaAlunoId = itineranciaAlunoId;
            Resposta = resposta;
        }

        public long QuestaoId { get; set; }
        public long ItineranciaAlunoId { get; set; }
        public string Resposta { get; set; }
    }

    public class SalvarItineranciaAlunoQuestaoCommandValidator : AbstractValidator<SalvarItineranciaAlunoQuestaoCommand>
    {
        public SalvarItineranciaAlunoQuestaoCommandValidator()
        {
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O id da questão do aluno da itinerância deve ser informado!");
            RuleFor(x => x.ItineranciaAlunoId)
                   .GreaterThan(0)
                   .WithMessage("O id da itinerância do aluno deve ser informado!");
            RuleFor(x => x.Resposta)
                   .NotEmpty()
                   .WithMessage("A resposta da questão deve ser informada!");
        }
    }
}
