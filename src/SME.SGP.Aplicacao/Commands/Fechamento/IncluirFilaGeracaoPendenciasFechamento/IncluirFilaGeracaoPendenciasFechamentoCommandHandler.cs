using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaGeracaoPendenciasFechamentoCommandHandler : IRequestHandler<IncluirFilaGeracaoPendenciasFechamentoCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaGeracaoPendenciasFechamentoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaGeracaoPendenciasFechamentoCommand request, CancellationToken cancellationToken)
        {
            var command = new GerarPendenciasFechamentoCommand(request.ComponenteCurricularId
                                                            , request.TurmaCodigo
                                                            , request.TurmaNome
                                                            , request.PeriodoEscolarInicio
                                                            , request.PeriodoEscolarFim
                                                            , request.Bimestre
                                                            , request.Usuario.Id
                                                            , request.Usuario.CodigoRf
                                                            , request.FechamentoTurmaDisciplinaId
                                                            , request.Justificativa
                                                            , request.CriadoRF
                                                            , request.ComponenteSemNota
                                                            , request.RegistraFrequencia);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaGeracaoPendenciasFechamento, command, Guid.NewGuid(), request.Usuario));
            SentrySdk.AddBreadcrumb($"Incluir fila Geração de Pendências do Fechamento", "RabbitMQ");

            return true;
        }
    }
}
