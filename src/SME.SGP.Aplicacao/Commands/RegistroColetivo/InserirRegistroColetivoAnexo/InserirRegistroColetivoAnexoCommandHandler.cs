using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroColetivoAnexoCommandHandler : IRequestHandler<InserirRegistroColetivoAnexoCommand, bool>
    {
        private readonly IRepositorioRegistroColetivoAnexo repositorio;
        private readonly IMediator mediator;

        public InserirRegistroColetivoAnexoCommandHandler(IRepositorioRegistroColetivoAnexo repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirRegistroColetivoAnexoCommand request, CancellationToken cancellationToken)
        {
            var codigosArquivos = request.Anexos.Select(anexo => anexo.AnexoId).ToArray();
            var arquivos = await mediator.Send(new ObterArquivosPorCodigosQuery(codigosArquivos));
            var registros = ObterRegistro(request.RegistroColetivoId, arquivos);

            foreach(var registro in registros)
            {
                await repositorio.SalvarAsync(registro);
            }

            return true;
        }

        private IEnumerable<RegistroColetivoAnexo> ObterRegistro(long registroColetivoId, IEnumerable<Arquivo> arquivos)
        {
            foreach (var arquivo in arquivos)
            {
                yield return new RegistroColetivoAnexo()
                {
                    RegistroColetivoId = registroColetivoId,
                    ArquivoId = arquivo.Id
                };
            }
        }
    }
}
