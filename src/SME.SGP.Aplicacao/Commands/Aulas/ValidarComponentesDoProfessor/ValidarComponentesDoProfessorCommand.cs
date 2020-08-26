using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ValidarComponentesDoProfessorCommand: IRequest<(bool resultado, string mensagem)>
    {
        public ValidarComponentesDoProfessorCommand(Usuario usuario,
                                                    string turmaCodigo,
                                                    long componenteCurricularCodigo,
                                                    DateTime data)
        {
            Usuario = usuario;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            Data = data;
        }

        public Usuario Usuario { get; set; }
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public DateTime Data { get; set; }
    }

    public class ValidarComponentesDoProfessorCommandValidator: AbstractValidator<ValidarComponentesDoProfessorCommand>
    {
        public ValidarComponentesDoProfessorCommandValidator()
        {
            RuleFor(c => c.Usuario)
            .NotEmpty()
            .WithMessage("O usuário deve ser informado para validação dos componentes curriculares do professor");

            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("A turma deve ser informada para validação dos componentes curriculares do professor");

            RuleFor(c => c.ComponenteCurricularCodigo)
            .NotEmpty()
            .WithMessage("O componente curricular deve ser informado para validação do professor");

            RuleFor(c => c.Data)
            .NotEmpty()
            .WithMessage("A data deve ser informada para validação dos componentes curriculares do professor");
        }
    }
}
