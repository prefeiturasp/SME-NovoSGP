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
        private readonly IServicoEOL servicoEOL;

        public ConsultasFechamentoAluno(IRepositorioFechamentoAluno repositorio
                                            , IServicoEOL servicoEOL)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
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

        public async Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacaoAlunoParaConselhoAsync(string alunoCodigo, string turmaCodigo, int bimestre, bool EhFinal)
        {
            var anotacoesDto = await repositorio.ObterAnotacoesTurmaAlunoBimestreAsync(alunoCodigo, turmaCodigo, bimestre, EhFinal);
            if (anotacoesDto == null || !anotacoesDto.Any())
                return default;

            var disciplinasIds = anotacoesDto.Select(a => long.Parse(a.DisciplinaId)).ToArray();

            var disciplinas = await servicoEOL.ObterDisciplinasPorIdsAsync(disciplinasIds);

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
            => anotacaoAluno == null ? null :
            new FechamentoAlunoCompletoDto()
            {
                Id = anotacaoAluno.Id,
                Anotacao = anotacaoAluno.Anotacao,
                Aluno = dadosAluno,

                CriadoEm = anotacaoAluno.CriadoEm,
                CriadoPor = anotacaoAluno.CriadoPor,
                CriadoRF = anotacaoAluno.CriadoRF,
                AlteradoEm = anotacaoAluno.AlteradoEm,
                AlteradoPor = anotacaoAluno.AlteradoPor,
                AlteradoRF = anotacaoAluno.AlteradoRF
            };
    }
}