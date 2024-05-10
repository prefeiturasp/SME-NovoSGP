using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosQueryHandler : ConsultasBase, IRequestHandler<ObterComunicadosPaginadosQuery, PaginacaoResultadoDto<ComunicadoListaPaginadaDto>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterComunicadosPaginadosQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioComunicado repositorioComunicado) : base(contextoAplicacao)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>> Handle(ObterComunicadosPaginadosQuery request, CancellationToken cancellationToken)
            => await repositorioComunicado.ListarComunicados(request.AnoLetivo,
                                                             request.DreCodigo,
                                                             request.UeCodigo,
                                                             request.Modalidades,
                                                             request.Semestre,
                                                             request.DataEnvioInicio,
                                                             request.DataEnvioFim,
                                                             request.DataExpiracaoInicio,
                                                             request.DataExpiracaoFim,
                                                             request.Titulo,
                                                             request.TurmasCodigo,
                                                             request.AnosEscolares,
                                                             request.TiposEscolas,
                                                             Paginacao);
    }
}
