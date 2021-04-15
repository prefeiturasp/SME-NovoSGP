using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarCicloEnsinoCommand : IRequest<AuditoriaDto>
    {
        public SalvarCicloEnsinoCommand(CicloRetornoDto cicloEnsino)
        {
            CicloEnsino = cicloEnsino;
        }

        public CicloRetornoDto CicloEnsino { get; set; }
    }
    public class SalvarCicloEnsinoCommandValidator : AbstractValidator<SalvarCicloEnsinoCommand>
    {
        public SalvarCicloEnsinoCommandValidator()
        {
            RuleFor(a => a.CicloEnsino)
                   .NotEmpty()
                   .WithMessage("O ciclo deve ser informado!");
        }
    }
}
