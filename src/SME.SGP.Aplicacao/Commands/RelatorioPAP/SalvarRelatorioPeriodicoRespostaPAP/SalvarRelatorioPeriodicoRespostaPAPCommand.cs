using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoRespostaPAPCommand : IRequest<long>
    {
        public SalvarRelatorioPeriodicoRespostaPAPCommand(string resposta, TipoQuestao tipoQuestao, long relatorioPeriodicoQuestaoId)
        {
            Resposta = resposta;
            TipoQuestao = tipoQuestao;
            RelatorioPeriodicoQuestaoId = relatorioPeriodicoQuestaoId;
        }

        public string Resposta { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public long RelatorioPeriodicoQuestaoId { get; set; }
    }

    public class SalvarRelatorioPeriodicoRespostaPAPCommandValidator : AbstractValidator<SalvarRelatorioPeriodicoRespostaPAPCommand>
    {
        public SalvarRelatorioPeriodicoRespostaPAPCommandValidator()
        {
            RuleFor(x => x.Resposta)
                .NotEmpty()
                .WithMessage("O resposta relatório PAP deve ser informada para cadastro da resposta!");
            RuleFor(x => x.TipoQuestao)
               .NotEmpty()
               .WithMessage("O tipo questão relatório PAP deve ser informada para cadastro da resposta!");
            RuleFor(x => x.RelatorioPeriodicoQuestaoId)
               .NotEmpty()
               .WithMessage("O id do relatório periodico questão PAP deve ser informada para cadastro da resposta!");
        }
    }
}
