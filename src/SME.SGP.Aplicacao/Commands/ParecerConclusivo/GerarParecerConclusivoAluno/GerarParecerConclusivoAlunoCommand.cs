using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class GerarParecerConclusivoAlunoCommand : IRequest<ParecerConclusivoDto>
    {
        public GerarParecerConclusivoAlunoCommand(Dominio.ConselhoClasseAluno conselhoClasseAluno, long usuarioSolicitanteId)
        {
            ConselhoClasseAluno = conselhoClasseAluno;
            UsuarioSolicitanteId = usuarioSolicitanteId;
        }

        public ConselhoClasseAluno ConselhoClasseAluno { get; }
        public long UsuarioSolicitanteId { get; }
    }

    public class GerarParecerConclusivoAlunoCommandValidator : AbstractValidator<GerarParecerConclusivoAlunoCommand>
    {
        public GerarParecerConclusivoAlunoCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseAluno)
                .NotEmpty()
                .WithMessage("O registro do conselho de classe do aluno deve ser informado para gerar seu parecer conclusivo");

            RuleFor(a => a.UsuarioSolicitanteId)
                .NotEmpty()
                .WithMessage("O identificador do usuário solicitante do parecer conclusivo do conselho de classe do aluno deve ser informado");
        }
    }
}
