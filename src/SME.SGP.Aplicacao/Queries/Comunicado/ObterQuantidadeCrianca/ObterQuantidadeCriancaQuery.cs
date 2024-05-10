using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeCriancaQuery : IRequest<QuantidadeCriancaDto>
    {
        public ObterQuantidadeCriancaQuery(int anoLetivo, string[] turma, string dreId, string ueId, int[] modalidade, string[] anoTurma)
        {
            AnoTurma = anoTurma;
            Turma = turma;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
        public string[] AnoTurma { get; set; }
        public string[] Turma { get; set; }
        public string DreId { get; set; }
        public string UeId { get; set; }
        public int[] Modalidade { get; set; }
    }
    public class ObterQuantidadeCriancaQueryValidator : AbstractValidator<ObterQuantidadeCriancaQuery>
    {
        public ObterQuantidadeCriancaQueryValidator()
        {
            RuleFor(a => a.AnoTurma)
               .NotEmpty()
               .WithMessage("O ano deve ser informado");
            RuleFor(a => a.Turma)
               .NotEmpty()
               .WithMessage("A turma deve ser informada");
            RuleFor(a => a.DreId)
               .NotEmpty()
               .WithMessage("A dre deve ser informada");
            RuleFor(a => a.UeId)
               .NotEmpty()
               .WithMessage("A ue deve ser informada");
            RuleFor(a => a.Modalidade)
               .NotEmpty()
               .WithMessage("A modalidade deve ser informada");
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("O ano letivo deve ser informado");
        }
    }
}
