using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroAcaoSecaoCommandHandler : IRequestHandler<AlterarRegistroAcaoSecaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioRegistroAcaoBuscaAtivaSecao repositorioRegistroAcaoSecao;

        public AlterarRegistroAcaoSecaoCommandHandler(IRepositorioRegistroAcaoBuscaAtivaSecao repositorioRegistroAcaoSecao)
        {
            this.repositorioRegistroAcaoSecao = repositorioRegistroAcaoSecao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcaoSecao));
        }

        public async Task<AuditoriaDto> Handle(AlterarRegistroAcaoSecaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioRegistroAcaoSecao.SalvarAsync(request.Secao);

            return (AuditoriaDto)request.Secao;
        }

    }
}
