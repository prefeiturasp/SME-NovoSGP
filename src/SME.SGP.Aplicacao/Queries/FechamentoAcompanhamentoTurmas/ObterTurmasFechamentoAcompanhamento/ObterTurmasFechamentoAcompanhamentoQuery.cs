using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoAcompanhamentoQuery : IRequest<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>>
    {
        public ObterTurmasFechamentoAcompanhamentoQuery(long dreId, long ueId, long[] turmaId, Modalidade modalidade, int semestre, int bimestre, int anoLetivo)
        {            
            DreId = dreId;
            UeId = ueId;
            TurmaId = turmaId;
            Modalidade = modalidade;
            Semestre = semestre;
            Bimestre = bimestre;
            AnoLetivo = anoLetivo;            
        }
        
        public long DreId { get; set; }
        public long UeId { get; set; }
        public long[] TurmaId { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Bimestre { get; set; }
        public int AnoLetivo { get; set; }
    }
    public class ObterTurmasFechamentoAcompanhamentoQueryValidator : AbstractValidator<ObterTurmasFechamentoAcompanhamentoQuery>
    {
        public ObterTurmasFechamentoAcompanhamentoQueryValidator()
        {            
            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("O código da DRE deve ser informado.");
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("Pelo menos uma turma de ser informada.");
            RuleFor(a => a.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada.");
            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O semestre deve ser informado.");
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado.");
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
