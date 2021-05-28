using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoAluno : IConsultasFechamentoAluno
    {
        private readonly IRepositorioFechamentoAluno repositorio;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IServicoEol servicoEOL;

        public ConsultasFechamentoAluno(IRepositorioFechamentoAluno repositorio
                                            , IServicoEol servicoEOL, IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<FechamentoAlunoCompletoDto> ObterAnotacaoAluno(string codigoAluno, long fechamentoId, string codigoTurma, int anoLetivo)
        {
            var consultaFechamentoAluno = repositorio.ObterFechamentoAluno(fechamentoId, codigoAluno);
            var dadosAlunos = await servicoEOL.ObterDadosAluno(codigoAluno, anoLetivo);
            if (dadosAlunos == null || !dadosAlunos.Any(c => c.CodigoTurma.ToString() == codigoTurma))
                throw new NegocioException($"Não foram localizados dados do aluno {codigoAluno} na turma {codigoTurma} no EOL para o ano letivo {anoLetivo}");

            var dadosAluno = (AlunoDadosBasicosDto)dadosAlunos.FirstOrDefault(c => c.CodigoTurma.ToString() == codigoTurma);

            var anotacaoAluno = await consultaFechamentoAluno;
            var anotacaoDto = anotacaoAluno == null ?
                            new FechamentoAlunoCompletoDto() { Aluno = dadosAluno } :
                            MapearParaDto(anotacaoAluno, dadosAluno);

            return anotacaoDto;
        }

        public async Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacaoAlunoParaConselhoAsync(string alunoCodigo, long fechamentoTurmaId)
        {
            var anotacoesDto = await repositorio.ObterAnotacoesTurmaAlunoBimestreAsync(alunoCodigo, fechamentoTurmaId);
            if (anotacoesDto == null || !anotacoesDto.Any())
                return default;

            var disciplinasIds = anotacoesDto.Select(a => long.Parse(a.DisciplinaId)).ToArray();

            var disciplinas = await repositorioComponenteCurricular.ObterDisciplinasPorIds(disciplinasIds);

            foreach (var anotacao in anotacoesDto)
            {
                var disciplina = disciplinas.FirstOrDefault(a => a.CodigoComponenteCurricular == long.Parse(anotacao.DisciplinaId));
                if (disciplina != null)
                    anotacao.Disciplina = disciplina.Nome;
            }

            return anotacoesDto;
        }

        public async Task<FechamentoAluno> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno)
            => await repositorio.ObterFechamentoAluno(fechamentoId, codigoAluno);

        private FechamentoAlunoCompletoDto MapearParaDto(FechamentoAluno anotacaoAluno, AlunoDadosBasicosDto dadosAluno)
        {
            if (anotacaoAluno == null)
                return null;
            else
            {
                var fechamentoAluno = new FechamentoAlunoCompletoDto()
                {
                    Id = anotacaoAluno.Id,
                    Anotacao = anotacaoAluno.Anotacao,
                    Aluno = dadosAluno,
                };

                if (!string.IsNullOrEmpty(anotacaoAluno.Anotacao))
                {
                    fechamentoAluno.CriadoEm = anotacaoAluno.CriadoEm;
                    fechamentoAluno.CriadoPor = anotacaoAluno.CriadoPor;
                    fechamentoAluno.CriadoRF = anotacaoAluno.CriadoRF;
                    fechamentoAluno.AlteradoEm = anotacaoAluno.AlteradoEm;
                    fechamentoAluno.AlteradoPor = anotacaoAluno.AlteradoPor;
                    fechamentoAluno.AlteradoRF = anotacaoAluno.AlteradoRF;
                }

                return fechamentoAluno;
            }
        }
    }
}