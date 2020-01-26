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
        private readonly IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela;
        private readonly IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo;
        private readonly IServicoEOL servicoEOL;

        public ConsultasRecuperacaoParalela(
            IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela,
            IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo,
            IServicoEOL servicoEOL,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioRecuperacaoParalela = repositorioRecuperacaoParalela ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalela));
            this.repositorioRecuperacaoParalelaPeriodo = repositorioRecuperacaoParalelaPeriodo ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalelaPeriodo));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<RecuperacaoParalelaListagemDto> Listar(FiltroRecuperacaoParalelaDto filtro)
        {
            var alunosEol = await servicoEOL.ObterAlunosAtivosPorTurma(filtro.TurmaId);
            var alunosRecuperacaoParalela = await repositorioRecuperacaoParalela.Listar(filtro.TurmaId, filtro.PeriodoId);
            return MapearParaDto(alunosEol, alunosRecuperacaoParalela, filtro.TurmaId, filtro.PeriodoId);
        }

        public Task<object> ListarPeriodo()
        {
            throw new NotImplementedException();
        }

        private RecuperacaoParalelaListagemDto MapearParaDto(IEnumerable<AlunoPorTurmaResposta> alunosEol, IEnumerable<RetornoRecuperacaoParalela> alunosRecuperacaoParalela, long turmaId, int periodoId)
        {
            var alunos = alunosEol.Where(w => !alunosRecuperacaoParalela.Select(s => s.AlunoId).Contains(Convert.ToInt32(w.CodigoAluno))).ToList();
            var alunosRecParalela = alunosRecuperacaoParalela.ToList();
            alunos.ForEach(x => alunosRecParalela.Add(new RetornoRecuperacaoParalela { AlunoId = Convert.ToInt64(x.CodigoAluno) }));
            var retorno = alunosRecParalela.Select(s => new { s.AlunoId, s.Id }).Distinct();
            return new RecuperacaoParalelaListagemDto
            {
                Periodo = new RecuperacaoParalelaPeriodoDto
                {
                    Id = periodoId,
                    CriadoPor = alunosRecParalela.FirstOrDefault().CriadoPor,
                    AlteradoPor = alunosRecParalela.FirstOrDefault().AlteradoPor,
                    AlteradoEm = alunosRecParalela.FirstOrDefault().AlteradoEm,
                    AlteradoRF = alunosRecParalela.FirstOrDefault().AlteradoRF,
                    CriadoEm = alunosRecParalela.FirstOrDefault().CriadoEm == DateTime.MinValue ? null : alunosRecParalela.FirstOrDefault().CriadoEm,
                    CriadoRF = alunosRecParalela.FirstOrDefault().CriadoRF,
                    Alunos = retorno.Select(a => new RecuperacaoParalelaAlunoDto
                    {
                        Nome = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.NomeAluno).FirstOrDefault(),
                        NumeroChamada = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.NumeroAlunoChamada).FirstOrDefault(),
                        CodAluno = a.AlunoId,
                        Turma = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == a.AlunoId).Select(s => s.TurmaEscola).FirstOrDefault(),
                        Respostas = alunosRecuperacaoParalela
                                                    .Where(w => w.Id == a.Id)
                                                    .Select(s => new RespostaDto
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