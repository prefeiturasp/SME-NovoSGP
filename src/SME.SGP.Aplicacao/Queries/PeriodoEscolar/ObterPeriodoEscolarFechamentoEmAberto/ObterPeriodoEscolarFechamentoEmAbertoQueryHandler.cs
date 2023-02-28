using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarFechamentoEmAbertoQueryHandler : IRequestHandler<ObterPeriodoEscolarFechamentoEmAbertoQuery, IEnumerable<PeriodoEscolar>>
    {
        private readonly IRepositorioEventoFechamentoConsulta repositorioFechamento;

        public ObterPeriodoEscolarFechamentoEmAbertoQueryHandler(IRepositorioEventoFechamentoConsulta repositorioFechamento)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new System.ArgumentNullException(nameof(repositorioFechamento));
        }

        public Task<IEnumerable<PeriodoEscolar>> Handle(ObterPeriodoEscolarFechamentoEmAbertoQuery request, CancellationToken cancellationToken)
            => repositorioFechamento.ObterPeriodoFechamentoEmAbertoTurma(request.CodigoTurma, request.ModalidadeTipoCalendario, request.DataReferencia);
    }
}