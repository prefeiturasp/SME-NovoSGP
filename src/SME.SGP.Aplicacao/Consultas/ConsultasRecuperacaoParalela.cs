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

        public async Task<IEnumerable<RecuperacaoParalelaDto>> Listar(FiltroRecuperacaoParalelaDto filtro)
        {
            var alunosEol = await servicoEOL.ObterAlunosAtivosPorTurma(filtro.TurmaId);
            var alunosRecuperacaoParalela = await repositorioRecuperacaoParalela.Listar(filtro.TurmaId);
            return MapearParaDto(alunosEol, alunosRecuperacaoParalela, filtro.TurmaId);
        }

        public Task<object> ListarPeriodo()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<RecuperacaoParalelaDto> MapearParaDto(IEnumerable<AlunoPorTurmaResposta> alunosEol, IEnumerable<RetornoRecuperacaoParalela> alunosRecuperacaoParalela, long turmaId)
        {
            var alunos = alunosEol.Where(w => !alunosRecuperacaoParalela.Select(s => s.AlunoId).Contains(Convert.ToInt32(w.CodigoAluno))).ToList();
            var alunosRecParalela = alunosRecuperacaoParalela.ToList();
            var periodos = repositorioRecuperacaoParalelaPeriodo.Listar();
            alunos.ForEach(x => alunosRecParalela.Add(new RetornoRecuperacaoParalela { AlunoId = Convert.ToInt64(x.CodigoAluno) }));
            var retorno = alunosRecParalela.Select(s => new { s.AlunoId, s.Id }).Distinct();
            return retorno?.Select(x => new RecuperacaoParalelaDto
            {
                Id = x.Id,
                Nome = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == x.AlunoId).Select(s => s.NomeAluno).FirstOrDefault(),
                NumeroChamada = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == x.AlunoId).Select(s => s.NumeroAlunoChamada).FirstOrDefault(),
                Turma = alunosEol.Where(w => Convert.ToInt32(w.CodigoAluno) == x.AlunoId).Select(s => s.TurmaEscola).FirstOrDefault(),
                Periodos = periodos
                        .Select(ps => new RecuperacaoParalelaPeriodoDto
                        {
                            Id = ps.Id,
                            Descricao = ps.Descricao,
                            Nome = ps.Nome,
                            Respostas = alunosRecuperacaoParalela
                                        .Where(w => w.Id == x.Id)
                                        .Select(s => new RespostaDto
                                        {
                                            ObjetivoId = s.ObjetivoId,
                                            RespostaId = s.RespostaId
                                        }).ToList()
                        }).ToList()
            }).ToList();
        }
    }
}