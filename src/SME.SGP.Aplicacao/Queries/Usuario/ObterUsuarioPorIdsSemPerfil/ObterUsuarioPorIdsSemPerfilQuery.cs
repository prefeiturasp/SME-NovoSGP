using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorIdsSemPerfilQuery : IRequest<IEnumerable<Dominio.Usuario>>
    {
        public ObterUsuarioPorIdsSemPerfilQuery(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; set; }
    }

    public class ObterUsuarioPorIdsQueryValidator : AbstractValidator<ObterUsuarioPorIdsSemPerfilQuery>
    {
        public ObterUsuarioPorIdsQueryValidator()
        {
            RuleFor(c => c.Ids)
                .NotEmpty()
                .WithMessage("Os Ids dos usuários devem ser informados para consulta dos usuários.");
        }
    }
}
