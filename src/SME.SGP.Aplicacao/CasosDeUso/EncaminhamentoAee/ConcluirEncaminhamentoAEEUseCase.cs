using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConcluirEncaminhamentoAEEUseCase : AbstractUseCase, IConcluirEncaminhamentoAEEUseCase
    {
        public ConcluirEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoId)
        {
            var encaminhamento = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoId));

            encaminhamento.Situacao = VerificaEstudanteNecessitaAEE(encaminhamento) ? SituacaoAEE.Deferido : SituacaoAEE.Indeferido;

            await mediator.Send(new SalvarEncaminhamentoAEECommand(encaminhamento));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            await mediator.Send(new ExecutaNotificacaoConclusaoEncaminhamentoAEECommand(encaminhamento.Id, usuarioLogado.CodigoRf, usuarioLogado.Nome));

            await ExcluirPendenciasEncaminhamentoAEE(encaminhamentoId);

            return true;
        }

        private async Task ExcluirPendenciasEncaminhamentoAEE(long encaminhamentoId)
        {
            var pendenciasEncaminhamentoAEE = await mediator.Send(new ObterPendenciasDoEncaminhamentoAEEPorIdQuery(encaminhamentoId));
            if (pendenciasEncaminhamentoAEE != null || !pendenciasEncaminhamentoAEE.Any())
            {
                foreach (var pendenciaEncaminhamentoAEE in pendenciasEncaminhamentoAEE)
                {
                    await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendenciaEncaminhamentoAEE.PendenciaId));
                }
            }
        }

        private bool VerificaEstudanteNecessitaAEE(EncaminhamentoAEE encaminhamento)
        {
            var secao = encaminhamento.Secoes.FirstOrDefault(c => c.SecaoEncaminhamentoAEE.Etapa == 3);
            if (secao == null)
                throw new NegocioException("Não localizado a seção da Etapa 3 no encaminhamento AEE");

            var questao = secao.Questoes.FirstOrDefault(c => c.Questao.Ordem == 2 && c.Questao.Tipo == TipoQuestao.Radio);
            if (questao == null)
                throw new NegocioException("Questão não localizada para identificar se o estudante/criaça necessita do Atendimento Educacional Especializado");

            var respostaQuestao = questao.Respostas.FirstOrDefault();
            if (respostaQuestao == null)
                throw new NegocioException("Não localizada resposta para identificar se o estudante/criaça necessita do Atendimento Educacional Especializado");

            return respostaQuestao.Resposta.Nome == "Sim";
        }
    }
}
