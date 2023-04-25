using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaParaCompensacaoQuery : IRequest<IEnumerable<RegistroFaltasNaoCompensadaDto>>
    {
        public ObterAusenciaParaCompensacaoQuery(long compensacaoId, string turmaCodigo, string disciplinaId, int bimestre, IEnumerable<AlunoQuantidadeCompensacaoDto> alunosQuantidadeCompensacoes)
        {
            CompensacaoId = compensacaoId;
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
            AlunosQuantidadeCompensacoes = alunosQuantidadeCompensacoes;
            Bimestre = bimestre;
        }

        public long CompensacaoId { get; }
        public string TurmaCodigo { get; }
        public string DisciplinaId { get; }
        public int Bimestre { get; }
        public IEnumerable<AlunoQuantidadeCompensacaoDto> AlunosQuantidadeCompensacoes { get; }
    }

    public class ObterAusenciaParaCompensacaoQueryValidator : AbstractValidator<ObterAusenciaParaCompensacaoQuery>
    {
        public ObterAusenciaParaCompensacaoQueryValidator()
        {
            RuleFor(x => x.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser preenchido para obter ausência para compensação.");

            RuleFor(x => x.DisciplinaId)
                .NotEmpty()
                .WithMessage("A disciplina id deve ser preenchido para obter ausência para compensação.");

            RuleFor(x => x.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser preenchido para obter ausência para compensação.");

            RuleFor(x => x.AlunosQuantidadeCompensacoes)
                .NotEmpty()
                .WithMessage("Os alunos e quantidade de compensações deve ser preenchido para obter ausência para compensação.");
        }
    }
}
