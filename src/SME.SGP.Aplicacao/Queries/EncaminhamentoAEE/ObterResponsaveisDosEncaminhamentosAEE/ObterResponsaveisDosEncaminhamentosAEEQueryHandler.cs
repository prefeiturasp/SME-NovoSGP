using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisDosEncaminhamentosAEEQueryHandler : IRequestHandler<ObterResponsaveisDosEncaminhamentosAEEQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public ObterResponsaveisDosEncaminhamentosAEEQueryHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterResponsaveisDosEncaminhamentosAEEQuery request, CancellationToken cancellationToken)
            => await repositorioEncaminhamentoAEE.ObterResponsaveis(request.DreId,
                                                                    request.UeId,
                                                                    request.TurmaId,
                                                                    request.AlunoCodigo,
                                                                    request.AnoLetivo,
                                                                    (int?)request.Situacao);
    }
}
