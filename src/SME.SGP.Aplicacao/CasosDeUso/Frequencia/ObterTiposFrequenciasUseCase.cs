﻿using MediatR;
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
                        .Select(d => new TipoFrequenciaDto() { Valor = d.ShortName(), Descricao = d.ShortName() })
                        .ToList();

            if (filtro.Modalidade.NaoEhNulo())
            {
                var tipoParametroSistema = ObterTipoParametroPorModalidade((Modalidade)filtro.Modalidade);
                var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(tipoParametroSistema, filtro.AnoLetivo.NaoEhNulo() ? filtro.AnoLetivo.Value : DateTime.Now.Year));;

                if (parametro.EhNulo() || parametro.Valor == "0")
                    return retorno.Where(a => a.Valor != TipoFrequencia.R.ShortName());

            }

            return retorno;
        }

        private TipoParametroSistema ObterTipoParametroPorModalidade(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EducacaoInfantil:
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