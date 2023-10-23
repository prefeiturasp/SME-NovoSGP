using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessorPorRFUeDreAnoLetivoQuery : IRequest<ProfessorResumoDto>
    {
        public ObterProfessorPorRFUeDreAnoLetivoQuery(string codigoRF, int anoLetivo, string dreId, string ueId, bool buscarOutrosCargos, bool buscarPorTodasDre)
        {
            CodigoRF = codigoRF;
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            BuscarOutrosCargos = buscarOutrosCargos;
            BuscarPorTodasDre = buscarPorTodasDre;
        }

        public string CodigoRF { get; set; }
        public int AnoLetivo { get; set; }
        public string DreId { get; set; }
        public string UeId { get; set; }
        public bool BuscarOutrosCargos { get; set; }
        public bool BuscarPorTodasDre { get; set; }
    }
    public class ObterProfessorPorRFUeDreAnoLetivoQueryValidator : AbstractValidator<ObterProfessorPorRFUeDreAnoLetivoQuery>
    {
        public ObterProfessorPorRFUeDreAnoLetivoQueryValidator()
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
            
            RuleFor(a => a.CodigoRF)
                .NotEmpty()
                .WithMessage("O Rf deve ser informado para obter professores.");
        }
    }
}
