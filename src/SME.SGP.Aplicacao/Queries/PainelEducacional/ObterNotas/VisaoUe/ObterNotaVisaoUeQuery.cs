using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe
{
    public class ObterNotaVisaoUeQuery : IRequest<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>
    {
        public ObterNotaVisaoUeQuery(Paginacao paginacao, string codigoDre, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            Paginacao = paginacao;
            CodigoDre = codigoDre;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            Modalidade = modalidade;
        }
        public Paginacao Paginacao { get; set; }
        public string CodigoDre { get; set; }
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public Modalidade Modalidade { get; set; }
    }

    public class ObterNotaVisaoUeQueryValidator : AbstractValidator<ObterNotaVisaoUeQuery>
    {
        public ObterNotaVisaoUeQueryValidator()
        {
            RuleFor(c => c.CodigoDre)
            .NotEmpty()
            .WithMessage("O código DRE deve ser informado.");

            RuleFor(c => c.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Bimestre)
           .NotEmpty()
           .WithMessage("O bimestre deve ser informado.");

            RuleFor(c => c.Modalidade)
           .NotEmpty()
           .WithMessage("A modalidade deve ser informada.");
        }
    }
}
