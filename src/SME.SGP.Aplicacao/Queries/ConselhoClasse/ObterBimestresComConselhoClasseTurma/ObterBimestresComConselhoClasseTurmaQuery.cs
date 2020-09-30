using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresComConselhoClasseTurmaQuery : IRequest<IEnumerable<BimestreComConselhoClasseTurmaDto>>
    {
        public long Id { get; set; }
        public ObterBimestresComConselhoClasseTurmaQuery(long id)
        {
            Id = id;
        }

        public class ObterBimestresComConselhoClasseTurmaValidator : AbstractValidator<ObterBimestresComConselhoClasseTurmaQuery>
        {
            public ObterBimestresComConselhoClasseTurmaValidator()
            {
                RuleFor(c => c.Id)
                    .NotEmpty()
                    .WithMessage("O código da turma deve ser informado.");
            }
        }
    }
}
