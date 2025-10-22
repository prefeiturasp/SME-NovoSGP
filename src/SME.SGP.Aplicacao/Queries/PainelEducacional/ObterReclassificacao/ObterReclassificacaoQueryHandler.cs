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
            
            return MapearParaDto(dadosConsolidados);
        }

        private IEnumerable<PainelEducacionalReclassificacaoDto> MapearParaDto(IEnumerable<PainelEducacionalReclassificacaoDto> dadosConsolidados)
        {
            if (dadosConsolidados == null || !dadosConsolidados.Any())
                return Enumerable.Empty<PainelEducacionalReclassificacaoDto>();

            var resultado = new List<PainelEducacionalReclassificacaoDto>();

            foreach (var item in dadosConsolidados)
            {
                if (item.Modalidade?.Any() == true)
                {
                    var modalidadesAgrupadas = item.Modalidade
                        .SelectMany(m => new[]
                        {
                            new ModalidadeReclassificacaoDto
                            {
                                Nome = m.Nome,
                                AnoTurma = m.AnoTurma,
                                QuantidadeAlunos = m.QuantidadeAlunos
                            }
                        });

                    resultado.Add(new PainelEducacionalReclassificacaoDto
                    {
                        Modalidade = modalidadesAgrupadas
                    });
                }
            }

            return resultado.OrderBy(a => a.Modalidade.FirstOrDefault().AnoTurma);
        }
    }
}
