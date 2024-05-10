using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaGeralUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaGeralUseCase
    {
        private const int QUANTIDADE_REGISTROS_POR_PAGINA = 1000;

        public ExecutarConsolidacaoTurmaGeralUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoTurmaDto>();

            var turmasEModalidadesParaConsolidar = filtro.TurmaCodigo.Equals("-1") ?
                await mediator.Send(new ObterTurmasConsolidacaoFechamentoGeralPorAnoLetivoTiposEscolaQuery(filtro.AnoLetivo, filtro.Pagina, QUANTIDADE_REGISTROS_POR_PAGINA, null)) :
                await mediator.Send(new ObterTurmasConsolidacaoFechamentoGeralQuery(filtro.TurmaCodigo));

            if (turmasEModalidadesParaConsolidar.EhNulo() || !turmasEModalidadesParaConsolidar.Any())
                return true;

            foreach (var turmaEModalidadesParaConsolidar in turmasEModalidadesParaConsolidar)
            {
                var guidParaCorrelacao = Guid.NewGuid();

                var dto = new ConsolidacaoTurmaDto() { TurmaId = turmaEModalidadesParaConsolidar.TurmaId };

                if (filtro.Bimestre.HasValue)
                    await PublicaNaFila(dto, filtro.Bimestre.Value, guidParaCorrelacao);
                else
                    await PublicaBimestres(dto, guidParaCorrelacao, turmaEModalidadesParaConsolidar.Modalidade == Modalidade.EJA ? 2 : 4);
            }

            if (filtro.TurmaCodigo.Equals("-1"))
            {
                var dto = new FiltroConsolidacaoTurmaDto("-1", filtro.Bimestre, filtro.Pagina + 1);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaSync, dto, Guid.NewGuid(), null));
            }

            return true;
        }

        private async Task PublicaBimestres(ConsolidacaoTurmaDto mensagem, Guid guidParaCorrelacao, int bimestres)
        {
            for (var i = 1; i <= bimestres; i++)
                await PublicaNaFila(mensagem, i, guidParaCorrelacao);

            await PublicaNaFila(mensagem, null, guidParaCorrelacao);
        }

        private async Task PublicaNaFila(ConsolidacaoTurmaDto mensagem, int? bimestre, Guid codigoCorrelacao)
        {
            mensagem.Bimestre = bimestre;
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaTratar, mensagem, codigoCorrelacao, null));
        }
    }
}

