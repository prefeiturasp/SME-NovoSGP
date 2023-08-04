using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoUeConselhoClasseUseCase : AbstractUseCase, IExecutarConsolidacaoUeConselhoClasseUseCase
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ExecutarConsolidacaoUeConselhoClasseUseCase(IMediator mediator, IRepositorioTurmaConsulta repositorioUeConsulta) : base(mediator)
        {
            this.repositorioTurmaConsulta = repositorioUeConsulta;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidarTurmaConselhoClasseAlunoPorUeAnoDto>();
            try
            {
                var turmasBimestre = await repositorioTurmaConsulta.ObterTurmasComFechamentoTurmaPorUeId(filtro.UeId, filtro.AnoLetivo);
                foreach (var turma in turmasBimestre)
                {
                    var mensagemPorTurma = new ConsolidacaoTurmaDto(turma.TurmaId, turma.Bimestre == 0 ? null : turma.Bimestre);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseSync, JsonConvert.SerializeObject(mensagemPorTurma), mensagemRabbit.CodigoCorrelacao, null));
                }
                return true;
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a consolidacao turma conselho classe aluno por Ue/ano.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                throw;
            }
        }
    }
}
