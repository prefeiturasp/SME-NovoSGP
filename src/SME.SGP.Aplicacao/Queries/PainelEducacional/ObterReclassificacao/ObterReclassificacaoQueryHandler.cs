using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterReclassificacao
{
    public class ObterReclassificacaoQueryHandler : IRequestHandler<ObterReclassificacaoQuery, IEnumerable<PainelEducacionalReclassificacaoDto>>
    {
        private readonly IRepositorioReclassificacao repositorio;

        public ObterReclassificacaoQueryHandler(
            IRepositorioReclassificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalReclassificacaoDto>> Handle(ObterReclassificacaoQuery request, CancellationToken cancellationToken)
        {
            var dadosConsolidados = await repositorio.ObterReclassificacao(request.CodigoDre, request.CodigoUe, request.AnoLetivo, request.AnoTurma);

            return MapearParaDto(dadosConsolidados, request.CodigoDre, request.CodigoUe);
        }

        private IEnumerable<PainelEducacionalReclassificacaoDto> MapearParaDto(IEnumerable<PainelEducacionalReclassificacaoDto> dadosConsolidados, string codigoDre, string codigoUe)
        {
            if (dadosConsolidados == null || !dadosConsolidados.Any())
                return Enumerable.Empty<PainelEducacionalReclassificacaoDto>();

            var resultado = new List<PainelEducacionalReclassificacaoDto>();

            foreach (var item in dadosConsolidados)
            {
                if (item.Modalidade?.Any() == true)
                {
                    var modalidadesAgrupadas = CodigoDreOuCodigoUeEhNulo(codigoDre, codigoUe)
                        ? item.Modalidade
                           .GroupBy(m => m.AnoTurma)
                            .Select(g => new ModalidadeReclassificacaoDto
                            {
                                Nome = g.FirstOrDefault()?.Nome, 
                                AnoTurma = g.Key, 
                                QuantidadeAlunos = g.Sum(m => m.QuantidadeAlunos)
                            })
                            .OrderBy(m => m.AnoTurma)
                        : item.Modalidade.Select(m => new ModalidadeReclassificacaoDto
                        {
                            Nome = m.Nome,
                            AnoTurma = m.AnoTurma,
                            QuantidadeAlunos = m.QuantidadeAlunos
                        })
                        .OrderBy(m => m.AnoTurma);

                    resultado.Add(new PainelEducacionalReclassificacaoDto
                    {
                        Modalidade = modalidadesAgrupadas
                    });
                }
            }

            return resultado.OrderBy(a => a.Modalidade.FirstOrDefault()?.AnoTurma);
        }

        public bool CodigoDreOuCodigoUeEhNulo(string codigoDre, string codigoUe)
        {
            return string.IsNullOrEmpty(codigoDre) || string.IsNullOrEmpty(codigoUe);
        }
    }
}
