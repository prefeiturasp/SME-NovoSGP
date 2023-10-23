using FluentValidation;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterEstruturaInstuticionalVigentePorTurmaQuery : IRequest<EstruturaInstitucionalRetornoEolDTO>
    {
        public ObterEstruturaInstuticionalVigentePorTurmaQuery(string[] codigosTurma)
        {
            CodigosTurma = codigosTurma;
        }
        public string[] CodigosTurma { get; set; }
    }
    
    public class ObterEstruturaInstuticionalVigentePorTurmaQueryValidator : AbstractValidator<ObterEstruturaInstuticionalVigentePorTurmaQuery>
    {
        public ObterEstruturaInstuticionalVigentePorTurmaQueryValidator()
        {
            RuleFor(x => x.CodigosTurma)
            .NotNull()
            .WithMessage("Os códigos da turma devem ser informados.");
        }
    }
}
