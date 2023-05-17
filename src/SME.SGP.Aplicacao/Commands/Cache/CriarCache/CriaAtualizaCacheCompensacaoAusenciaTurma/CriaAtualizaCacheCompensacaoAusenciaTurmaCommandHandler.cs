using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriaAtualizaCacheCompensacaoAusenciaTurmaCommandHandler : IRequestHandler<CriaAtualizaCacheCompensacaoAusenciaTurmaCommand, bool>
    {
        private const int MINUTOS_EXPIRACAO_CACHE = 5;
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IRepositorioCache repositorioCache;

        public CriaAtualizaCacheCompensacaoAusenciaTurmaCommandHandler(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia,
                                                                       IRepositorioCache repositorioCache)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.repositorioCache = repositorioCache;
        }

        public async Task<bool> Handle(CriaAtualizaCacheCompensacaoAusenciaTurmaCommand request, CancellationToken cancellationToken)
        {
            var compensacoesTurma = await repositorioCompensacaoAusencia
                .ObterTotalCompensacoesPorAlunosETurmaAsync(request.Bimestre, request.CodigoTurma);

            var valorNomeChave = string.Format(NomeChaveCache.NOME_CHAVE_COMPENSACAO_TURMA, request.CodigoTurma, request.Bimestre);            

            await repositorioCache
                .SalvarAsync(valorNomeChave, 
                    compensacoesTurma, MINUTOS_EXPIRACAO_CACHE);

            return true;
        }
    }
}
