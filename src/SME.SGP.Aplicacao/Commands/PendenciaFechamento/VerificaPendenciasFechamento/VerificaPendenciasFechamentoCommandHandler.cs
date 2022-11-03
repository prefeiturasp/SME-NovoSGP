using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class VerificaPendenciasFechamentoCommandHandler : AsyncRequestHandler<VerificaPendenciasFechamentoCommand>
    {
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IMediator mediator;

        public VerificaPendenciasFechamentoCommandHandler(IServicoPendenciaFechamento servicoPendenciaFechamento,
                                                          IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                          IMediator mediator)
        {
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(VerificaPendenciasFechamentoCommand request, CancellationToken cancellationToken)
        {
            // Verifica existencia de pendencia em aberto
            if (!servicoPendenciaFechamento.VerificarPendenciasEmAbertoPorFechamento(request.FechamentoId))
            {
                var fechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina.ObterPorId(request.FechamentoId);
                // Atualiza situação do fechamento
                fechamentoTurmaDisciplina.Situacao = SituacaoFechamento.ProcessadoComSucesso;
                repositorioFechamentoTurmaDisciplina.Salvar(fechamentoTurmaDisciplina);
                
                var consolidacaoTurma = new ConsolidacaoTurmaDto(request.TurmaId, request.Bimestre);
                var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync, mensagemParaPublicar, Guid.NewGuid(), null));
            }
        }
    }
}
