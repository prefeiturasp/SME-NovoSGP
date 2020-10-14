using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery : IRequest<PlanejamentoAnual>
    {
        public ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery(int ano, string escolaId, long turmaId, int bimestre, long disciplinaId)
        {
            Ano = ano;
            EscolaId = escolaId;
            TurmaId = turmaId;
            Bimestre = bimestre;
            DisciplinaId = disciplinaId;
        }

        public long TurmaId { get; set; }
        public int Ano { get; set; }
        public string EscolaId { get; set; }
        public int Bimestre { get; set; }
        public long DisciplinaId { get; set; }
    }

    public class ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryValidator : AbstractValidator<ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery>
    {
        public ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryValidator()
        {
            RuleFor(c => c.Ano)
                .NotEmpty()
                .WithMessage("O Ano deve ser informado.");

            RuleFor(c => c.EscolaId)
                .NotEmpty()
                .WithMessage("O ID da escola deve ser informado.");

            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(c => c.Bimestre)
                .NotEmpty()
                .WithMessage("O Bimestre deve ser informado.");

            RuleFor(c => c.DisciplinaId)
                .NotEmpty()
                .WithMessage("O Id da Disciplina deve ser informado.");
        }
    }
}
