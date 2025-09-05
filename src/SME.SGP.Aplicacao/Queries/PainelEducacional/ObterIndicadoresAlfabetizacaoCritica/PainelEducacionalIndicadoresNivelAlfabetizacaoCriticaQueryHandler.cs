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
        private readonly IRepositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica repositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica;

        public PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQueryHandler(IRepositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica repositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica)
        {
            this.repositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica = repositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica;
        }

        public async Task<IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>> Handle(PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica.ObterNumeroEstudantes(request.CodigoDre, request.CodigoUe);

            return MapearParaDto(registros);
        }

        private IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto> MapearParaDto(IEnumerable<PainelEducacionalIndicadoresNivelAlfabetizacaoCritica> registros)
        {
            var numeroAlunos = new List<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>();
            foreach (var item in registros)
            {
                numeroAlunos.Add(new PainelEducacionalIndicadorAlfabetizacaoCriticaDto()
                {
                    Posicao = item.Posicao,
                    Ue = item.Ue,
                    Dre = item.Dre,
                    TotalAlunosNaoAlfabetizados = item.TotalAlunosNaoAlfabetizados,
                    PercentualTotalAlunos = item.PercentualTotalAlunos
                });
            }

            return numeroAlunos;
        }
    }
}
