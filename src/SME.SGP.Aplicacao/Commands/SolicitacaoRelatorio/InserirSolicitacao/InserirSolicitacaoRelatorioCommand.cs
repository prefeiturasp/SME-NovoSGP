using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Dtos;

namespace SME.SGP.Aplicacao
{
    public class InserirSolicitacaoRelatorioCommand : IRequest<long>
    {
        public InserirSolicitacaoRelatorioCommand(SolicitacaoRelatorioDto solicitacaoRelatorio)
        {
            SolicitacaoRelatorio = solicitacaoRelatorio;
        }

        public SolicitacaoRelatorioDto SolicitacaoRelatorio { get; set; }
    }
    public class InserirSolicitacaoRelatorioCommandValidator : AbstractValidator<InserirSolicitacaoRelatorioCommand>
    {
        public InserirSolicitacaoRelatorioCommandValidator()
        {
            RuleFor(c => c.SolicitacaoRelatorio).NotEmpty();
        }
    }
}
