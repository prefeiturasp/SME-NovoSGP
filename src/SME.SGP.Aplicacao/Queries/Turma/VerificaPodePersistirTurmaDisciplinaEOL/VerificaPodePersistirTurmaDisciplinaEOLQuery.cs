using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificaPodePersistirTurmaDisciplinaEOLQuery : IRequest<bool>
    {
        public VerificaPodePersistirTurmaDisciplinaEOLQuery(Usuario usuario, string turmaId, string componenteCurricularId, DateTime data)
        {
            Usuario = usuario;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            Data = data;
        }

        public Usuario Usuario { get; set; }
        public string TurmaId { get; set; }
        public string ComponenteCurricularId { get; set; }
        public DateTime Data { get; set; }
    }

    public class VerificaPodePersistirTurmaDisciplinaEOLQueryValidator : AbstractValidator<VerificaPodePersistirTurmaDisciplinaEOLQuery>
    {
        public VerificaPodePersistirTurmaDisciplinaEOLQueryValidator()
        {
            RuleFor(a => a.Usuario)
               .NotEmpty()
               .WithMessage("O usuário deve ser informado");

            RuleFor(a => a.TurmaId)
               .NotEmpty()
               .WithMessage("A turma deve ser informada");

            RuleFor(a => a.ComponenteCurricularId)
              .NotEmpty()
              .WithMessage("O componente curricular deve ser informado");


        }
    }
}
