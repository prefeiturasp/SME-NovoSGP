using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarExclusaoAulaComFrequenciaCommandHandler : IRequestHandler<NotificarExclusaoAulaComFrequenciaCommand, bool>
    {
        private readonly IServicoFila servicoFila;

        public NotificarExclusaoAulaComFrequenciaCommandHandler(IServicoFila servicoFila)
        {
            this.servicoFila = servicoFila ?? throw new System.ArgumentNullException(nameof(servicoFila));
        }

        public Task<bool> Handle(NotificarExclusaoAulaComFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var fila = new PublicaFilaSgpDto(RotasRabbit.RotaNotificacaoExclusaoAulasComFrequencia,
                                             new NotificarExclusaoAulasComFrequenciaDto(request.Turma,
                                                                                        request.DatasAulas),
                                             Guid.NewGuid(), 
                                             null);

            servicoFila.PublicaFilaWorkerSgp(fila);
            return Task.FromResult(true);
        }
    }
}
