using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.UE.ObterUEsPorModalidadeCalendario
{
    public class ObterUEsPorModalidadeCalendarioQueryHandler : IRequestHandler<ObterUEsPorModalidadeCalendarioQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUEsPorModalidadeCalendarioQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUEsPorModalidadeCalendarioQuery request, CancellationToken cancellationToken)
        {
            var modalidades = ObterTurmas(request.ModalidadeTipoCalendario);

            return await repositorioUe.ObterUesPorModalidade(modalidades);
        }

        private int[] ObterTurmas(ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            switch (modalidadeTipoCalendario)
            {
                case ModalidadeTipoCalendario.FundamentalMedio:
                    return new int[] { (int)Modalidade.Fundamental, (int)Modalidade.Medio };
                case ModalidadeTipoCalendario.EJA:
                    return new int[] { (int)Modalidade.EJA };
                case ModalidadeTipoCalendario.Infantil:
                    return new int[] { (int)Modalidade.Infantil };
                default:
                    return null;
            }
        }
    }
}
