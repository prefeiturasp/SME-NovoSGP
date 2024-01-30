using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.Aluno;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosResponsaveisAlunoEolQuery : IRequest<IEnumerable<DadosResponsavelAlunoDto>>
    {
        public ObterDadosResponsaveisAlunoEolQuery(string codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; set; }
    }

    public class ObterDadosResponsaveisAlunoQueryValidator : AbstractValidator<ObterDadosResponsaveisAlunoEolQuery>
    {
        public ObterDadosResponsaveisAlunoQueryValidator()
        {

            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para obter dados dos responsáveis aluno eol.");
        }
    }
}
