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
                            .Count(),
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

            if (periodoId != (int)PeriodoRecuperacaoParalela.Encaminhamento)
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
    }
}