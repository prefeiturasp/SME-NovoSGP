using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasNotasVisaoUeUseCase : ConsultasBase, IConsultasNotasVisaoUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasNotasVisaoUeUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>> ObterNotasVisaoUe(string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            var dados = await mediator.Send(new ObterNotaVisaoUeQuery(this.Paginacao, codigoUe, anoLetivo, bimestre, modalidade));

            if (dados == null || dados.Items == null || !dados.Items.Any())
            {
                return new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
                {
                    Items = Enumerable.Empty<PainelEducacionalNotasVisaoUeDto>(),
                    TotalPaginas = 0,
                    TotalRegistros = 0
                };
            }

            var itensConvertidos = ConverterParaDto(dados.Items.ToList());

            return new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
            {
                Items = itensConvertidos,
                TotalPaginas = dados.TotalPaginas,
                TotalRegistros = dados.TotalRegistros
            };
        }

        private IEnumerable<PainelEducacionalNotasVisaoUeDto> ConverterParaDto(List<SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe.PainelEducacionalNotasVisaoUeRetornoSelectDto> dadosBrutos)
        {
            return dadosBrutos
                .GroupBy(d => new { d.AnoLetivo, d.CodigoDre, d.CodigoUe, d.Bimestre })
                .Select(grupo => new PainelEducacionalNotasVisaoUeDto
                {
                    Modalidades = grupo
                        .GroupBy(d => d.Modalidade)
                        .Select(modalidadeGrupo => new ModalidadeNotasVisaoUeDto
                        {
                            Id = modalidadeGrupo.Key != 0 ? (int)modalidadeGrupo.Key : 0,
                            Nome = modalidadeGrupo.Key.Name(),
                            Turmas = modalidadeGrupo
                                .GroupBy(d => d.AnoTurma)
                                .Select(turmaGrupo => new TurmaNotasVisaoUeDto
                                {
                                    Nome = turmaGrupo.Key,
                                    ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                                    {
                                        new ComponenteCurricularNotasDto
                                        {
                                            Nome = "Português",
                                            AbaixoDaMedia = (int)turmaGrupo.Sum(d => d.QuantidadeAbaixoMediaPortugues),
                                            AcimaDaMedia = (int)turmaGrupo.Sum(d => d.QuantidadeAcimaMediaPortugues)
                                        },
                                        new ComponenteCurricularNotasDto
                                        {
                                            Nome = "Matemática",
                                            AbaixoDaMedia = (int)turmaGrupo.Sum(d => d.QuantidadeAbaixoMediaMatematica),
                                            AcimaDaMedia = (int)turmaGrupo.Sum(d => d.QuantidadeAcimaMediaMatematica)
                                        },
                                        new ComponenteCurricularNotasDto
                                        {
                                            Nome = "Ciências",
                                            AbaixoDaMedia = (int)turmaGrupo.Sum(d => d.QuantidadeAbaixoMediaCiencias),
                                            AcimaDaMedia = (int)turmaGrupo.Sum(d => d.QuantidadeAcimaMediaCiencias)
                                        }
                                    }
                                })
                        })
                });
        }
    }   
}
