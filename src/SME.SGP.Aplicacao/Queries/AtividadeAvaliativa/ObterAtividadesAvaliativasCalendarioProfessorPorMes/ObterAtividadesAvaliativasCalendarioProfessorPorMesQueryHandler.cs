using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesAvaliativasCalendarioProfessorPorMesQueryHandler : IRequestHandler<ObterAtividadesAvaliativasCalendarioProfessorPorMesQuery, IEnumerable<AtividadeAvaliativa>>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ObterAtividadesAvaliativasCalendarioProfessorPorMesQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }
        public async Task<IEnumerable<AtividadeAvaliativa>> Handle(ObterAtividadesAvaliativasCalendarioProfessorPorMesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativa.ObterAtividadesCalendarioProfessorPorMes(request.DreCodigo, request.UeCodigo, request.Mes, request.AnoLetivo,request.TurmaCodigo);
        }
    }
}
