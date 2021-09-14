using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaGeralUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaGeralUseCase
    {
        public ExecutarConsolidacaoTurmaGeralUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoTurmaDto>();

            var turmasEModalidadesParaConsolidar = await mediator.Send(new ObterTurmasConsolidacaoFechamentoGeralQuery(filtro.TurmaCodigo));

            var guidParaCorrelacao = Guid.NewGuid();
            foreach (var turmaEModalidadesParaConsolidar in turmasEModalidadesParaConsolidar)
            {
                var dto = new ConsolidacaoTurmaDto() { TurmaId = turmaEModalidadesParaConsolidar.TurmaId };

                if (filtro.Bimestre.HasValue)
                    await PublicaNaFila(dto, filtro.Bimestre.Value, guidParaCorrelacao);
                else
                    await PublicaBimestres(dto, guidParaCorrelacao, turmaEModalidadesParaConsolidar.Modalidade == Modalidade.EJA ? 2 : 4);
            }

            return true;
        }

        private async Task PublicaBimestres(ConsolidacaoTurmaDto mensagem, Guid guidParaCorrelacao, int bimestres)
        {
            for(var i=1; i<=bimestres; i++)
                await PublicaNaFila(mensagem, i, guidParaCorrelacao);
        }

        private async Task PublicaNaFila(ConsolidacaoTurmaDto mensagem, int bimestre, Guid codigoCorrelacao)
        {
            mensagem.Bimestre = bimestre;
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaTratar, mensagem, codigoCorrelacao, null));
        }
    }
}

