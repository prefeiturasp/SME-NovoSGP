using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosPorCodigoUeModalidadeQuery : IRequest<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>>
    {
        public ObterAnosPorCodigoUeModalidadeQuery(string codigoUe, int[] modalidades)
        {
            CodigoUe = codigoUe;
            Modalidades = modalidades;
        }

        public string CodigoUe { get; set; }
        public int[] Modalidades { get; set; }
    }
    public class ObterAnosPorCodigoUeModalidadeQueryValidator : AbstractValidator<ObterAnosPorCodigoUeModalidadeQuery>
    {
        public ObterAnosPorCodigoUeModalidadeQueryValidator()
        {
            RuleFor(c => c.Modalidades)
                .NotEmpty()
                .WithMessage("É necessário informar pelo menos uma modalidade");
        }
    }
}
