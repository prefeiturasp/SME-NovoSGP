using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitora
{
    public class PainelEducacionalFluenciaLeitoraQueryHandler : IRequestHandler<PainelEducacionalFluenciaLeitoraQuery, IEnumerable<PainelEducacionalFluenciaLeitoraDto>>
    {
        private readonly IRepositorioPainelEducacionalFluenciaLeitoraConsulta repositorioPainelEducacionalFluenciaLeitoraConsulta;
        public PainelEducacionalFluenciaLeitoraQueryHandler(IRepositorioPainelEducacionalFluenciaLeitoraConsulta repositorioPainelEducacionalFluenciaLeitoraConsulta)
        {
            this.repositorioPainelEducacionalFluenciaLeitoraConsulta = repositorioPainelEducacionalFluenciaLeitoraConsulta;
        }


        public async Task<IEnumerable<PainelEducacionalFluenciaLeitoraDto>> Handle(PainelEducacionalFluenciaLeitoraQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalFluenciaLeitoraConsulta.ObterFluenciaLeitora(request.Periodo, request.AnoLetivo, request.CodigoDre, request.CodigoUe);

            return registros;
        }
    }
}
