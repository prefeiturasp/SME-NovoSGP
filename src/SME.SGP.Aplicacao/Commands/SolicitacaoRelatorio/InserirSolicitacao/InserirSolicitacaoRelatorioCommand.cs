using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Aplicacao
{
    public class InserirSolicitacaoRelatorioCommand : IRequest<long>
    {
        public InserirSolicitacaoRelatorioCommand(SolicitacaoRelatorio solicitacaoRelatorio)
        {
            SolicitacaoRelatorio = solicitacaoRelatorio;
        }

        public SolicitacaoRelatorio SolicitacaoRelatorio { get; set; }
    }
    public class InserirSolicitacaoRelatorioCommandValidator : AbstractValidator<InserirSolicitacaoRelatorioCommand>
    {
        public InserirSolicitacaoRelatorioCommandValidator()
        {
            RuleFor(c => c.SolicitacaoRelatorio).NotEmpty();
        }
    }
}
