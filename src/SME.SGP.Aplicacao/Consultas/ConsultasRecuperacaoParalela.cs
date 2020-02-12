using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasRecuperacaoParalela : ConsultasBase, IConsultaRecuperacaoParalela
    {
        private readonly IRepositorioEixo repositorioEixo;
        private readonly IRepositorioObjetivo repositorioObjetivo;
        private readonly IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela;
        private readonly IRepositorioResposta repositorioResposta;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoRecuperacaoParalela servicoRecuperacaoParalela;

        public ConsultasRecuperacaoParalela(
            IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela,
            IRepositorioEixo repositorioEixo,
            IRepositorioObjetivo repositorioObjetivo,
            IRepositorioResposta repositorioResposta,
            IServicoEOL servicoEOL,
            IServicoRecuperacaoParalela servicoRecuperacaoParalela,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioRecuperacaoParalela = repositorioRecuperacaoParalela ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalela));
            this.repositorioEixo = repositorioEixo ?? throw new ArgumentNullException(nameof(repositorioEixo));
            this.repositorioObjetivo = repositorioObjetivo ?? throw new ArgumentNullException(nameof(repositorioObjetivo));
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
            this.servicoRecuperacaoParalela = servicoRecuperacaoParalela ?? throw new ArgumentNullException(nameof(servicoRecuperacaoParalela));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<RecuperacaoParalelaListagemDto> Listar(FiltroRecuperacaoParalelaDto filtro)
        {
            var alunosEol = await servicoEOL.ObterAlunosAtivosPorTurma(filtro.TurmaId);

            if (!alunosEol.Any())
                return null;

            var alunosRecuperacaoParalela = await repositorioRecuperacaoParalela.Listar(filtro.TurmaId, filtro.PeriodoId);
            return await MapearParaDtoAsync(alunosEol, alunosRecuperacaoParalela, filtro.TurmaId, filtro.PeriodoId);
        }

        public async Task<PaginacaoResultadoDto<RecuperacaoParalelaTotalResultadoDto>> ListarTotalResultado(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int? pagina)
        {
            var totalResumo = await repositorioRecuperacaoParalela.ListarTotalResultado(periodo, dreId, ueId, cicloId, turmaId, ano, pagina);
            return MapearResultadoPaginadoParaDto(totalResumo);
        }

        public async Task<IEnumerable<RecuperacaoParalelaTotalResultadoDto>> ListarTotalResultadoEncaminhamento(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int? pagina)
        {
            if (periodo.HasValue && periodo.Value != (int)PeriodoRecuperacaoParalela.Encaminhamento) return null;
            var totalResumo = await repositorioRecuperacaoParalela.ListarTotalResultadoEncaminhamento(periodo, dreId, ueId, cicloId, turmaId, ano, pagina);
            return MapearResultadoParaDto(totalResumo);
        }

        public async Task<RecuperacaoParalelaTotalEstudanteDto> TotalEstudantes(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano)
        {
            var totalAlunosPorSeries = await repositorioRecuperacaoParalela.ListarTotalAlunosSeries(periodo, dreId, ueId, cicloId, turmaId, ano);
            if (!totalAlunosPorSeries.Any()) return null;
            var total = totalAlunosPorSeries.Sum(s => s.Total);
            return MapearParaDtoTotalEstudantes(total, totalAlunosPorSeries);
        }

        public async Task<RecuperacaoParalelaTotalEstudantePorFrequenciaDto> TotalEstudantesPorFrequencia(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano)
        {
            var totalAlunosPorSeriesFrequencia = await repositorioRecuperacaoParalela.ListarTotalEstudantesPorFrequencia(periodo, dreId, ueId, cicloId, turmaId, ano);
            if (!totalAlunosPorSeriesFrequencia.Any()) return null;
            var total = totalAlunosPorSeriesFrequencia.Sum(s => s.Total);
            return MapearParaDtoTotalEstudantesPorFrequencia(total, totalAlunosPorSeriesFrequencia);
        }

        private async Task<RecuperacaoParalelaListagemDto> MapearParaDtoAsync(IEnumerable<AlunoPorTurmaResposta> alunosEol, IEnumerable<RetornoRecuperacaoParalela> alunosRecuperacaoParalela, long turmaId, long periodoId)
        {
            //alunos eol que não estão ainda na tabela de recuperação paralela
            var alunos = alunosEol.Where(w => !alunosRecuperacaoParalela.Select(s => s.AlunoId).Contains(Convert.ToInt32(w.CodigoAluno))).ToList();

            var respostas = await repositorioResposta.Listar(periodoId);
            var objetivos = await repositorioObjetivo.Listar(periodoId);
            var eixos = await repositorioEixo.Listar(periodoId);

            var alunosRecParalela = alunosRecuperacaoParalela.ToList();
            //adicionar na lista de recuperação paralela com o id zerado, com isso saberá que será um novo registro
            alunos.ForEach(x => alunosRecParalela.Add(new RetornoRecuperacaoParalela { AlunoId = Convert.ToInt64(x.CodigoAluno) }));

            var retorno = alunosRecParalela.Select(s => new { s.AlunoId, s.Id }).Distinct();
            var recuperacaoRetorno = new RecuperacaoParalelaListagemDto
            {
                Eixos = eixos,
                Objetivos = objetivos,
                Respostas = respostas,
                Periodo = new RecuperacaoParalelaPeriodoListagemDto
                {
                    Id = periodoId,
                    CriadoPor = alunosRecParalela.OrderByDescending(o => o.CriadoEm).FirstOrDefault().CriadoPor,
                    AlteradoPor = alunosRecParalela.OrderByDescending(o => o.AlteradoEm).FirstOrDefault().AlteradoPor,
                    AlteradoEm = alunosRecParalela.OrderByDescending(o => o.AlteradoEm).FirstOrDefault().AlteradoEm,
                    AlteradoRF = alunosRecParalela.OrderByDescending(o => o.AlteradoEm).FirstOrDefault().AlteradoRF,
                    CriadoEm = alunosRecParalela.OrderByDescending(o => o.CriadoEm).FirstOrDefault().CriadoEm == DateTime.MinValue ? null : alunosRecParalela.FirstOrDefault().CriadoEm,
                    CriadoRF = alunosRecParalela.OrderByDescending(o => o.CriadoEm).FirstOrDefault().CriadoRF,
                    Alunos = retorno.Select(a => new RecuperacaoParalelaAlunoListagemDto
                    {
                        Id = a.Id,
                        Concluido = servicoRecuperacaoParalela.ObterStatusRecuperacaoParalela(
                            alunosRecuperacaoParalela
                            .Where(w => w.Id == a.Id)
                            .Count() - 1,
                            objetivos.Count()),
                        ParecerConclusivo = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.ParecerConclusivo).FirstOrDefault(),
                        Nome = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.NomeAluno).FirstOrDefault(),
                        NumeroChamada = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.NumeroAlunoChamada).FirstOrDefault(),
                        CodAluno = a.AlunoId,
                        Turma = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.TurmaEscola).FirstOrDefault(),
                        TurmaId = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.CodigoTurma).FirstOrDefault(),
                        TurmaRecuperacaoParalelaId = turmaId,
                        Respostas = alunosRecuperacaoParalela
                                                    .Where(w => w.Id == a.Id)
                                                    .Select(s => new ObjetivoRespostaDto
                                                    {
                                                        ObjetivoId = s.ObjetivoId,
                                                        RespostaId = s.RespostaId
                                                    }).ToList()
                    }).OrderBy(o => o.Nome).ToList()
                }
            };

            //parecer conclusivo
            recuperacaoRetorno.Periodo.Alunos.Where(w => w.Id == 0 && w.ParecerConclusivo.HasValue && char.GetNumericValue(w.ParecerConclusivo.Value) <= 3).ToList().ForEach(x => x.Respostas.Add(new ObjetivoRespostaDto
            {
                ObjetivoId = 3,
                RespostaId = servicoRecuperacaoParalela.ValidarParecerConclusivo(x.ParecerConclusivo.Value)
            }));

            if (periodoId != (int)PeriodoRecuperacaoParalela.Encaminhamento && alunos.Any())
            {
                //pegar o dados daquela turma pap
                var dadosTurma = alunos.FirstOrDefault(w => w.CodigoComponenteCurricular.HasValue);
                //pegar as frequencias de acordo com os critérios
                var frequencias = await servicoRecuperacaoParalela.ObterFrequencias(alunos.Select(w => w.CodigoAluno).ToArray(), dadosTurma.CodigoComponenteCurricular.ToString(), dadosTurma.Ano, (PeriodoRecuperacaoParalela)periodoId);
                //frequencias
                foreach (var frequencia in frequencias)
                {
                    if (recuperacaoRetorno.Periodo.Alunos.Any(w => w.CodAluno == Convert.ToInt32(frequencia.Key)))
                    {
                        recuperacaoRetorno.Periodo.Alunos
                            .FirstOrDefault(w => w.CodAluno == Convert.ToInt32(frequencia.Key))
                            .Respostas
                            .Add(new ObjetivoRespostaDto
                            {
                                ObjetivoId = 4,
                                RespostaId = frequencia.Value
                            });
                    }
                }
            }
            return recuperacaoRetorno;
        }

        private RecuperacaoParalelaTotalEstudanteDto MapearParaDtoTotalEstudantes(int total, IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoDto> totalAlunosPorSeries)
        {
            //todo: mudar double para um numero que quebre direito a porcentagem
            return new RecuperacaoParalelaTotalEstudanteDto
            {
                QuantidadeTotal = total,
                PorcentagemTotal = 100,
                Anos = totalAlunosPorSeries.Select(x => new RecuperacaoParalelaTotalAnoDto
                {
                    AnoDescricao = x.Ano,
                    Quantidade = x.Total,
                    Porcentagem = ((double)(x.Total * 100)) / total
                }),
                Ciclos = totalAlunosPorSeries.GroupBy(g => g.Ciclo).Select(x => new RecuperacaoParalelaTotalCicloDto
                {
                    CicloDescricao = x.First().Ciclo,
                    Quantidade = x.Sum(c => c.Total),
                    Porcentagem = ((double)(x.Sum(c => c.Total) * 100)) / total
                })
            };
        }

        private RecuperacaoParalelaTotalEstudantePorFrequenciaDto MapearParaDtoTotalEstudantesPorFrequencia(int total, IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoFrequenciaDto> items)
        {
            var retorno = new RecuperacaoParalelaTotalEstudantePorFrequenciaDto
            {
                Frequencia = items.GroupBy(fg => new { fg.RespostaId, fg.Frequencia }).Select(freq => new RecuperacaoParalelaTotalEstudanteFrequenciaDto
                {
                    FrequenciaDescricao = freq.Key.Frequencia,
                    PorcentagemTotalFrequencia = (freq.Sum(x => x.Total) * 100) / total,
                    QuantidadeTotalFrequencia = freq.Sum(x => x.Total),
                    Linhas = items.Where(wlinha => wlinha.RespostaId == freq.Key.RespostaId).GroupBy(glinha => new { glinha.RespostaId }).Select(lin => new RecuperacaoParalelaResumoFrequenciaDto
                    {
                        Anos = items.Where(wano => wano.RespostaId == lin.Key.RespostaId).GroupBy(gano => new { gano.Ano }).Select(ano => new RecuperacaoParalelaTotalFrequenciaAnoDto
                        {
                            CodigoAno = ano.Key.Ano,
                            Chave = ano.Key.Ano.ToString(),
                            Descricao = $"{ano.Key.Ano}º",
                            Quantidade = ano.Sum(c => c.Total),
                            Porcentagem = ((double)ano.Sum(c => c.Total) * 100) / total,
                            TotalQuantidade = items.Where(t => t.RespostaId == lin.Key.RespostaId).Sum(s => s.Total),
                            TotalPorcentagem = ((double)items.Where(t => t.RespostaId == lin.Key.RespostaId).Sum(s => s.Total) * 100) / total
                        }),
                        Ciclos = items.Where(wciclo => wciclo.RespostaId == lin.Key.RespostaId).GroupBy(gciclo => new { gciclo.CicloId, gciclo.Ciclo }).Select(ciclo => new RecuperacaoParalelaTotalFrequenciaCicloDto
                        {
                            CodigoCiclo = ciclo.Key.CicloId,
                            Descricao = ciclo.Key.Ciclo,
                            Chave = ciclo.Key.Ciclo,
                            Quantidade = ciclo.Sum(c => c.Total),
                            Porcentagem = ((double)ciclo.Sum(c => c.Total) * 100) / total,
                            TotalQuantidade = items.Where(t => t.RespostaId == lin.Key.RespostaId).Sum(s => s.Total),
                            TotalPorcentagem = ((double)items.Where(t => t.RespostaId == lin.Key.RespostaId).Sum(s => s.Total) * 100) / total
                        })
                    }).ToList()
                }).ToList()
            };

            var linhaTotal = new List<RecuperacaoParalelaResumoFrequenciaDto>();
            linhaTotal.Add(new RecuperacaoParalelaResumoFrequenciaDto
            {
                Anos = items.GroupBy(w => w.Ano).Select(s => new RecuperacaoParalelaTotalFrequenciaAnoDto
                {
                    Quantidade = s.Sum(x => x.Total),
                    TotalQuantidade = total,
                    Porcentagem = ((double)s.Sum(x => x.Total) * 100) / total,
                    TotalPorcentagem = 100,
                    Chave = s.Key.ToString(),
                    CodigoAno = s.Key,
                }),
                Ciclos = items.GroupBy(w => new { w.Ciclo, w.CicloId }).Select(s => new RecuperacaoParalelaTotalFrequenciaCicloDto
                {
                    CodigoCiclo = s.Key.CicloId,
                    Descricao = s.Key.Ciclo,
                    Chave = s.Key.Ciclo,
                    Quantidade = s.Sum(c => c.Total),
                    Porcentagem = ((double)s.Sum(x => x.Total) * 100) / total,
                    TotalQuantidade = total,
                    TotalPorcentagem = 100
                })
            });
            retorno.Frequencia.Add(new RecuperacaoParalelaTotalEstudanteFrequenciaDto
            {
                FrequenciaDescricao = "Total",
                QuantidadeTotalFrequencia = total,
                PorcentagemTotalFrequencia = 100,
                Linhas = linhaTotal
            });

            return retorno;
        }

        private PaginacaoResultadoDto<RecuperacaoParalelaTotalResultadoDto> MapearResultadoPaginadoParaDto(PaginacaoResultadoDto<RetornoRecuperacaoParalelaTotalResultadoDto> totalResumo)
        {
            return new PaginacaoResultadoDto<RecuperacaoParalelaTotalResultadoDto>
            {
                Items = MapearResultadoParaDto(totalResumo.Items),
                TotalPaginas = totalResumo.TotalPaginas,
                TotalRegistros = totalResumo.TotalRegistros
            };
        }

        private IEnumerable<RecuperacaoParalelaTotalResultadoDto> MapearResultadoParaDto(IEnumerable<RetornoRecuperacaoParalelaTotalResultadoDto> items)
        {
            var total = items.Sum(s => s.Total);
            return items.GroupBy(g => new { g.EixoId, g.Eixo }).Select(x => new RecuperacaoParalelaTotalResultadoDto
            {
                EixoDescricao = x.Key.Eixo,
                Objetivos = items.Where(obj => obj.EixoId == x.Key.EixoId).GroupBy(objetivo => new { objetivo.ObjetivoId, objetivo.Objetivo }).Select(z => new RecuperacaoParalelaResumoResultadoObjetivoDto
                {
                    ObjetivoDescricao = z.Key.Objetivo,
                    Anos = items.Where(ano => ano.ObjetivoId == z.Key.ObjetivoId).GroupBy(h => new { h.Ano, h.ObjetivoId }).Select(a => new RecuperacaoParalelaResumoResultadoAnoDto
                    {
                        AnoDescricao = a.Key.Ano,
                        Respostas = items.Where(res => res.ObjetivoId == a.Key.ObjetivoId).GroupBy(gre => new { gre.Resposta, gre.RespostaId }).Select(r => new RecuperacaoParalelaResumoResultadoRespostaDto
                        {
                            RespostaDescricao = r.Key.Resposta,
                            Quantidade = r.Sum(q => q.Total),
                            Porcentagem = ((double)r.Sum(q => q.Total) * 100) / total
                        })
                    }),
                    Ciclos = items.Where(ciclo => ciclo.ObjetivoId == z.Key.ObjetivoId).GroupBy(c => new { c.Ciclo, c.ObjetivoId }).Select(cs => new RecuperacaoParalelaResumoResultadoCicloDto
                    {
                        CicloDescricao = cs.Key.Ciclo,
                        Respostas = items.Where(res => res.ObjetivoId == cs.Key.ObjetivoId).GroupBy(gre => new { gre.Resposta, gre.RespostaId }).Select(r => new RecuperacaoParalelaResumoResultadoRespostaDto
                        {
                            RespostaDescricao = r.Key.Resposta,
                            Quantidade = r.Sum(q => q.Total),
                            Porcentagem = ((double)r.Sum(q => q.Total) * 100) / total
                        })
                    }),
                    Total = items.Where(tot => tot.ObjetivoId == z.Key.ObjetivoId).GroupBy(gt => gt.ObjetivoId).Select(tr => new RecuperacaoParalelaResumoResultadoRespostaDto
                    {
                        TotalQuantidade = tr.Sum(tq => tq.Total),
                        TotalPorcentagem = (tr.Sum(tq => tq.Total) * 100) / total
                    })
                })
            });
        }
    }
}