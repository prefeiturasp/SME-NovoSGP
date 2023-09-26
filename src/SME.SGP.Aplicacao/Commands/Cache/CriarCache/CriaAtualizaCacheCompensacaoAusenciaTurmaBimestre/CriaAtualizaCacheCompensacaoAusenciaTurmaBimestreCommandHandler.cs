using MediatR;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriaAtualizaCacheCompensacaoAusenciaTurmaBimestreCommandHandler : IRequestHandler<CriaAtualizaCacheCompensacaoAusenciaTurmaBimestreCommand, bool>
    {
        private const int MINUTOS_EXPIRACAO_CACHE = 5;
        private readonly IRepositorioCache repositorioCache;

        public CriaAtualizaCacheCompensacaoAusenciaTurmaBimestreCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache;
        }

        public async Task<bool> Handle(CriaAtualizaCacheCompensacaoAusenciaTurmaBimestreCommand request, CancellationToken cancellationToken)
        {
            var valorNomeChave = string.Format(NomeChaveCache.NOME_CHAVE_COMPENSACAO_TURMA_BIMESTRE, request.CodigoTurma, request.Bimestre);

            await repositorioCache
                .SalvarAsync(valorNomeChave,
                    request.TotaisCompensacoesAusenciasAlunos, MINUTOS_EXPIRACAO_CACHE);

            return true;
        }
    }
}
