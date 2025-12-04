using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosDeAcaoParaNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterRegistrosDeAcaoParaNAAPAQuery, PaginacaoResultadoDto<RegistroAcaoBuscaAtivaNAAPADto>>
    {
        private readonly IRepositorioRegistroAcaoBuscaAtiva repositorio;
        public ObterRegistrosDeAcaoParaNAAPAQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRegistroAcaoBuscaAtiva repositorio) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaNAAPADto>> Handle(ObterRegistrosDeAcaoParaNAAPAQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ListarPaginadoRegistroAcaoParaNAAPA(request.CodigoAluno, Paginacao);
        }
    }
}
