using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoDadosAulasFrequenciaUseCase : AbstractUseCase, IExecutarSincronizacaoDadosAulasFrequenciaUseCase
    {
        public ExecutarSincronizacaoDadosAulasFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosAula = mensagemRabbit.ObterObjetoMensagem<FiltroMigracaoFrequenciaAulasDto>();
            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(dadosAula.TurmaCodigo));
            var codigosAlunosFitlrados = new List<string>();
            foreach (var aluno in alunosDaTurma.Where(a => a.DeveMostrarNaChamada(dadosAula.DataAula)).OrderBy(c => c.NomeAluno))
            {

                if (aluno.EstaInativo(dadosAula.DataAula) ||
                    (aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo && aluno.DataSituacao > dadosAula.DataAula))
                    continue;

                if (dadosAula.DataAula < aluno.DataMatricula.Date)
                    continue;

                if (aluno.EstaInativo(dadosAula.DataAula) && aluno.DataSituacao < dadosAula.DataAula)
                    continue;
                codigosAlunosFitlrados.Add(aluno.CodigoAluno);
            }
            var migracaoFrequenciaTurmaAula = new MigracaoFrequenciaTurmaAulaDto(dadosAula.TurmaCodigo, dadosAula.AulaId, dadosAula.QuantidadeAula, dadosAula.RegistroFrequenciaId, codigosAlunosFitlrados.ToArray());

            try
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarDadosAlunosFrequenciaMigracao, migracaoFrequenciaTurmaAula, Guid.NewGuid(), null));
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            return true;
        }
    }
}
