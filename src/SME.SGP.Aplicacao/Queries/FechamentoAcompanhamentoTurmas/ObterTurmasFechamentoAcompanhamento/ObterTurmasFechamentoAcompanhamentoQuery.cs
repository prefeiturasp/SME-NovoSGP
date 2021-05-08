using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoAcompanhamentoQuery : IRequest<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>>
    {
        public ObterTurmasFechamentoAcompanhamentoQuery(string dreCodigo, string ueCodigo, long[] turmaId, Modalidade modalidade, int semestre, int bimestre, int anoLetivo)
        {
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            TurmaId = turmaId;
            Modalidade = modalidade;
            Semestre = semestre;
            Bimestre = bimestre;
            AnoLetivo = anoLetivo;
        }

        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
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
            RuleFor(a => a.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da DRE deve ser informado.");
            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("Pelo menos uma turma deve ser informada.");
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
