using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoAcompanhamentoQuery : IRequest<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>>
    {
        public ObterTurmasFechamentoAcompanhamentoQuery(long dreId, long ueId, long[] turmasId, Modalidade modalidade, int semestre, int bimestre, int anoLetivo, bool listarTodasTurmas)
        {
            DreId = dreId;
            UeId = ueId;
            TurmasId = turmasId;
            Modalidade = modalidade;
            Semestre = semestre;
            Bimestre = bimestre;
            AnoLetivo = anoLetivo;
            ListarTodasTurmas = listarTodasTurmas;
        }

        public long DreId { get; set; }
        public long UeId { get; set; }
        public long[] TurmasId { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Bimestre { get; set; }
        public int AnoLetivo { get; set; }        
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
            RuleFor(a => a.TurmasId)
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
