using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioPorAnoLetivoEModalidadeQuery : IRequest<TipoCalendario>
    {
        public int AnoLetivo { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public int Semestre { get; set; }

        public ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, int semestre)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidadeTipoCalendario;
            Semestre = semestre;
        }

    }

    public class ObterTipoCalendarioPorAnoLetivoEModalidadeQueryValidator : AbstractValidator<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>
    {
        public ObterTipoCalendarioPorAnoLetivoEModalidadeQueryValidator()
        {
            RuleFor(a => a.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada para consulta do Tipo de Calendário");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta do TIpo de Calendário");
        }
    }

}
