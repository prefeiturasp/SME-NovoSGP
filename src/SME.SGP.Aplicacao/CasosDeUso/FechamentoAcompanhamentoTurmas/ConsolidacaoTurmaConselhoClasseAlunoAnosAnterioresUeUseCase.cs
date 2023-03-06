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
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase : AbstractUseCase, IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase
    {
        private readonly IRepositorioUeConsulta repositorioUeConsulta;

        public ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase(IMediator mediator, IRepositorioUeConsulta repositorioUeConsulta) : base(mediator)
        {
            this.repositorioUeConsulta = repositorioUeConsulta;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidarTurmaConselhoClasseAlunoPorUeDto>();

            try
            {
                var turmas = await repositorioUeConsulta.ObterTurmasPorCodigoUe(filtro.CodigoUe, filtro.AnoLetivo);

                foreach (var turma in turmas)
                {
                    
                    var mensagemPorTurma = new MensagemConsolidarTurmaConselhoClasseAlunoPorTurmaDto(turma.Id);

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaTratar, JsonConvert.SerializeObject(mensagemPorTurma), mensagemRabbit.CodigoCorrelacao, null));
                }
                return true;
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a consolidacao turma conselho classe aluno anos anteriores por ue.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }
    }
}
