using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery: IRequest<bool>
    {
        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery(long componenteCurricularId, string codigoTurma, string usuarioRf, DateTime dataInicio, DateTime dataFim)
        {
            ComponenteCurricularId = componenteCurricularId;
            CodigoTurma = codigoTurma;
            UsuarioRf = usuarioRf;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public long ComponenteCurricularId { get; set; }
        public string CodigoTurma { get; }
        public string UsuarioRf { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQueryValidator : AbstractValidator<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery>
    {
        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQueryValidator()
        {

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado.");

            RuleFor(c => c.DataInicio)
                .NotEmpty()
                .WithMessage("A data de inicio deve ser informada.");

            RuleFor(c => c.DataFim)
                .NotEmpty()
                .WithMessage("A data de fim deve ser informada.");

            RuleFor(c => c.UsuarioRf)
                .NotEmpty()
                .WithMessage("O usuário deve ser informado.");

            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }

}
