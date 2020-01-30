using SME.SGP.Aplicacao.Integracoes;
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
        private readonly IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo;
        private readonly IRepositorioResposta repositorioResposta;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoRecuperacaoParalela servicoRecuperacaoParalela;

        public ConsultasRecuperacaoParalela(
            IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela,
            IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo,
            IRepositorioEixo repositorioEixo,
            IRepositorioObjetivo repositorioObjetivo,
            IRepositorioResposta repositorioResposta,
            IServicoEOL servicoEOL,
            IServicoRecuperacaoParalela servicoRecuperacaoParalela,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioRecuperacaoParalela = repositorioRecuperacaoParalela ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalela));
            this.repositorioRecuperacaoParalelaPeriodo = repositorioRecuperacaoParalelaPeriodo ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalelaPeriodo));
            this.repositorioEixo = repositorioEixo ?? throw new ArgumentNullException(nameof(repositorioEixo));
            this.repositorioObjetivo = repositorioObjetivo ?? throw new ArgumentNullException(nameof(repositorioObjetivo));
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
            this.servicoRecuperacaoParalela = servicoRecuperacaoParalela ?? throw new ArgumentNullException(nameof(servicoRecuperacaoParalela));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<RecuperacaoParalelaListagemDto> Listar(FiltroRecuperacaoParalelaDto filtro)
        {
            var alunosEol = await servicoEOL.ObterAlunosAtivosPorTurma(filtro.TurmaId);
            var alunosRecuperacaoParalela = await repositorioRecuperacaoParalela.Listar(filtro.TurmaId, filtro.PeriodoId);
            return await MapearParaDtoAsync(alunosEol, alunosRecuperacaoParalela, filtro.TurmaId, filtro.PeriodoId);
        }

        private async Task<RecuperacaoParalelaListagemDto> MapearParaDtoAsync(IEnumerable<AlunoPorTurmaResposta> alunosEol, IEnumerable<RetornoRecuperacaoParalela> alunosRecuperacaoParalela, long turmaId, long periodoId)
        {
            var alunos = alunosEol.Where(w => !alunosRecuperacaoParalela.Select(s => s.AlunoId).Contains(Convert.ToInt32(w.CodigoAluno))).ToList();
            var respostas = await repositorioResposta.Listar(periodoId);
            var objetivos = await repositorioObjetivo.Listar(periodoId);
            var eixos = await repositorioEixo.Listar(periodoId);

            var alunosRecParalela = alunosRecuperacaoParalela.Where(w => w.PeriodoRecuperacaoParalelaId == periodoId).ToList();
            alunos.ForEach(x => alunosRecParalela.Add(new RetornoRecuperacaoParalela { AlunoId = Convert.ToInt64(x.CodigoAluno) }));
            var retorno = alunosRecParalela.Select(s => new { s.AlunoId, s.Id }).Distinct();
            return new RecuperacaoParalelaListagemDto
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
                            .Count(),
                            objetivos.Count()),
                        Nome = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.NomeAluno).FirstOrDefault(),
                        NumeroChamada = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.NumeroAlunoChamada).FirstOrDefault(),
                        CodAluno = a.AlunoId,
                        Turma = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.TurmaEscola).FirstOrDefault(),
                        TurmaId = turmaId,
                        Respostas = alunosRecuperacaoParalela
                                                    .Where(w => w.Id == a.Id)
                                                    .Select(s => new ObjetivoRespostaDto
                                                    {
                                                        ObjetivoId = s.ObjetivoId,
                                                        RespostaId = s.RespostaId
                                                    }).ToList()
                    }).ToList()
                }
            };
        }
    }
}