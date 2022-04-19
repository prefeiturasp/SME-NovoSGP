using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaIdPorComponenteProfessorBimestreQuery : IRequest<long>
    {
        public string ComponenteCurricularId { get; set; }
        public string CodigoRf { get; set; }
        public long PeriodoEscolarId { get; set; }

        public ObterPendenciaIdPorComponenteProfessorBimestreQuery(string componenteId, string codigoRf, long periodoEscolarId)
        {
            ComponenteCurricularId = componenteId;
            CodigoRf = codigoRf;
            PeriodoEscolarId = periodoEscolarId;
        }
    }

    public class ObterPendenciaIdPorComponenteProfessorBimestreQueryValidator : AbstractValidator<ObterPendenciaIdPorComponenteProfessorBimestreQuery>
    {
        public ObterPendenciaIdPorComponenteProfessorBimestreQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do componente curricular para verificar se já existe pendência.");

            RuleFor(a => a.CodigoRf)
               .NotEmpty()
               .WithMessage("É necessário informar o RF para verificar se já existe pendência.");

            RuleFor(a => a.CodigoRf)
               .NotEmpty()
               .WithMessage("É necessário informar o id do período escolar para verificar se já existe pendência.");
        }
    }
}
