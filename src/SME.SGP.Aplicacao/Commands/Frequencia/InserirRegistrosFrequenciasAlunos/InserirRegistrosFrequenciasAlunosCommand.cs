using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistrosFrequenciasAlunosCommand : IRequest<bool>
    {
        public InserirRegistrosFrequenciasAlunosCommand(IList<RegistroFrequenciaAlunoDto> frequencias, long registroFrequenciaId)
        {
            Frequencias = frequencias;
            RegistroFrequenciaId = registroFrequenciaId;
        }

        public IList<RegistroFrequenciaAlunoDto> Frequencias { get; set; }
        public long RegistroFrequenciaId { get; set; }
    }

    public class InserirRegistrosFrequenciasAlunosCommandValidator : AbstractValidator<InserirRegistrosFrequenciasAlunosCommand>
    {
        public InserirRegistrosFrequenciasAlunosCommandValidator()
        {
            RuleFor(x => x.Frequencias)
               .NotEmpty()
               .WithMessage("As frequencias precisam ser informadas");
            RuleFor(x => x.RegistroFrequenciaId)
              .NotEmpty()
              .WithMessage("O registro de frequência precisa ser informado");
        }
    }
}
