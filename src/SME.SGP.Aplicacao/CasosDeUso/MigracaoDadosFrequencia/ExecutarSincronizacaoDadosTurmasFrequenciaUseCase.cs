using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoDadosTurmasFrequenciaUseCase : AbstractUseCase, IExecutarSincronizacaoDadosTurmasFrequenciaUseCase
    {
        public ExecutarSincronizacaoDadosTurmasFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroMigracaoFrequenciaTurmaDto>();

            var dadosAulas = await ObterAulasComFrequenciaNaTurma(filtro.TurmaCodigo);

            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(filtro.TurmaCodigo));

            foreach (var dadosAula in dadosAulas)
            {
                var codigosAlunosFitlrados = new List<string>();
                foreach (var aluno in alunosDaTurma.Where(a => a.DeveMostrarNaChamada(dadosAula.DataAula)).OrderBy(c => c.NomeAluno))
                {
                    if (aluno.EstaInativo(dadosAula.DataAula) ||
                        (aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo &&
                         aluno.DataSituacao > dadosAula.DataAula))
                        continue;

                    if (dadosAula.DataAula < aluno.DataMatricula.Date)
                        continue;

                    if (aluno.EstaInativo(dadosAula.DataAula) && aluno.DataSituacao < dadosAula.DataAula)
                        continue;
                    codigosAlunosFitlrados.Add(aluno.CodigoAluno);
                }
                var migracaoFrequenciaTurmaAula = new MigracaoFrequenciaTurmaAulaDto(filtro.TurmaCodigo, dadosAula.AulaId, dadosAula.QuantidadeAulas, dadosAula.RegistroFrequenciaId, codigosAlunosFitlrados.ToArray());

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.CarregarDadosAlunosFrequenciaMigracao, migracaoFrequenciaTurmaAula, Guid.NewGuid(), null));
            }

            return true;
        }

        private async Task<IEnumerable<AulaComFrequenciaNaDataDto>> ObterAulasComFrequenciaNaTurma(string turmaCodigo)
            => await mediator.Send(new ObterAulasComFrequenciaPorTurmaCodigoQuery(turmaCodigo));
    }
}
