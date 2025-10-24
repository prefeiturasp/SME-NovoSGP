using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe
{
    public class ObterNotaVisaoUeQuery : IRequest<PaginacaoNotaResultadoDto<PainelEducacionalNotasVisaoUeRetornoSelectDto>>
    {
        public ObterNotaVisaoUeQuery(Paginacao paginacao, string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            Paginacao = paginacao;
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            Modalidade = modalidade;
        }
        public Paginacao Paginacao { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public Modalidade Modalidade { get; set; }
    }

    public class ObterNotaVisaoUeQueryValidator : AbstractValidator<ObterNotaVisaoUeQuery>
    {
        public ObterNotaVisaoUeQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código UE deve ser informado.");

            RuleFor(c => c.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Bimestre)
           .NotEmpty()
           .WithMessage("O bimestre deve ser informado.");
        }
    }
}
