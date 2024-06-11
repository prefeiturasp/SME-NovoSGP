using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQuery : IRequest<bool>
    {
        public UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQuery(long turmaId, long usuarioId)
        {
            this.TurmaId = turmaId;
            this.UsuarioId = usuarioId;
        }
        public long TurmaId { get; set; }
        public long UsuarioId { get; set; }
    }

    public class UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQueryValidator : AbstractValidator<UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQuery>
    {
        public UsuarioPossuiSomenteAbrangenciaHistoricaNaTurmaQueryValidator()
        {
            RuleFor(x => x.TurmaId)
                .GreaterThan(0)
                .WithMessage("Informe o id da turma");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0)
                .WithMessage("Informe o id do usuário");
        }
    }
}
