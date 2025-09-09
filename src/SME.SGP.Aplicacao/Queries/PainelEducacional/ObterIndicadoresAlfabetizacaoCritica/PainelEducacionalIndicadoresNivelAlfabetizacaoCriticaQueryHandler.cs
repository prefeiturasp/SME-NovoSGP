using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresAlfabetizacaoCritica
{
    public class PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQueryHandler : IRequestHandler<PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery, IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>>
    {
        private readonly IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita repositorioConsolidacaoAlfabetizacaoCriticaEscrita;

        public PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQueryHandler(IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita repositorioConsolidacaoAlfabetizacaoCriticaEscrita)
        {
            this.repositorioConsolidacaoAlfabetizacaoCriticaEscrita = repositorioConsolidacaoAlfabetizacaoCriticaEscrita;
        }

        public async Task<IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>> Handle(PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioConsolidacaoAlfabetizacaoCriticaEscrita.ObterNumeroEstudantes(request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto> MapearParaDto(IEnumerable<ConsolidacaoAlfabetizacaoCriticaEscrita> registros)
        {
            var numeroAlunos = new List<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>();
            foreach (var item in registros)
            {
                numeroAlunos.Add(new PainelEducacionalIndicadorAlfabetizacaoCriticaDto()
                {
                    Posicao = item.Posicao.ToString().PadLeft(2, '0'),
                    Ue = item.UeNome,
                    CodigoUe = item.UeCodigo,
                    Dre = item.DreNome,
                    CodigoDre = item.DreCodigo,
                    TotalAlunosNaoAlfabetizados = item.TotalAlunosNaoAlfabetizados,
                    PercentualTotalAlunos = item.PercentualTotalAlunos
                });
            }

            return numeroAlunos;
        }
    }
}
