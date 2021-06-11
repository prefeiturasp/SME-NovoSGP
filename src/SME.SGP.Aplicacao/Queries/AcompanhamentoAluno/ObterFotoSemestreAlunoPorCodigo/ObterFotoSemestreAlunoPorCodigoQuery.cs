using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoSemestreAlunoPorCodigoQuery : IRequest<AcompanhamentoAlunoFoto>
    {
        public ObterFotoSemestreAlunoPorCodigoQuery(Guid codigoFoto)
        {
            CodigoFoto = codigoFoto;
        }

        public Guid CodigoFoto { get; }
    }

    public class ObterFotoSemestreAlunoPorCodigoQueryValidator : AbstractValidator<ObterFotoSemestreAlunoPorCodigoQuery>
    {
        public ObterFotoSemestreAlunoPorCodigoQueryValidator()
        {
            RuleFor(a => a.CodigoFoto)
                .NotEmpty()
                .WithMessage("O código da foto é nescessário para obter a foto do aluno");
        }
    }
}
