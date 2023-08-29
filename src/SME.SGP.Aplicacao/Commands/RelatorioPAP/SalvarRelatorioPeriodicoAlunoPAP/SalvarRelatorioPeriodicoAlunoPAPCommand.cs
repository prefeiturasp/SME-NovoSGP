using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoAlunoPAPCommand : IRequest<RelatorioPeriodicoPAPAluno>
    {
        public SalvarRelatorioPeriodicoAlunoPAPCommand(string alunoCodigo, string alunoNome, long relatorioPeriodicoTurmaPAPId)
        {
            AlunoCodigo = alunoCodigo;
            AlunoNome = alunoNome;
            RelatorioPeriodicoTurmaPAPId = relatorioPeriodicoTurmaPAPId;
        }

        public string AlunoCodigo { get; set; }
        public string AlunoNome {  get; set; }
        public long RelatorioPeriodicoTurmaPAPId { get; set; }
    }

    public class SalvarRelatorioPeriodicoAlunoPAPCommandValidator : AbstractValidator<SalvarRelatorioPeriodicoAlunoPAPCommand>
    {
        public SalvarRelatorioPeriodicoAlunoPAPCommandValidator()
        {
            RuleFor(x => x.AlunoCodigo)
                   .NotEmpty()
                   .WithMessage("O código do aluno deve ser informado para registar relatório periodico aluno pap!");
            RuleFor(x => x.AlunoNome)
                   .NotEmpty()
                   .WithMessage("O nome do aluno deve ser informado para registar relatório periodico aluno pap!");
            RuleFor(x => x.RelatorioPeriodicoTurmaPAPId)
                   .GreaterThan(0)
                   .WithMessage("O Id do relatório periodico turma pap deve ser informada para registar relatório periodico aluno pap!");
        }
    }
}
