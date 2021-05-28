using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposFrequenciasUseCase : AbstractUseCase, IObterTiposFrequenciasUseCase
    {
        public ObterTiposFrequenciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TipoFrequenciaDto>> Executar(TipoFrequenciaFiltroDto filtro)
        {
            var retorno = Enum.GetValues(typeof(TipoFrequencia))
                        .Cast<TipoFrequencia>()
                        .Select(d => new TipoFrequenciaDto() { Valor = (int)d, Descricao = d.ShortName() })
                        .ToList();

            if (filtro.Modalidade != null)
            {
                var tipoParametroSistema = ObterTipoParametroPorModalidade((Modalidade)filtro.Modalidade);
                var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(tipoParametroSistema, filtro.AnoLetivo != null ? filtro.AnoLetivo.Value : DateTime.Now.Year));;

                if (parametro.Valor == "0")
                    return retorno.Where(a => a.Valor != (int)TipoFrequencia.R);

            }

            return retorno;
        }

        private TipoParametroSistema ObterTipoParametroPorModalidade(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.InfantilCEI:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaEICEI;
                case Modalidade.EJA:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaEJA;
                case Modalidade.CIEJA:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaCIEJA;
                case Modalidade.Fundamental:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaEF;
                case Modalidade.Medio:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaEM;
                case Modalidade.CMCT:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaCMCT;
                case Modalidade.MOVA:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaMOVA;
                case Modalidade.ETEC:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaETEC;
                default:
                    return TipoParametroSistema.HabilitaFrequenciaRemotaEIPre;
            }
        }
    }
}