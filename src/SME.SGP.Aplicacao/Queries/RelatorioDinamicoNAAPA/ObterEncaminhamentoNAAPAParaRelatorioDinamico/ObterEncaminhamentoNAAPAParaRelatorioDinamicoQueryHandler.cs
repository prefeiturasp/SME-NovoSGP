using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAParaRelatorioDinamicoQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentoNAAPAParaRelatorioDinamicoQuery, RelatorioDinamicoNAAPADto>
    {
        private readonly IRepositorioRelatorioDinamicoNAAPA repositorio;

        public ObterEncaminhamentoNAAPAParaRelatorioDinamicoQueryHandler(
                            IContextoAplicacao contextoAplicacao,
                            IRepositorioRelatorioDinamicoNAAPA repositorio) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<RelatorioDinamicoNAAPADto> Handle(ObterEncaminhamentoNAAPAParaRelatorioDinamicoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterRelatorioDinamicoNAAPA(request.Filtro, Paginacao);
        }
    }
}
