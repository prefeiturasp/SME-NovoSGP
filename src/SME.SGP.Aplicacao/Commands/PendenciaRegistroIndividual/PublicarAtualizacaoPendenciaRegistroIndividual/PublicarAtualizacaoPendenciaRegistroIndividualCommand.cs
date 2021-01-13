using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class PublicarAtualizacaoPendenciaRegistroIndividualCommand : IRequest
    {
        public long TurmaId { get; set; }
        public long CodigoAluno { get; set; }
        public DateTime DataRegistro { get; set; }

        public PublicarAtualizacaoPendenciaRegistroIndividualCommand(long turmaId, long codigoAluno, DateTime dataRegistro)
        {
            TurmaId = turmaId;
            CodigoAluno = codigoAluno;
            DataRegistro = dataRegistro;
        }
    }

    public class PublicarAtualizacaoPendenciaRegistroIndividualCommandValidator : AbstractValidator<PublicarAtualizacaoPendenciaRegistroIndividualCommand>
    {
        public PublicarAtualizacaoPendenciaRegistroIndividualCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para atualização de pendências por ausência de registro individual");

            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para atualização de pendências por ausência de registro individual");

            RuleFor(x => x.DataRegistro)
                .NotEmpty()
                .WithMessage("A data do registro individual do aluno deve ser informado para atualização de pendências por ausência de registro individual");
        }
    }
}