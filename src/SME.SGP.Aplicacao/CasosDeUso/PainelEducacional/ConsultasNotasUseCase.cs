using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasNotasUseCase : ConsultasBase, IConsultasNotasUseCase
    {
        private readonly IMediator mediator;

        public ConsultasNotasUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, string anoTurma)
        {
            var dadosConsolidacao = await mediator.Send(new ObterNotaVisaoSmeDreQuery(codigoDre, anoLetivo, bimestre, anoTurma));

            if (dadosConsolidacao == null || !dadosConsolidacao.Any())
                return Enumerable.Empty<PainelEducacionalNotasVisaoSmeDreDto>();

            var dadosAgrupados = !string.IsNullOrEmpty(codigoDre)
                ? dadosConsolidacao
                    .GroupBy(d => new { d.CodigoDre, d.AnoLetivo, d.Modalidade, d.AnoTurma, d.Bimestre })
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
                    .GroupBy(d => new { d.AnoLetivo, d.Modalidade, d.AnoTurma, d.Bimestre })
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

        public async Task<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>> ObterNotasVisaoUe(string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            return await mediator.Send(new ObterNotaVisaoUeQuery(this.Paginacao, codigoUe, anoLetivo, bimestre, modalidade));
        }
    }
}
