using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommand : IRequest<bool>
    {
        public long RegistroFrequenciaId { get; set; }
        public string AlunoCodigo { get; set; }
        public int NumeroAula { get; set; }

        public ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommand(long registroFrequenciaId, string alunoCodigo, int numeroAula)
        {
            RegistroFrequenciaId = registroFrequenciaId;
            AlunoCodigo = alunoCodigo;
            NumeroAula = numeroAula;
        }
    }

    public class ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaQueryValidator : AbstractValidator<ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaCommand>
    {
        public ExcluirFrequenciaPorRegistroFrequenciaIdAlunoCodigoNumeroAulaQueryValidator()
        {
            RuleFor(a => a.RegistroFrequenciaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do registro de frequencia para excluir a frequência do aluno na aula");
            RuleFor(a => a.AlunoCodigo)
               .NotEmpty()
               .WithMessage("É necessário informar o codigo do aluno para excluir a frequência dele na aula");
            RuleFor(a => a.NumeroAula)
              .NotEmpty()
              .WithMessage("É necessário informar o número da aula para excluir a frequência do aluno na aula");
        }
    }
}
