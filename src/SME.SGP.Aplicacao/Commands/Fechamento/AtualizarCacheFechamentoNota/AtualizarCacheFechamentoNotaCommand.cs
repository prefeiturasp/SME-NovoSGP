using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AtualizarCacheFechamentoNotaCommand : IRequest<bool>
    {
        public AtualizarCacheFechamentoNotaCommand(
                            FechamentoNota fechamentoNota, 
                            string codigoAluno, 
                            string codigoTurma,
                            long disciplinaId,
                            bool emAprovacao = false,
                            ConselhoClasseAlunosNotaPorFechamentoIdDto conselhosClasseAlunos = null)
        {
            FechamentoNota = fechamentoNota;
            CodigoAluno = codigoAluno;
            CodigoTurma = codigoTurma;
            DisciplinaId = disciplinaId;
            EmAprovacao = emAprovacao;
            ConselhosClasseAlunos = conselhosClasseAlunos;
        }

        public FechamentoNota FechamentoNota { get; set; }
        public string CodigoAluno { get; set; }
        public string CodigoTurma { get; set; }
        public long DisciplinaId { get; set; }
        public bool EmAprovacao { get; set; }
        public ConselhoClasseAlunosNotaPorFechamentoIdDto ConselhosClasseAlunos { get; set; }
    }

    public class AtualizarCacheFechamentoNotaCommandValidator : AbstractValidator<AtualizarCacheFechamentoNotaCommand>
    {
        public AtualizarCacheFechamentoNotaCommandValidator()
        {
            RuleFor(a => a.FechamentoNota)
                .NotEmpty()
                .WithMessage("O fechamento nota deve ser informado para atualização do cache");

            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para atualização do cache");

            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para atualização do cache");
        }
    }
}
