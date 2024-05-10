using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase : AbstractUseCase, IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase
    {
        private readonly IRepositorioUeConsulta repositorioUeConsulta;

        public ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase(IMediator mediator, IRepositorioUeConsulta repositorioUeConsulta) : base(mediator)
        {
            this.repositorioUeConsulta = repositorioUeConsulta;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroConsolidacaoConselhoClasseAlunoPorAnoDto>();

            try
            {
                var ues = repositorioUeConsulta.ObterTodas();

                foreach (var ue in ues)
                {
                    var mensagemPorUe = new MensagemConsolidarTurmaConselhoClasseAlunoPorUeDto(ue.CodigoUe, filtro.AnoLetivo);

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeTratar, JsonConvert.SerializeObject(mensagemPorUe), mensagemRabbit.CodigoCorrelacao, null));
                }
                return true;
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a consolidacao turma conselho classe aluno anos anteriores.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }
    }
}
