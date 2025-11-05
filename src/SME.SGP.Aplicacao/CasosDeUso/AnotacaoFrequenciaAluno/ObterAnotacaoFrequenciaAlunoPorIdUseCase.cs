using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoPorIdUseCase : AbstractUseCase, IObterAnotacaoFrequenciaAlunoPorIdUseCase
    {
        public ObterAnotacaoFrequenciaAlunoPorIdUseCase(IMediator mediator) : base(mediator) { }

        public async Task<AnotacaoFrequenciaAlunoCompletoDto> Executar(long id)
        {
            var anotacao = await mediator.Send(new ObterAnotacaoFrequenciaAlunoPorIdQuery(id));

            if (anotacao.EhNulo() || anotacao.Excluido)
                throw new NegocioException("Anotação não encontrada!");

            MotivoAusencia motivoAusencia = null;

            if (anotacao.MotivoAusenciaId.HasValue)
                motivoAusencia = await mediator.Send(new ObterMotivoAusenciaPorIdQuery(anotacao.MotivoAusenciaId.Value));

            var aula = await mediator.Send(new ObterAulaPorIdQuery(anotacao.AulaId));

            if (aula.EhNulo())
                throw new NegocioException("Aula vinculada a anotação não encontrada");

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aula.TurmaId));

            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turma.CodigoTurma));

            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException("Alunos da turma não encontrado");

            var aluno = alunos.FirstOrDefault(a => a.CodigoAluno.Equals(anotacao.CodigoAluno));

            if (aluno.EhNulo())
                throw new NegocioException("Aluno não encontrado");

            var frequencia = await mediator.Send(new ObterFrequenciaGeralAlunoQuery(aluno.CodigoAluno, turma.CodigoTurma));

            return await MapearParaDto(anotacao, motivoAusencia, aluno, turma.AnoLetivo, frequencia);
        }

        private async Task<AnotacaoFrequenciaAlunoCompletoDto> MapearParaDto(Dominio.AnotacaoFrequenciaAluno anotacao, Dominio.MotivoAusencia motivoAusencia, AlunoPorTurmaResposta aluno, int anoLetivo, double frequencia)
        {
            return new AnotacaoFrequenciaAlunoCompletoDto()
            {
                Aluno = new AlunoDadosBasicosDto()
                {
                    CelularResponsavel = aluno.CelularResponsavel,
                    CodigoEOL = aluno.CodigoAluno,
                    DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                    DataNascimento = aluno.DataNascimento,
                    DataSituacao = aluno.DataSituacao,
                    Nome = aluno.NomeValido(),
                    NomeResponsavel = aluno.NomeResponsavel,
                    NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                    Situacao = aluno.SituacaoMatricula,
                    SituacaoCodigo = aluno.CodigoSituacaoMatricula,
                    TipoResponsavel = ObterTipoResponsavel(aluno.TipoResponsavel),
                    EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, anoLetivo)),
                    Frequencia=frequencia,
                },
                Anotacao = anotacao.Anotacao,
                Auditoria = (AuditoriaDto)anotacao,
                AulaId = anotacao.AulaId,
                CodigoAluno = anotacao.CodigoAluno,
                Id = anotacao.Id,
                MotivoAusencia = motivoAusencia.EhNulo() ? null :
                    new MotivoAusenciaDto()
                    {
                        Id = motivoAusencia.Id,
                        Descricao = motivoAusencia.Descricao
                    },
                MotivoAusenciaId = anotacao.MotivoAusenciaId.NaoEhNulo() ? anotacao.MotivoAusenciaId.Value : 0,
            };
        }

        private string ObterTipoResponsavel(string tipoResponsavel)
        {
            switch (tipoResponsavel)
            {
                case "1":
                    {
                        return TipoResponsavel.Filiacao1.Name();
                    }
                case "2":
                    {
                        return TipoResponsavel.Filiacao2.Name();
                    }
                case "3":
                    {
                        return TipoResponsavel.ResponsavelLegal.Name();
                    }
                case "4":
                    {
                        return TipoResponsavel.ProprioEstudante.Name();
                    }
            }
            return TipoResponsavel.Filiacao1.ToString();
        }
    }
}
