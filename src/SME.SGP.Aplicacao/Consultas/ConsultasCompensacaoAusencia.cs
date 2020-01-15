using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCompensacaoAusencia : ConsultasBase, IConsultasCompensacaoAusencia
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IConsultasCompensacaoAusenciaAluno consultasCompensacaoAusenciaAluno;
        private readonly IServicoEOL servicoEOL;

        public ConsultasCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia, 
                                            IConsultasCompensacaoAusenciaAluno consultasCompensacaoAusenciaAluno,
                                            IServicoEOL servicoEOL, 
                                            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.consultasCompensacaoAusenciaAluno = consultasCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(consultasCompensacaoAusenciaAluno));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>> ListarPaginado(string turmaId, string disciplinaId, int bimestre, string nomeAtividade, string nomeAluno)
        {
            var listaCompensacoesDto = new List<CompensacaoAusenciaListagemDto>();
            var listaCompensacoes = await repositorioCompensacaoAusencia.Listar(Paginacao, turmaId, disciplinaId, bimestre, nomeAtividade);

            // Busca os nomes de alunos do EOL por turma
            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaId);

            foreach (var compensacaoAusencia in listaCompensacoes.Items)
            {
                var compensacaoDto = MapearParaDto(compensacaoAusencia);
                compensacaoAusencia.Alunos = await consultasCompensacaoAusenciaAluno.ObterPorCompensacao(compensacaoAusencia.Id);

                if (compensacaoAusencia.Alunos.Any())
                {
                    foreach (var aluno in compensacaoAusencia.Alunos)
                    {
                        // Adiciona nome do aluno no Dto de retorno
                        var alunoEol = alunos.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);
                        if (alunoEol != null)
                            compensacaoDto.Alunos.Add(alunoEol.NomeAluno);
                    }
                }

                listaCompensacoesDto.Add(compensacaoDto);
            };

            if (!string.IsNullOrEmpty(nomeAluno))
                listaCompensacoesDto = listaCompensacoesDto.Where(c => c.Alunos.Exists(a => a.ToLower().Contains(nomeAluno.ToLower()))).ToList();

            // Mostrar apenas 3 alunos
            foreach (var compensacaoDto in listaCompensacoesDto.Where(c => c.Alunos.Count > 3))
            {
                var qtd = compensacaoDto.Alunos.Count();
                compensacaoDto.Alunos = compensacaoDto.Alunos.GetRange(0, 3);
                compensacaoDto.Alunos.Add($"mais {qtd - 3} alunos");
            }



            var resultado = new PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>();
            resultado.TotalPaginas = listaCompensacoes.TotalPaginas;
            resultado.TotalRegistros = listaCompensacoes.TotalRegistros;
            resultado.Items = listaCompensacoesDto;

            return resultado;
        }

        private CompensacaoAusenciaListagemDto MapearParaDto(CompensacaoAusencia compensacaoAusencia)
            => compensacaoAusencia == null ? null : 
            new CompensacaoAusenciaListagemDto()
            {
                Id = compensacaoAusencia.Id,
                Bimestre = compensacaoAusencia.Bimestre,
                AtividadeNome = compensacaoAusencia.Nome,
                Alunos = new List<string>()
            };
    }
}
