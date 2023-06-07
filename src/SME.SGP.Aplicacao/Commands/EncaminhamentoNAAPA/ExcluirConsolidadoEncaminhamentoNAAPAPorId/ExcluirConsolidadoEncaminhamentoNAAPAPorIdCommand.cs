using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommand : IRequest<bool>
    {
        public ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommandValidator : AbstractValidator<ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommand>
    {
        public ExcluirConsolidadoEncaminhamentoNAAPAPorIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Você deve informar um Id para exclusão da registro de consolidação");
        }
    }
}