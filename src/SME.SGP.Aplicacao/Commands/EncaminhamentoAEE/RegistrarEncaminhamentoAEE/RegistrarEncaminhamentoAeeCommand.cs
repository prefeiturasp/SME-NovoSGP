using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoAeeCommand : IRequest<ResultadoEncaminhamentoAEEDto>
    {
        public long TurmaId { get; set; }
        public SituacaoAEE Situacao { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }

        public RegistrarEncaminhamentoAeeCommand()
        {
        }

        public RegistrarEncaminhamentoAeeCommand(long turmaId, string alunoNome, string alunoCodigo, SituacaoAEE situacao)
        {
            TurmaId = turmaId;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
            Situacao = situacao;
        }
    }
    public class RegistrarEncaminhamentoAeeCommandValidator : AbstractValidator<RegistrarEncaminhamentoAeeCommand>
    {
        public RegistrarEncaminhamentoAeeCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada!");
        }
    }
}
