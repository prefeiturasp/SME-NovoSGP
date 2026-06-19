using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroDeFrequenciaAlunoPorIdRegistroQuery : IRequest<IEnumerable<RegistroFrequenciaAluno>>
    {
        public long RegistroFrequenciaId { get; set; }

        public ObterRegistroDeFrequenciaAlunoPorIdRegistroQuery(long registroFrequenciaId)
        {
            RegistroFrequenciaId = registroFrequenciaId;
        }
    }

    public class ObterRegistroDeFrequenciaAlunoPorIdRegistroQueryValidator : AbstractValidator<ObterRegistroDeFrequenciaAlunoPorIdRegistroQuery>
    {
        public ObterRegistroDeFrequenciaAlunoPorIdRegistroQueryValidator()
        {
            RuleFor(a => a.RegistroFrequenciaId)
                .NotEmpty()
                .WithMessage("O id do registro de frequência deve ser informado para retorno de seus registros.");
        }
    }
}
