using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosResponsavelQuery : IRequest<IEnumerable<DadosResponsavelAlunoEolDto>>
    {
        public ObterDadosResponsavelQuery(string cpfResponsavel)
        {
            CpfResponsavel = cpfResponsavel;
        }

        public string CpfResponsavel { get; set; }
    }

    public class ObterDadosResponsaveisQueryValidator : AbstractValidator<ObterDadosResponsavelQuery>
    {
        public ObterDadosResponsaveisQueryValidator()
        {
            RuleFor(x => x.CpfResponsavel).NotEmpty().WithMessage("O CPF é Obrigátorio");
        }
    }
}
