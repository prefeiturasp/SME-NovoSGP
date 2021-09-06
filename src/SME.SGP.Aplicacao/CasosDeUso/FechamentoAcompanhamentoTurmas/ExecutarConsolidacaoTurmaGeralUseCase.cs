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

        public async Task Executar(string turmaCodigo, int? bimestre)
        {
            var turmasEModalidadesParaConsolidar = await mediator.Send(new ObterTurmasConsolidacaoFechamentoGeralQuery(turmaCodigo));

            var guidParaCorrelacao = Guid.NewGuid();
            foreach (var turmaEModalidadesParaConsolidar in turmasEModalidadesParaConsolidar)
            {
                var mensagem = new ConsolidacaoTurmaDto() { TurmaId = turmaEModalidadesParaConsolidar.TurmaId };

                if (bimestre.HasValue)
                    await PublicaNaFila(mensagem, bimestre.Value, guidParaCorrelacao);
                else
                    await PublicaBimestres(mensagem, guidParaCorrelacao, turmaEModalidadesParaConsolidar.Modalidade == Modalidade.EJA ? 2 : 4);
            }
        }

        private async Task PublicaBimestres(ConsolidacaoTurmaDto mensagem, Guid guidParaCorrelacao, int bimestres)
        {
            for(var i=0; i<=bimestres; i++)
                await PublicaNaFila(mensagem, i, guidParaCorrelacao);
        }

        private async Task PublicaNaFila(ConsolidacaoTurmaDto mensagem, int bimestre, Guid codigoCorrelacao)
        {
            mensagem.Bimestre = bimestre;
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaSync, mensagem, codigoCorrelacao, null));
        }
    }
}

