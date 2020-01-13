using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCompensacaoAusencia : IConsultasCompensacaoAusencia
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IServicoEOL servicoEOL;

        public ConsultasCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia, 
                                            IServicoEOL servicoEOL)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<CompensacaoAusenciaListagemDto>> Listar(string turmaId, string disciplinaId, int bimestre, string nomeAtividade, string nomeAluno)
        {
            var listaCompensacoesDto = new List<CompensacaoAusenciaListagemDto>();

            var listaCompensacoes = await repositorioCompensacaoAusencia.Listar(turmaId, disciplinaId, bimestre, nomeAtividade);

            // Busca os nomes de alunos do EOL por turma
            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaId);

            foreach (var compensacaoAusencia in listaCompensacoes)
            {
                var compensacaoDto = MapearParaDto(compensacaoAusencia);

                if (compensacaoAusencia.Alunos.Any())
                {
                    foreach (var aluno in compensacaoAusencia.Alunos)
                    {
                        if (compensacaoDto.Alunos.Count == 3)
                        {
                            compensacaoDto.Alunos.Add($"mais {compensacaoAusencia.Alunos.Count() - 3} alunos");
                            break;
                        }

                        // Adiciona nome do aluno no Dto de retorno
                        var alunoEol = alunos.FirstOrDefault(a => a.CodigoAluno == aluno.CodigoAluno);
                        if (alunoEol != null)
                            compensacaoDto.Alunos.Add(alunoEol.NomeAluno);
                    }
                }

                listaCompensacoesDto.Add(compensacaoDto);
            };

            if (!string.IsNullOrEmpty(nomeAluno))
                listaCompensacoesDto = listaCompensacoesDto.Where(c => c.Alunos.Exists(a => a.Contains(nomeAluno))).ToList();

            return listaCompensacoesDto;
        }

        private CompensacaoAusenciaListagemDto MapearParaDto(CompensacaoAusencia compensacaoAusencia)
            => compensacaoAusencia == null ? null : 
            new CompensacaoAusenciaListagemDto()
            {
                Bimestre = compensacaoAusencia.Bimestre,
                AtividadeNome = compensacaoAusencia.Nome,
                Alunos = new List<string>()
            };
    }
}
