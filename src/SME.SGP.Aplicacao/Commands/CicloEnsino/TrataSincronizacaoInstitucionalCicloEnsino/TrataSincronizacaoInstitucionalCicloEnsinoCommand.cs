using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalCicloEnsinoCommand : IRequest<bool>
    {
        public TrataSincronizacaoInstitucionalCicloEnsinoCommand(CicloRetornoDto cicloEol, CicloEnsino cicloSgp)
        {
            CicloEol = cicloEol;
            CicloSgp = cicloSgp;
        }

        public CicloRetornoDto CicloEol { get; set; }
        public CicloEnsino CicloSgp { get; set; }
    }
    public class TrataSincronizacaoInstitucionalCicloEnsinoCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalCicloEnsinoCommand>
    {
        public TrataSincronizacaoInstitucionalCicloEnsinoCommandValidator()
        {
            RuleFor(a => a.CicloEol)
                   .NotEmpty()
                   .WithMessage("O cicloEol deve ser informado!");
        }
    }
}
