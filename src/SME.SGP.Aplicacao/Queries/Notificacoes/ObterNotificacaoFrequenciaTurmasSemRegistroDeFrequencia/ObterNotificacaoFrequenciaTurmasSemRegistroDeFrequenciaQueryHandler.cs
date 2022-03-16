using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQueryHandler : IRequestHandler<ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQuery, IEnumerable<RegistroFrequenciaFaltanteDto>>
    {
        private readonly IRepositorioNotificacaoFrequenciaConsulta repositorioNotificacaoFrequencia;

        public ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQueryHandler(IRepositorioNotificacaoFrequenciaConsulta repositorio)
        {
            this.repositorioNotificacaoFrequencia = repositorio;
        }

        public async Task<IEnumerable<RegistroFrequenciaFaltanteDto>> Handle(ObterNotificacaoFrequenciaTurmasSemRegistroDeFrequenciaQuery request, CancellationToken cancellationToken)
            => await repositorioNotificacaoFrequencia.ObterTurmasSemRegistroDeFrequencia(request.Tipo);
    }
}

