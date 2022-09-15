using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class CriarCacheAulaPrevistaCommand : IRequest<IEnumerable<AulaPrevista>>
    {
        public CriarCacheAulaPrevistaCommand(string nomeChave, long codigoUe)
        {
            NomeChave = nomeChave;
            CodigoUe = codigoUe;
        }

        public long CodigoUe { get; }
        public string NomeChave { get; }
    }

    public class CriarCacheAulaPrevistaCommandValidator : AbstractValidator<CriarCacheAulaPrevistaCommand>
    {
        public CriarCacheAulaPrevistaCommandValidator()
        {
            RuleFor(x => x.NomeChave).NotNull().NotEmpty().WithMessage("É preciso informar o nome da chave para criar aula prevista no cache");
            RuleFor(x => x.CodigoUe).GreaterThan(0).WithMessage("É preciso informar o código da UE para criar aula prevista no cache");
        }
    }
}