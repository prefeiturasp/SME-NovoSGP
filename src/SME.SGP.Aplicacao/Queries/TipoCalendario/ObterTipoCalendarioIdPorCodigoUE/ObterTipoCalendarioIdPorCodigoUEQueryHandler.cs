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
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IMediator mediator;

        public ObterTipoCalendarioIdPorCodigoUEQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario, IMediator mediator)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(ObterTipoCalendarioIdPorCodigoUEQuery request, CancellationToken cancellationToken)
        {
            var tipoCalendario = await ObterTipoCalendario(request.UeCodigo);
            var semestre = ObterSemestre(request.Semestre, tipoCalendario);

            return await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(tipoCalendario, request.AnoLetivo, semestre));
        }

        private int ObterSemestre(int semestre, ModalidadeTipoCalendario tipoCalendario)
            => tipoCalendario == ModalidadeTipoCalendario.EJA ? semestre : 0;

        private async Task<ModalidadeTipoCalendario> ObterTipoCalendario(string ueCodigo)
        {
            var tipoEscola = await mediator.Send(new ObterTipoEscolaPorCodigoUEQuery(ueCodigo));
            switch (tipoEscola)
            {
                case Dominio.TipoEscola.EMEF:
                case Dominio.TipoEscola.EMEFM:
                case Dominio.TipoEscola.EMEBS:
                case Dominio.TipoEscola.ESCPART:
                case Dominio.TipoEscola.CEUEMEF:
                case Dominio.TipoEscola.CEU:
                case Dominio.TipoEscola.CMCT:
                case Dominio.TipoEscola.MOVA:
                case Dominio.TipoEscola.ETEC:
                case Dominio.TipoEscola.ESPCONV:
                case Dominio.TipoEscola.CEUATCOMPL:
                case Dominio.TipoEscola.CCA:
                case Dominio.TipoEscola.CECI:
                    return ModalidadeTipoCalendario.FundamentalMedio;
                    break;
                case Dominio.TipoEscola.EMEI:
                case Dominio.TipoEscola.CEIDIRET:
                case Dominio.TipoEscola.CEIINDIR:
                case Dominio.TipoEscola.CRPCONV:
                case Dominio.TipoEscola.CCICIPS:
                case Dominio.TipoEscola.CEUEMEI:
                case Dominio.TipoEscola.CEUCEI:
                case Dominio.TipoEscola.CEMEI:
                case Dominio.TipoEscola.CEUCEMEI:
                    return ModalidadeTipoCalendario.Infantil;
                    break;
                case Dominio.TipoEscola.CIEJA:
                    return ModalidadeTipoCalendario.EJA;
                    break;
                case Dominio.TipoEscola.Nenhum:
                default:
                    throw new NegocioException("Tipo de Escola não identificado para consulta do Tipo de Calendario");
                    break;
            }
        }
    }
}
