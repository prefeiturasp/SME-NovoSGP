using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorRFsQuery : IRequest<IEnumerable<ProfessorResumoDto>>
    {
        public IEnumerable<string> CodigosRf { get; set; }

        public ObterFuncionariosPorRFsQuery(IEnumerable<string> codigosRf)
        {
            CodigosRf = codigosRf;
        }
    }

    public class ObterListaNomePorListaRFQueryValidator : AbstractValidator<ObterFuncionariosPorRFsQuery>
    {
        public ObterListaNomePorListaRFQueryValidator()
        {
            RuleForEach(x => x.CodigosRf)
                .NotEmpty()
                .WithMessage("O código do usuário deve ser informado.");
        }
    }
}