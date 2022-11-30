using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterEstudanteLocalAtividadeUseCase 
    {
        Task<IEnumerable<AlunoLocalAtividadeDto>> Executar(string codigoAluno, int? anoLetivo, bool filtrarSituacaoMatricula);
    }
}

