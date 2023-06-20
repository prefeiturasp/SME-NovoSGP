using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosSrmPaeeColaborativoEolQuery : IRequest<IEnumerable<SrmPaeeColaborativoSgpDto>>
    {
        public ObterDadosSrmPaeeColaborativoEolQuery(long codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public long CodigoAluno { get; set; }
    }

    public class ObterDadosSrmPaeeColaborativoEolQueryValidator : AbstractValidator<ObterDadosSrmPaeeColaborativoEolQuery>
    {
        public ObterDadosSrmPaeeColaborativoEolQueryValidator()
        {
            RuleFor(x => x.CodigoAluno).GreaterThan(0)
                .WithMessage("Informe o Código do Aluno para consultar as Informações do SRM no EOL ");
        }
    }
}