using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosEscolaAquiPorDreUeTurmaMesQueryHandler : IRequestHandler<ObterEventosEscolaAquiPorDreUeTurmaMesQuery, IEnumerable<EventoEADto>>
    {
        private readonly IRepositorioEventoConsulta repositorioEvento;

        public ObterEventosEscolaAquiPorDreUeTurmaMesQueryHandler(IRepositorioEventoConsulta repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public Task<IEnumerable<EventoEADto>> Handle(ObterEventosEscolaAquiPorDreUeTurmaMesQuery request, CancellationToken cancellationToken)
        {
            return repositorioEvento.ObterEventosEscolaAquiPorDreUeTurmaMes(request.Dre_id, request.Ue_id, request.Turma_id, request.ModalidadeCalendario, request.MesAno);
        }

    }

}