using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoAcompanhamentoQuery : IRequest<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>>
    {
        public ObterTurmasFechamentoAcompanhamentoQuery(FiltroAcompanhamentoFechamentoTurmasDto filtro, int? situacaoFechamento, int? situacaoConselhoClasse, bool listarTodasTurmas)
        {
            DreId = filtro.DreId;
            UeId = filtro.UeId;
            TurmasCodigo = filtro.TurmasId;
            Modalidade = filtro.Modalidade;
            Semestre = filtro.Semestre;
            Bimestre = filtro.Bimestre;
            AnoLetivo = filtro.AnoLetivo;
            SituacaoFechamento = situacaoFechamento;
            SituacaoConselhoClasse = situacaoConselhoClasse;
            ListarTodasTurmas = listarTodasTurmas;
        }

        public long DreId { get; set; }
        public long UeId { get; set; }
        public string[] TurmasCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Bimestre { get; set; }
        public int AnoLetivo { get; set; }
        public int? SituacaoFechamento { get; set; }
        public int? SituacaoConselhoClasse { get; set; }
        public bool ListarTodasTurmas { get; set; }
    }
    public class ObterTurmasFechamentoAcompanhamentoQueryValidator : AbstractValidator<ObterTurmasFechamentoAcompanhamentoQuery>
    {
        public ObterTurmasFechamentoAcompanhamentoQueryValidator()
        {
            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("O id da DRE deve ser informado.");
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O id da Ue deve ser informado.");
            RuleFor(a => a.TurmasCodigo)
                .NotEmpty()
                .WithMessage("Pelo menos uma turma deve ser informada.");
            RuleFor(a => a.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada.");
            RuleFor(a => a.Semestre)
                .NotNull()
                .WithMessage("O semestre deve ser informado.");
            RuleFor(a => a.Bimestre)
                .NotNull()
                .WithMessage("O bimestre deve ser informado.");
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
