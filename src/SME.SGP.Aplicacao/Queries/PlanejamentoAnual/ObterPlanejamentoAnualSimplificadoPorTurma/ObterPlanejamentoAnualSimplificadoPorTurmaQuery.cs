using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualSimplificadoPorTurmaQuery : IRequest<PlanejamentoAnualDto>
    {
        public ObterPlanejamentoAnualSimplificadoPorTurmaQuery(long turmaId)
        {            
            TurmaId = turmaId;            
        }       
        public long TurmaId { get; set; }        
    }

    public class ObterPlanejamentoAnualSimplificadoPorTurmaQueryValidator : AbstractValidator<ObterPlanejamentoAnualSimplificadoPorTurmaQuery>
    {
        public ObterPlanejamentoAnualSimplificadoPorTurmaQueryValidator()
        {           
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O Id da turma deve ser informado.");            
        }
    }
}
