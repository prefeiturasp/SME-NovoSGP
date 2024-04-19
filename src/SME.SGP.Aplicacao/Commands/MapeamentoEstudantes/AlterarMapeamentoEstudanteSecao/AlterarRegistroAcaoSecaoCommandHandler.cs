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
    public class AlterarMapeamentoEstudanteSecaoCommandHandler : IRequestHandler<AlterarMapeamentoEstudanteSecaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioMapeamentoEstudanteSecao repositorioMapeamentoSecao;

        public AlterarMapeamentoEstudanteSecaoCommandHandler(IRepositorioMapeamentoEstudanteSecao repositorioMapeamentoSecao)
        {
            this.repositorioMapeamentoSecao = repositorioMapeamentoSecao ?? throw new ArgumentNullException(nameof(repositorioMapeamentoSecao));
        }

        public async Task<AuditoriaDto> Handle(AlterarMapeamentoEstudanteSecaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioMapeamentoSecao.SalvarAsync(request.Secao);

            return (AuditoriaDto)request.Secao;
        }

    }
}
