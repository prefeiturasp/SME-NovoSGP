using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarRelatorioPeriodicoRespostaPAPCommand : IRequest<long>
    {
        public AlterarRelatorioPeriodicoRespostaPAPCommand(RelatorioPeriodicoPAPResposta resposta, RelatorioPAPRespostaDto respostaDto)
        {
            Resposta = resposta;
            RespostaDto = respostaDto;
        }

        public RelatorioPeriodicoPAPResposta Resposta { get; set; }
        public RelatorioPAPRespostaDto RespostaDto { get; set; }    
    }

    public class AlterarRelatorioPeriodicoRespostaPAPCommandValidator : AbstractValidator<AlterarRelatorioPeriodicoRespostaPAPCommand>
    {
        public AlterarRelatorioPeriodicoRespostaPAPCommandValidator()
        {
            RuleFor(x => x.Resposta)
                   .NotEmpty()
                   .WithMessage("O objeto relatório periodico resposta pap deve ser informado para registar sua alteração!");
            RuleFor(x => x.RespostaDto)
                    .NotEmpty()
                    .WithMessage("O dto relatório periodico resposta pap deve ser informado para registar sua alteração!");
        }
    }
}
