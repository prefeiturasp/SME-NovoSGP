using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre
{
    public class ObterNotaVisaoUeQueryQueryHandler : IRequestHandler<ObterNotaVisaoSmeDreQuery, IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>>
    {
        private readonly IRepositorioNota repositorio;

        public ObterNotaVisaoUeQueryQueryHandler(
            IRepositorioNota repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>> Handle(ObterNotaVisaoSmeDreQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto> dadosConsolidacao;

            if (!string.IsNullOrEmpty(request.CodigoDre))
            {
                dadosConsolidacao = await repositorio.ObterNotasVisaoDre(request.CodigoDre, request.AnoLetivo, request.Bimestre, request.AnoTurma);
            }
            else
            {
                dadosConsolidacao = await repositorio.ObterNotasVisaoSme(request.AnoLetivo, request.Bimestre, request.AnoTurma);
            }

            if (dadosConsolidacao == null || !dadosConsolidacao.Any())
                return Enumerable.Empty<PainelEducacionalNotasVisaoSmeDreDto>();

            var dadosAgrupados = !string.IsNullOrEmpty(request.CodigoDre)
                ? dadosConsolidacao
                    .GroupBy(d => new { d.Modalidade, d.AnoTurma })
                    .Select(grupo => new
                    {
                        Modalidade = grupo.Key.Modalidade.Name(),
                        AnoTurma = grupo.Key.AnoTurma,
                        QuantidadeAbaixoMediaPortugues = grupo.Sum(x => x.QuantidadeAbaixoMediaPortugues),
                        QuantidadeAbaixoMediaMatematica = grupo.Sum(x => x.QuantidadeAbaixoMediaMatematica),
                        QuantidadeAbaixoMediaCiencias = grupo.Sum(x => x.QuantidadeAbaixoMediaCiencias),
                        QuantidadeAcimaMediaPortugues = grupo.Sum(x => x.QuantidadeAcimaMediaPortugues),
                        QuantidadeAcimaMediaMatematica = grupo.Sum(x => x.QuantidadeAcimaMediaMatematica),
                        QuantidadeAcimaMediaCiencias = grupo.Sum(x => x.QuantidadeAcimaMediaCiencias)
                    })
                    .ToList()
                : dadosConsolidacao
                    .GroupBy(d => new { d.CodigoDre, d.Modalidade, d.AnoTurma })
                    .Select(grupo => new
                    {
                        Modalidade = grupo.Key.Modalidade.Name(),
                        AnoTurma = grupo.Key.AnoTurma,
                        QuantidadeAbaixoMediaPortugues = grupo.Sum(x => x.QuantidadeAbaixoMediaPortugues),
                        QuantidadeAbaixoMediaMatematica = grupo.Sum(x => x.QuantidadeAbaixoMediaMatematica),
                        QuantidadeAbaixoMediaCiencias = grupo.Sum(x => x.QuantidadeAbaixoMediaCiencias),
                        QuantidadeAcimaMediaPortugues = grupo.Sum(x => x.QuantidadeAcimaMediaPortugues),
                        QuantidadeAcimaMediaMatematica = grupo.Sum(x => x.QuantidadeAcimaMediaMatematica),
                        QuantidadeAcimaMediaCiencias = grupo.Sum(x => x.QuantidadeAcimaMediaCiencias)
                    })
                    .ToList();

            var modalidades = dadosAgrupados
                .GroupBy(d => d.Modalidade)
                .Select(grupoModalidade => new ModalidadeNotasVisaoSmeDreDto
                {
                    Nome = grupoModalidade.Key,
                    SerieAno = grupoModalidade
                        .GroupBy(m => m.AnoTurma)
                        .Select(grupoSerie => new SerieAnoNotasVisaoSmeDreDto
                        {
                            Nome = grupoSerie.Key,
                            ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                            {
                                new ComponenteCurricularNotasDto
                                {
                                    Nome = "Português",
                                    AbaixoDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAbaixoMediaPortugues),
                                    AcimaDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAcimaMediaPortugues)
                                },
                                new ComponenteCurricularNotasDto
                                {
                                    Nome = "Matemática",
                                    AbaixoDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAbaixoMediaMatematica),
                                    AcimaDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAcimaMediaMatematica)
                                },
                                new ComponenteCurricularNotasDto
                                {
                                    Nome = "Ciências",
                                    AbaixoDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAbaixoMediaCiencias),
                                    AcimaDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAcimaMediaCiencias)
                                }
                            }
                        })
                        .ToList()
                })
                .ToList();

            return new List<PainelEducacionalNotasVisaoSmeDreDto>
            {
                new PainelEducacionalNotasVisaoSmeDreDto
                {
                    Modalidades = modalidades
                }
            };
        }
    }
}
