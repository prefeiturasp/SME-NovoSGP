using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommand : IRequest<bool>
    {
        public List<RegistroFrequenciaAulaParcialDto> ListaDeRegistroAula { get; set; }
        public ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommand(List<RegistroFrequenciaAulaParcialDto> listaDeRegistroAula)
        {
            ListaDeRegistroAula = listaDeRegistroAula;
        }
    }

    public class ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommandValidator : AbstractValidator<ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommand>
    {
        public ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommandValidator()
        {
            RuleFor(x => x.ListaDeRegistroAula)
                .NotEmpty()
                .NotNull()
                .WithMessage("A lista de registro de aula precisa ser informada para processar a carga da referência da aula no Registro de Frequência do Aluno");

        }
    }
}
