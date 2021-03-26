using MediatR;
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
        private readonly IMediator mediator;

        public ConsultasFechamentoAluno(IRepositorioFechamentoAluno repositorio,                                         
                                        IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                        IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<FechamentoAlunoCompletoDto> ObterAnotacaoAluno(string codigoAluno, long fechamentoId, string codigoTurma, int anoLetivo)
        {
            var consultaFechamentoAluno = await repositorio.ObterFechamentoAluno(fechamentoId, codigoAluno);
            var dadosAlunos = await mediator.Send(new ObterDadosAlunosQuery(codigoTurma, anoLetivo));
            if (dadosAlunos == null || !dadosAlunos.Any(da => da.CodigoEOL.Equals(codigoAluno)))
                throw new NegocioException($"Não foram localizados dados do aluno {codigoAluno} na turma {codigoTurma} no EOL para o ano letivo {anoLetivo}");

            var dadosAluno = dadosAlunos.First(da => da.CodigoEOL.Equals(codigoAluno));

            dadosAluno.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(codigoAluno, anoLetivo));

            var anotacaoAluno = consultaFechamentoAluno;
            var anotacaoDto = anotacaoAluno == null ?
                            new FechamentoAlunoCompletoDto() { Aluno = dadosAluno } :
                            MapearParaDto(anotacaoAluno, dadosAluno);

            return anotacaoDto;
        }

        public async Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacaoAlunoParaConselhoAsync(string alunoCodigo, string[] turmasCodigos)
        {
            var anotacoesDto = await repositorio.ObterAnotacoesTurmaAlunoBimestreAsync(alunoCodigo, turmasCodigos);
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