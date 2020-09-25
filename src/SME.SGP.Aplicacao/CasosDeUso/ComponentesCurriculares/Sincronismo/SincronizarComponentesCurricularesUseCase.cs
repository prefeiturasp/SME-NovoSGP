using MediatR;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SincronizarComponentesCurricularesUseCase : ISincronizarComponentesCurricularesUseCase
    {
        private readonly IListarComponentesCurricularesEolUseCase listarComponentesCurricularesEolUseCase;

        public SincronizarComponentesCurricularesUseCase(IListarComponentesCurricularesEolUseCase listarComponentesCurricularesEolUseCase)
        {
            this.listarComponentesCurricularesEolUseCase = listarComponentesCurricularesEolUseCase ?? 
                throw new System.ArgumentNullException(nameof(listarComponentesCurricularesEolUseCase));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var componentesEol = await listarComponentesCurricularesEolUseCase.Executar();
            return true;
        }
    }
}
