using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPorObjetivoAprendizagemJuremaQuery : IRequest<long>
    {
    
        public ObterIdPorObjetivoAprendizagemJuremaQuery(long planoId, long objetivoAprendizagemJuremaId)
        {
            PlanoId = planoId;
            ObjetivoAprendizagemJuremaId = objetivoAprendizagemJuremaId;
        }

        public long PlanoId { get; set; }
        public long ObjetivoAprendizagemJuremaId { get; set; }
        
    }

    public class ObterIdPorObjetivoAprendizagemJuremaQueryValidator : AbstractValidator<ObterIdPorObjetivoAprendizagemJuremaQuery>
    {
        public ObterIdPorObjetivoAprendizagemJuremaQueryValidator()
        {
            RuleFor(a => a.PlanoId)
                .NotEmpty()
                .WithMessage("O ID do Plano precisa ser informado.");
            RuleFor(a => a.ObjetivoAprendizagemJuremaId)
                .NotEmpty()
                .WithMessage("O id do jurema precisa ser informado.");
        }
    }
}
