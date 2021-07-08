using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterMuralAvisoPorAulaIdQueryHandler : IRequestHandler<ObterMuralAvisoPorAulaIdQuery, IList<MuralAvisosRetornoDto>>
    {

        private readonly IRepositorioAviso _repositorioAviso;

        public ObterMuralAvisoPorAulaIdQueryHandler(IRepositorioAviso repositorioAviso)
        {
            _repositorioAviso = repositorioAviso;
        }

        public async Task<IList<MuralAvisosRetornoDto>> Handle(ObterMuralAvisoPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            var avisos = await _repositorioAviso.ObterPorAulaId(request.AulaId);
            return avisos.Select(x => new MuralAvisosRetornoDto(x.CriadoEm, x.Mensagem, "")).ToList();
        }
    }
}