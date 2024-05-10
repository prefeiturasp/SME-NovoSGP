using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class UsuarioNotificadoAulaPrevistaDivergenteQuery : IRequest<bool>
    {
        public UsuarioNotificadoAulaPrevistaDivergenteQuery(long usuarioId, int bimestre, string turmaCodigo, string componenteCurricularId)
        {
            UsuarioId = usuarioId;
            Bimestre = bimestre;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long UsuarioId { get; }
        public int Bimestre { get; }
        public string TurmaCodigo { get; }
        public string ComponenteCurricularId { get; }
    }

    public class UsuarioNotificadoAulaPrevistaDivergenteQueryValidator : AbstractValidator<UsuarioNotificadoAulaPrevistaDivergenteQuery>
    {
        public UsuarioNotificadoAulaPrevistaDivergenteQueryValidator()
        {
            RuleFor(a => a.UsuarioId)
                .NotEmpty()
                .WithMessage("O identificador do usuário deve ser informado para consulta de notificação de divergência de aula prevista x dada");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para consulta de notificação de divergência de aula prevista x dada");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de notificação de divergência de aula prevista x dada");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para consulta de notificação de divergência de aula prevista x dada");
        }
    }
}
