using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorCodigoUEQueryHandler : IRequestHandler<ObterTipoCalendarioIdPorCodigoUEQuery, long>
    {
        private readonly IMediator mediator;

        public ObterTipoCalendarioIdPorCodigoUEQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(ObterTipoCalendarioIdPorCodigoUEQuery request, CancellationToken cancellationToken)
        {
            var tipoCalendario = await ObterTipoCalendario(request.UeCodigo);
            var semestre = ObterSemestre(request.Semestre, tipoCalendario);

            return await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(tipoCalendario, request.AnoLetivo, semestre));
        }

        private int ObterSemestre(int semestre, ModalidadeTipoCalendario tipoCalendario)
            => tipoCalendario.EhEjaOuCelp() ? semestre : 0;

        private async Task<ModalidadeTipoCalendario> ObterTipoCalendario(string ueCodigo)
        {
            var tipoEscola = await mediator.Send(new ObterTipoEscolaPorCodigoUEQuery(ueCodigo));
            switch (tipoEscola)
            {
                case TipoEscola.EMEF:
                case TipoEscola.EMEFM:
                case TipoEscola.EMEBS:
                case TipoEscola.ESCPART:
                case TipoEscola.CEUEMEF:
                case TipoEscola.CEU:
                case TipoEscola.CMCT:
                case TipoEscola.MOVA:
                case TipoEscola.ETEC:
                case TipoEscola.ESPCONV:
                case TipoEscola.CCA:
                case TipoEscola.CECI:
                    return ModalidadeTipoCalendario.FundamentalMedio;
                case TipoEscola.EMEI:
                case TipoEscola.CEIDIRET:
                case TipoEscola.CEIINDIR:
                case TipoEscola.CRPCONV:
                case TipoEscola.CCICIPS:
                case TipoEscola.CEUEMEI:
                case TipoEscola.CEUCEI:
                case TipoEscola.CEMEI:
                case TipoEscola.CEUCEMEI:
                    return ModalidadeTipoCalendario.Infantil;
                case TipoEscola.CIEJA:
                    return ModalidadeTipoCalendario.EJA;
                case TipoEscola.CEUATCOMPL:
                    return ModalidadeTipoCalendario.CELP;
                case TipoEscola.Nenhum:
                default:
                    throw new NegocioException("Tipo de Escola não identificado para consulta do Tipo de Calendario");
            }
        }
    }
}
