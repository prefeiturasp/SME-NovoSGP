using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutarSincronizacaoDevolutivasPorTurmaInfantilSyncUseCase : AbstractUseCase, IExecutarSincronizacaoDevolutivasPorTurmaInfantilSyncUseCase
    {
        public ExecutarSincronizacaoDevolutivasPorTurmaInfantilSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var codigosDre = await mediator.Send(new ObterCodigosDresQuery());

            // Obter turmas devolutivas

            // validar se existem turmas

            // Para cada turma publicar mensagem na fila

            if (codigosDre == null || !codigosDre.Any())
            {
                throw new NegocioException("Não foi possível localizar as Dres no EOL para a sincronização instituicional");
            }

            foreach (var codigoDre in codigosDre)
            {
                try
                {
                    var publicarTratamentoTurma = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaDevolutivasPorTurmaInfantilTratar, codigoDre, param.CodigoCorrelacao, null, fila: RotasRabbit.SincronizaDevolutivasPorTurmaInfantilTratar));
                    if (!publicarTratamentoTurma)
                    {
                        var mensagem = $"Não foi possível inserir a Turma : {publicarTratamentoTurma} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }

            return true;
        }
    }
}
