using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRelatorioPeriodicoRespostaPAPCommand : IRequest<bool>
    {
        public ExcluirRelatorioPeriodicoRespostaPAPCommand(RelatorioPeriodicoPAPResposta resposta)
        {
            Resposta = resposta;
        }

        public RelatorioPeriodicoPAPResposta Resposta { get; set; }
    }

    public class ExcluirRelatorioPeriodicoRespostaPAPCommandValidator : AbstractValidator<ExcluirRelatorioPeriodicoRespostaPAPCommand>
    {
        public ExcluirRelatorioPeriodicoRespostaPAPCommandValidator()
        {
            RuleFor(x => x.Resposta)
                   .NotEmpty()
                   .WithMessage("O relatório periodico resposta pap deve ser informado para exclusão!");
        }
    }
}
