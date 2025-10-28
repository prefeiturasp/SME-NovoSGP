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

            var modalidadesAgrupadas = dadosRaw
                .GroupBy(x => x.CodigoModalidade)
                .Select(grupo => new ModalidadeReclassificacaoDto
                {
                    Modalidade = new ModalidadeReclassificacaoArrayDto
                    {
                        NomeModalidade = ObterNomeModalidade(grupo.Key, grupo.First().Nome),
                        AnoTurma = grupo.First().AnoTurma,
                        QuantidadeAlunos = grupo.Sum(x => x.QuantidadeAlunos)
                    }
                })
                .OrderBy(x => x.Modalidade.AnoTurma);

            return new[]
            {
                new PainelEducacionalReclassificacaoDto
                {
                    Modalidades = modalidadesAgrupadas
                }
            };
        }

        private static string ObterNomeModalidade(string codigoModalidade, string nomeFromDatabase)
        {
            // Se o nome já vem do banco de dados, usa ele
            if (!string.IsNullOrWhiteSpace(nomeFromDatabase))
                return nomeFromDatabase;

            // Fallback: tenta converter o código da modalidade para o nome do enum
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
