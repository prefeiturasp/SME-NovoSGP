using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasAlunosSyncUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmasAlunosSyncUseCase
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ConciliacaoFrequenciaTurmasAlunosSyncUseCase(IMediator mediator, IRepositorioTurmaConsulta repositorioTurmaConsulta) : base(mediator)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new System.ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {

            var filtro = mensagem.ObterObjetoMensagem<FiltroCalculoFrequenciaDataRereferenciaDto>();

            var turmasComponentesDoAnoLetivo = await repositorioTurmaConsulta.ObterTurmasComponentesPorAnoLetivo(filtro.DataReferencia);

            if (turmasComponentesDoAnoLetivo != null && turmasComponentesDoAnoLetivo.Any())
            {
                var turmasComponentesDoAnoLetivoAgrupados = turmasComponentesDoAnoLetivo.GroupBy(a => a.TurmaCodigo).ToList();
                
                foreach (var turmaComponenteDoAnoLetivoAgrupado in turmasComponentesDoAnoLetivoAgrupados)
                {
                    var componentesDaTurma = turmaComponenteDoAnoLetivoAgrupado.Select(a => a.ComponenteCurricularId).Distinct().ToArray();
                    var turmaCodigo = turmaComponenteDoAnoLetivoAgrupado.Key;
                    var periodosDaTurma = turmaComponenteDoAnoLetivoAgrupado.Select(a => a.DataReferencia).Distinct().ToArray();

                    var mensagemParaEnviar = new TurmaComponentesParaCalculoFrequenciaDto(turmaCodigo, componentesDaTurma, periodosDaTurma);

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConciliacaoFrequenciaTurmasAlunosBuscar, mensagemParaEnviar, Guid.NewGuid(), null));
                }


            } else
            {
                SentrySdk.CaptureException(new System.Exception("Não foi possível obter as turmas e componentes para o cálculo de frequencia."));
            }

            return true;
        }
    }


}
