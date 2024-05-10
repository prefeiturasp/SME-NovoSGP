using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEstruturaInstitucionalVigenteQueryHandler : IRequestHandler<ObterEstruturaInstitucionalVigenteQuery, EstruturaInstitucionalRetornoEolDTO>
    {
        private readonly IMediator mediator;

        public ObterEstruturaInstitucionalVigenteQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<EstruturaInstitucionalRetornoEolDTO> Handle(ObterEstruturaInstitucionalVigenteQuery request, CancellationToken cancellationToken)
        {
            EstruturaInstitucionalRetornoEolDTO resultado = default;

            var codigosDres = await mediator.Send(ObterCodigosDresQuery.Instance);
            
            if (codigosDres.NaoEhNulo() && codigosDres.Length > 0)
            {
                resultado = new EstruturaInstitucionalRetornoEolDTO();
                foreach (var item in codigosDres)
                {
                    var retorno = await mediator.Send(new ObterEstruturaInstitucionalVigentePorDreQuery(item));

                    if (retorno.NaoEhNulo())
                        resultado.Dres.AddRange(retorno.Dres);
                }
            }

            return resultado;
        }
    }
}