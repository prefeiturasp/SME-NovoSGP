using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadePorPendenciaQueryHandler : IRequestHandler<ObterModalidadePorPendenciaQuery, int>
    {
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        public ObterModalidadePorPendenciaQueryHandler(IRepositorioPendencia repositorioPendencia, IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }
        public async Task<int> Handle(ObterModalidadePorPendenciaQuery request, CancellationToken cancellationToken)
        {
            int modalidadeDaTurma = await repositorioPendencia.ObterModalidadePorPendenciaETurmaId(request.PendenciaId, request.TurmaId);
            int modalidadeTipoCalendario = (int)ModalidadeExtension.ObterModalidadeTipoCalendario((Modalidade)modalidadeDaTurma);
            return await repositorioPeriodoEscolar.ObterBimestrePorDataPendenciaEModalidade(request.DataPendencia, modalidadeTipoCalendario);
        }
    }
}
