using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQueryHandler : IRequestHandler<PainelEducacionalAprovacaoUeQuery, IEnumerable<PainelEducacionalAprovacaoUeDto>>
    {
        private readonly IRepositorioPainelEducacionalAprovacaoUe repositorioPainelEducacionalAprovacaoUe;

        public PainelEducacionalAprovacaoUeQueryHandler(IRepositorioPainelEducacionalAprovacaoUe repositorioPainelEducacionalAprovacaoUe)
        {
            this.repositorioPainelEducacionalAprovacaoUe = repositorioPainelEducacionalAprovacaoUe;
        }

        public async Task<IEnumerable<PainelEducacionalAprovacaoUeDto>> Handle(PainelEducacionalAprovacaoUeQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalAprovacaoUe.ObterAprovacao(request.AnoLetivo, request.CodigoUe);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalAprovacaoUeDto> MapearParaDto(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> registros)
        {
            var lista = new List<PainelEducacionalAprovacaoUeDto>();

            foreach (var item in registros)
            {
                lista.Add(new PainelEducacionalAprovacaoUeDto
                {
                    CodigoDre = item.CodigoDre,
                    Turma = item.Turma,
                    Modalidade = item.Modalidade,
                    TotalPromocoes = item.TotalPromocoes,
                    TotalRetencoesAusencias = item.TotalRetencoesAusencias,
                    TotalRetencoesNotas = item.TotalRetencoesNotas,
                    AnoLetivo = item.AnoLetivo,
                });
            }

            return lista;
        }
    }
}
