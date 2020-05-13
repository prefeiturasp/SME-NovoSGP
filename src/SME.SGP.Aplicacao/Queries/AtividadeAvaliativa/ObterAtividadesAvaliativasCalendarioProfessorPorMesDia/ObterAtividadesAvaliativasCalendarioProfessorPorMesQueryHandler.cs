using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQueryHandler : IRequestHandler<ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQuery, IEnumerable<AtividadeAvaliativa>>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }
        public async Task<IEnumerable<AtividadeAvaliativa>> Handle(ObterAtividadesAvaliativasCalendarioProfessorPorMesDiaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativa.ObterAtividadesCalendarioProfessorPorMesDia(request.DreCodigo, request.UeCodigo, request.TurmaCodigo, request.CodigoRf, request.DataReferencia);
        }
    }
}
