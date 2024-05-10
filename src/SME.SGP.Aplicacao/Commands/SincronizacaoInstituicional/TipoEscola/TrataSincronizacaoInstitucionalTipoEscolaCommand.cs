using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTipoEscolaCommand : IRequest<bool>
    {
        public TrataSincronizacaoInstitucionalTipoEscolaCommand(TipoEscolaEol tipoEscolaSGP, TipoEscolaRetornoDto tipoEscolaEol)
        {
            TipoEscolaSGP = tipoEscolaSGP;
            TipoEscolaEol = tipoEscolaEol;
        }

        public TipoEscolaEol TipoEscolaSGP { get; set; }
        public TipoEscolaRetornoDto TipoEscolaEol { get; set; }
    }
    public class TrataSincronizacaoInstitucionalTipoEscolaCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalTipoEscolaCommand>
    {
        public TrataSincronizacaoInstitucionalTipoEscolaCommandValidator()
        {
            RuleFor(c => c.TipoEscolaEol)
                .NotEmpty()
                .WithMessage("A Tipo Escola do Eol deve ser informada.");
        }
    }
}
