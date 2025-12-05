using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidadoAtendimentoNAAPACommandHandler : IRequestHandler<SalvarConsolidadoAtendimentoNAAPACommand,bool>
    {
        private readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorio;

        public SalvarConsolidadoAtendimentoNAAPACommandHandler(IRepositorioConsolidadoEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(SalvarConsolidadoAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var entidade = await repositorio.ObterPorUeIdAnoLetivoSituacao(request.Consolidado.UeId,request.Consolidado.AnoLetivo,(int)request.Consolidado.Situacao, (int)request.Consolidado.Modalidade);
            if (entidade.NaoEhNulo())
            {
                request.Consolidado.Id = entidade.Id;
                request.Consolidado.CriadoPor = entidade.CriadoPor;
                request.Consolidado.CriadoRF = entidade.CriadoRF;
                request.Consolidado.CriadoEm = entidade.CriadoEm;
            }

            await repositorio.SalvarAsync(request.Consolidado);
            return true;
        }
    }
}