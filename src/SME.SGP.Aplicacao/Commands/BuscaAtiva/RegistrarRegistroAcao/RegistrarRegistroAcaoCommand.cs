using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarRegistroAcaoCommand : IRequest<ResultadoRegistroAcaoBuscaAtivaDto>
    {
        public long TurmaId { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }

        public RegistrarRegistroAcaoCommand()
        {
        }

        public RegistrarRegistroAcaoCommand(long turmaId, string alunoNome, string alunoCodigo)
        {
            TurmaId = turmaId;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
        }
    }
    public class RegistrarRegistroAcaoCommandValidator : AbstractValidator<RegistrarRegistroAcaoCommand>
    {
        public RegistrarRegistroAcaoCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada para o registro de ação!");
        }
    }
}
