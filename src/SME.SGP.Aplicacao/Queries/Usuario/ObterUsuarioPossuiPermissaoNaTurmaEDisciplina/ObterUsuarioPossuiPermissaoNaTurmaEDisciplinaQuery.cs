using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery : IRequest<bool>
    {
        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(long componenteCurricularId,string codigoTurma, DateTime data, Dominio.Usuario usuario)
        {
            ComponenteCurricularId = componenteCurricularId;
            CodigoTurma = codigoTurma;
            Data = data;
            Usuario = usuario;
        }

        public long ComponenteCurricularId { get; set; }
        public string CodigoTurma { get; }
        public DateTime Data { get; set; }
        public Dominio.Usuario Usuario { get; set; }
    }


    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryValidator : AbstractValidator<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>
    {
        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryValidator()
        {

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado.");

            RuleFor(c => c.Data)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada.");

            RuleFor(c => c.Usuario)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }

}
