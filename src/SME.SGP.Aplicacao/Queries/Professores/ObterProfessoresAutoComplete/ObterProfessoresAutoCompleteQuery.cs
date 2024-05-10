using FluentValidation;
using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresAutoCompleteQuery : IRequest<IEnumerable<ProfessorResumoDto>>
    {
        public ObterProfessoresAutoCompleteQuery(int anoLetivo, string dreId, string ueId, string nomeProfessor)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            NomeProfessor = nomeProfessor;
        }

        public int AnoLetivo { get; set; }
        public string DreId { get; set; }
        public string UeId { get; set; }
        public string NomeProfessor { get; set; }
    }
    public class ObterProfessoresAutoCompleteQueryValidator : AbstractValidator<ObterProfessoresAutoCompleteQuery>
    {
        public ObterProfessoresAutoCompleteQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado para obter professores.");
            
            RuleFor(a => a.DreId)
                .NotEmpty()
                .WithMessage("O código da Dre deve ser informado para obter professores.");
            
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado para obter professores.");
            
            RuleFor(a => a.NomeProfessor)
                .NotEmpty()
                .WithMessage("O nome do professor deve ser informado para obter professores.");
        }
    }
}
