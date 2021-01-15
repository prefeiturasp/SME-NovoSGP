using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaNomePorListaRFQuery : IRequest<IEnumerable<ProfessorResumoDto>>
    {
        public IEnumerable<string> CodigosRf { get; set; }

        public ObterListaNomePorListaRFQuery(IEnumerable<string> codigosRf)
        {
            CodigosRf = codigosRf;
        }
    }

    public class ObterListaNomePorListaRFQueryValidator : AbstractValidator<ObterListaNomePorListaRFQuery>
    {
        public ObterListaNomePorListaRFQueryValidator()
        {
            RuleForEach(x => x.CodigosRf)
                .NotEmpty()
                .WithMessage("O código do usuário deve ser informado.");
        }
    }
}