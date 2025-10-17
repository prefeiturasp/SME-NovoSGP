using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre
{
    public class ObterNotaVisaoSmeDreQueryHandler : IRequestHandler<ObterNotaVisaoSmeDreQuery, IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>>
    {
        private readonly IRepositorioNota repositorio;

        public ObterNotaVisaoSmeDreQueryHandler(
            IRepositorioNota repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>> Handle(ObterNotaVisaoSmeDreQuery request, CancellationToken cancellationToken)
        {
            var dadosConsolidacao = await repositorio.ObterNotasVisaoSmeDre(request.CodigoDre, request.AnoLetivo, request.Bimestre, request.AnoTurma);

            if (dadosConsolidacao == null || !dadosConsolidacao.Any())
                return Enumerable.Empty<PainelEducacionalNotasVisaoSmeDreDto>();

            var dadosAgrupados = dadosConsolidacao
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
                            ComponentesCurriculares = new List<ComponenteCurricularNotasVisaoSmeDreDto>
                            {
                                new ComponenteCurricularNotasVisaoSmeDreDto
                                {
                                    Nome = "Português",
                                    AbaixoDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAbaixoMediaPortugues),
                                    AcimaDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAcimaMediaPortugues)
                                },
                                new ComponenteCurricularNotasVisaoSmeDreDto
                                {
                                    Nome = "Matemática",
                                    AbaixoDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAbaixoMediaMatematica),
                                    AcimaDaMedia = (int)grupoSerie.Sum(s => s.QuantidadeAcimaMediaMatematica)
                                },
                                new ComponenteCurricularNotasVisaoSmeDreDto
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
