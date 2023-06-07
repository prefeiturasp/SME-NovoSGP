using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommand : IRequest<bool>
    {
        public ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommandValidator : AbstractValidator<ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommand>
    {
        public ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Você deve informar um Id para exclusão da registro de consolidação");
        }
    }
}