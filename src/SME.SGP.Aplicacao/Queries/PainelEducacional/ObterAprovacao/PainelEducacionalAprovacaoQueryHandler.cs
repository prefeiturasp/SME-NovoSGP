using FluentValidation;
using MediatR;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacao
{
    public class PainelEducacionalAprovacaoQueryHandler : IRequestHandler<PainelEducacionalAprovacaoQuery, IEnumerable<PainelEducacionalAprovacaoDto>>
    {
        private readonly IRepositorioPainelEducacionalAprovacao repositorioPainelEducacionalAprovacao;

        public PainelEducacionalAprovacaoQueryHandler(IRepositorioPainelEducacionalAprovacao repositorioPainelEducacionalAprovacao)
        {
            this.repositorioPainelEducacionalAprovacao = repositorioPainelEducacionalAprovacao;
        }

        public async Task<IEnumerable<PainelEducacionalAprovacaoDto>> Handle(PainelEducacionalAprovacaoQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalAprovacao.ObterAprovacao(request.AnoLetivo, request.CodigoDre);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalAprovacaoDto> MapearParaDto(IEnumerable<PainelEducacionalConsolidacaoAprovacao> registros)
        {
            var lista = new List<PainelEducacionalAprovacaoDto>();

            foreach (var item in registros)
            {
                lista.Add(new PainelEducacionalAprovacaoDto
                {
                    CodigoDre = item.CodigoDre,
                    SerieAno = item.SerieAno,
                    Modalidade = item.Modalidade,
                    TotalPromocoes = item.TotalPromocoes,
                    TotalRetencoesAusencias = item.TotalRetencoesAusencias,
                    TotalRetencoesNotas = item.TotalRetencoesNotas,
                    AnoLetivo = item.AnoLetivo,
                });
            }

            return lista;
        }
        public class PainelEducacionalAprovacaoUeQueryValidator : AbstractValidator<PainelEducacionalAprovacaoUeQuery>
        {
            public PainelEducacionalAprovacaoUeQueryValidator()
            {
                RuleFor(x => x.AnoLetivo)
                    .NotEmpty().WithMessage("Informe o ano letivo");

                RuleFor(x => x.CodigoUe)
                    .NotEmpty().WithMessage("Informe o código da Ue");

                RuleFor(x => x.ModalidadeId)
                    .NotEmpty().WithMessage("Informe o código modalidade");
            }
        }
    }
}
