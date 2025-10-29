using MediatR;
using SME.SGP.Aplicacao.Consultas;
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
    public class ConsultasNotasVisaoUeUseCase : ConsultasNotaBase, IConsultasNotasVisaoUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasNotasVisaoUeUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoNotaResultadoDto<TurmaNotasVisaoUeDto>> ObterNotasVisaoUe(string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            var dados = await mediator.Send(new ObterNotaVisaoUeQuery(this.Paginacao, codigoUe, anoLetivo, bimestre, modalidade));

            if (dados == null || dados.Items == null || !dados.Items.Any())
            {
                return new PaginacaoNotaResultadoDto<TurmaNotasVisaoUeDto>
                {
                    Items = Enumerable.Empty<TurmaNotasVisaoUeDto>(),
                    PaginaAtual = this.Paginacao.QuantidadeRegistros > 0 
                        ? (this.Paginacao.QuantidadeRegistrosIgnorados / this.Paginacao.QuantidadeRegistros) + 1 
                        : 1,
                    RegistrosPorPagina = this.Paginacao.QuantidadeRegistros,
                    TotalPaginas = 0,
                    TotalRegistros = 0
                };
            }

            var turmasConvertidas = ConverterParaTurmas(dados.Items.ToList());

            return new PaginacaoNotaResultadoDto<TurmaNotasVisaoUeDto>
            {
                Items = turmasConvertidas,
                PaginaAtual = dados.PaginaAtual,
                RegistrosPorPagina = dados.RegistrosPorPagina,
                TotalPaginas = dados.TotalPaginas,
                TotalRegistros = dados.TotalRegistros
            };
        }

        private IEnumerable<TurmaNotasVisaoUeDto> ConverterParaTurmas(List<PainelEducacionalNotasVisaoUeRetornoSelectDto> dadosBrutos)
        {
            return dadosBrutos
                .GroupBy(d => new { d.AnoTurma, d.Modalidade})
                .Select(turmaGrupo => new TurmaNotasVisaoUeDto
                {
                    Nome = turmaGrupo.Key.AnoTurma,
                    Modalidade = (int)turmaGrupo.Key.Modalidade,
                    ModalidadeDescricao = turmaGrupo.Key.Modalidade.ObterNome(),
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
                .OrderBy(t => t.Nome)
                .ToList();
        }
    }
}
