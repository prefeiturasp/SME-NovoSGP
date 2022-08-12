using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDeRegistroFrequenciaAulaPorTurmaQuery : IRequest<IEnumerable<RegistroFrequenciaAulaParcialDto>>
    {
        public string CodigoTurma { get; set; }

        public ObterListaDeRegistroFrequenciaAulaPorTurmaQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }
    }

    public class ObterListaDeRegistroFrequenciaAulaPorTurmaQueryValidator : AbstractValidator<ObterListaDeRegistroFrequenciaAulaPorTurmaQuery>
    {
        public ObterListaDeRegistroFrequenciaAulaPorTurmaQueryValidator()
        {
            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("É preciso informar o código da turma para consultar os registros de frequência e aula.");
        }
    }
}
