using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQdadeRegistrosAcaoAlunoMesQueryHandler : ConsultasBase, IRequestHandler<ObterQdadeRegistrosAcaoAlunoMesQuery, int>
    {
        public IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao { get; }

        public ObterQdadeRegistrosAcaoAlunoMesQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao) : base(contextoAplicacao)
        {
            this.repositorioRegistroAcao = repositorioRegistroAcao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcao));
        }

        public Task<int> Handle(ObterQdadeRegistrosAcaoAlunoMesQuery request, CancellationToken cancellationToken)
        {
            return repositorioRegistroAcao.ObterQdadeRegistrosAcaoAlunoMes(request.CodigoAluno, request.Mes, request.Ano);
        }
    }
}
