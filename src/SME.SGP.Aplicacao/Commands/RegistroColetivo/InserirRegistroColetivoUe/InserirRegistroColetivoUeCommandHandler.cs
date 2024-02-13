using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroColetivoUeCommandHandler : IRequestHandler<InserirRegistroColetivoUeCommand, bool>
    {
        private readonly IRepositorioRegistroColetivoUe repositorio;

        public InserirRegistroColetivoUeCommandHandler(IRepositorioRegistroColetivoUe repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(InserirRegistroColetivoUeCommand request, CancellationToken cancellationToken)
        {
            var registros = ObterRegistro(request.RegistroColetivoId, request.UeIds);

            foreach(var registro in registros)
            {
                await repositorio.SalvarAsync(registro);
            }
             
            return true;
        }

        private IEnumerable<RegistroColetivoUe> ObterRegistro(long registroColetivoId, IEnumerable<long> ueIds)
        {
            foreach (var idUe in ueIds)
            {
                yield return new RegistroColetivoUe()
                {
                    RegistroColetivoId = registroColetivoId,
                    UeId = idUe
                };
            }
        }
    }
}
