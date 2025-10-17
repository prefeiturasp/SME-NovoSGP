using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe
{
    public class ObterNotaVisaoUeQueryHandler : IRequestHandler<ObterNotaVisaoUeQuery, PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>
    {
        private readonly IRepositorioNota repositorio;

        public ObterNotaVisaoUeQueryHandler(
            IRepositorioNota repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>> Handle(ObterNotaVisaoUeQuery request, CancellationToken cancellationToken)
        {
            var dadosConsolidacao = await repositorio.ObterNotasVisaoUe(request.CodigoDre, request.AnoLetivo, request.Bimestre, request.Modalidade);

            if (dadosConsolidacao == null || !dadosConsolidacao.Any())
            {
                return new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
                {
                    Items = Enumerable.Empty<PainelEducacionalNotasVisaoUeDto>(),
                    TotalRegistros = 0,
                    TotalPaginas = 0
                };
            }

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
                .Select(grupoModalidade => new ModalidadeNotasVisaoUeDto
                {
                    Nome = grupoModalidade.Key,
                    Turmas = grupoModalidade
                        .GroupBy(m => m.AnoTurma)
                        .Select(grupoSerie => new TurmaNotasVisaoUeDto
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

            IEnumerable<ModalidadeNotasVisaoUeDto> modalidadesPaginadas = modalidades;
            var totalRegistrosModalidades = modalidades.Count;

            if (request.Paginacao != null && request.Paginacao.QuantidadeRegistros > 0)
            {
                modalidadesPaginadas = modalidades
                    .Skip(request.Paginacao.QuantidadeRegistrosIgnorados)
                    .Take(request.Paginacao.QuantidadeRegistros);
            }
            
            var resultado = new List<PainelEducacionalNotasVisaoUeDto>
            {
                new PainelEducacionalNotasVisaoUeDto
                {
                    Modalidades = modalidadesPaginadas
                }
            };

            var totalPaginas = request.Paginacao != null && request.Paginacao.QuantidadeRegistros > 0
                ? (int)Math.Ceiling((double)totalRegistrosModalidades / request.Paginacao.QuantidadeRegistros)
                : 1;

            return new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
            {
                Items = resultado,
                TotalRegistros = totalRegistrosModalidades,
                TotalPaginas = totalPaginas
            };
        }
    }
}
