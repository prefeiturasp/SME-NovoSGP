using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciaDaAulaCommandHandler : IRequestHandler<ExcluirFrequenciaDaAulaCommand, bool>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ExcluirFrequenciaDaAulaCommandHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<bool> Handle(ExcluirFrequenciaDaAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioFrequencia.ExcluirFrequenciaAula(request.AulaId);
            return true;
        }
    }
}
