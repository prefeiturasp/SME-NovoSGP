using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery : IRequest<IEnumerable<AbrangenciaTurmaRetorno>>
    {
        public ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
        }

        public string CodigoUe { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Periodo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQueryValidiator : AbstractValidator<ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQuery>
    {
        public ObterTurmasRegularesPorUeModalidadePeriodoAnoLetivoQueryValidiator()
        {
            RuleFor(x => x.CodigoUe).NotEmpty()
                .WithMessage("O Código da UE deve ser Informado para Consultar as Turmas Regulares");

            RuleFor(x => x.Modalidade).NotNull().WithMessage("A Modalidade deve ser Informada para Consultar as Turmas Regulares");
            
        }
    }
}