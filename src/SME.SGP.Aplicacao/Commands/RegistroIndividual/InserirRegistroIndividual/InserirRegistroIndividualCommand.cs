using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroIndividualCommand : IRequest<RegistroIndividual>
    {
        public InserirRegistroIndividualCommand(long turmaId, long alunoCodigo, long componenteCurricularId, DateTime dataRegistro, string registro)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            ComponenteCurricularId = componenteCurricularId;
            DataRegistro = dataRegistro;
            Registro = registro;
        }

        public long TurmaId { get; set; }
        public long AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public DateTime DataRegistro { get; set; }
        public string Registro { get; set; }
    }

    public class InserirRegistroIndividualCommandValidator : AbstractValidator<InserirRegistroIndividualCommand>
    {
        public InserirRegistroIndividualCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                   .NotEmpty()
                   .WithMessage("A aula deve ser informada!");

            RuleFor(a => a.AlunoCodigo)
                   .NotEmpty()
                   .WithMessage("O código do aluno deve ser informado!");

            RuleFor(a => a.ComponenteCurricularId)
                   .NotEmpty()
                   .WithMessage("O componente curricular deve ser informado!");

            RuleFor(a => a.DataRegistro)
                   .NotEmpty()
                   .WithMessage("A data do registro deve ser informada!");

            RuleFor(a => a.Registro)
                   .NotEmpty().WithMessage("O registro é obrigatório para o registro individual!");
        }
    }
}
