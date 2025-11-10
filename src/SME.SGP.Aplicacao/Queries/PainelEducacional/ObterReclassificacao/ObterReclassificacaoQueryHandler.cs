using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Dtos;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterReclassificacao
{
    public class ObterReclassificacaoQueryHandler : IRequestHandler<ObterReclassificacaoQuery, IEnumerable<PainelEducacionalReclassificacaoDto>>
    {
        private readonly IRepositorioReclassificacaoConsulta repositorio;

        public ObterReclassificacaoQueryHandler(
            IRepositorioReclassificacaoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalReclassificacaoDto>> Handle(ObterReclassificacaoQuery request, CancellationToken cancellationToken)
        {
            var dadosConsolidados = await repositorio.ObterReclassificacao(request.CodigoDre, request.CodigoUe, request.AnoLetivo, request.AnoTurma);
            
            return MapearParaDto(dadosConsolidados);
        }

        private IEnumerable<PainelEducacionalReclassificacaoDto> MapearParaDto(IEnumerable<ReclassificacaoRawDto> dadosRaw)
        {
            if (dadosRaw == null || !dadosRaw.Any())
                return Enumerable.Empty<PainelEducacionalReclassificacaoDto>();

            return dadosRaw
                .GroupBy(x => x.CodigoModalidade)
                .Select(modalidadeGrupo => new PainelEducacionalReclassificacaoDto
                {
                    Modalidade = ObterNomeModalidade(modalidadeGrupo.Key, modalidadeGrupo.First().Nome),
                    SerieAno = modalidadeGrupo
                        .GroupBy(x => x.AnoTurma)
                        .Select(anoGrupo => new SerieAnoReclassificacaoDto
                        {
                            AnoTurma = anoGrupo.Key,
                            QuantidadeAlunos = anoGrupo.Sum(x => x.QuantidadeAlunos)
                        })
                        .OrderBy(x => x.AnoTurma)
                        .ToList()
                })
                .OrderBy(x => x.Modalidade)
                .ToList();
        }

        private static string ObterNomeModalidade(string codigoModalidade, string nomeFromDatabase)
        {
            if (!string.IsNullOrWhiteSpace(nomeFromDatabase))
                return nomeFromDatabase;

            if (int.TryParse(codigoModalidade, out var modalidadeCodigo))
            {
                if (Enum.IsDefined(typeof(Modalidade), modalidadeCodigo))
                {
                    var modalidade = (Modalidade)modalidadeCodigo;
                    var displayAttribute = modalidade.GetAttribute<DisplayAttribute>();
                    return displayAttribute?.Name ?? modalidade.ToString();
                }
            }
            
            return codigoModalidade;
        }
    }
}
