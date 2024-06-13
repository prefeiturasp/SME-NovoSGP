using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarMapeamentoEstudanteCommand : IRequest<ResultadoMapeamentoEstudanteDto>
    {
        public long TurmaId { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }

        public RegistrarMapeamentoEstudanteCommand()
        {
        }

        public RegistrarMapeamentoEstudanteCommand(long turmaId, string alunoNome, string alunoCodigo, int bimestre)
        {
            TurmaId = turmaId;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
        }
    }
    public class RegistrarMapeamentoEstudanteCommandValidator : AbstractValidator<RegistrarMapeamentoEstudanteCommand>
    {
        public RegistrarMapeamentoEstudanteCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada para o mapeamento do estudante!");
            RuleFor(x => x.AlunoCodigo)
                   .NotEmpty()
                   .WithMessage("O código do aluno deve ser informado para o mapeamento do estudante!");
            RuleFor(x => x.Bimestre)
                   .GreaterThan(0)
                   .WithMessage("O bimestre deve ser informado para o mapeamento do estudante!");
        }
    }
}
