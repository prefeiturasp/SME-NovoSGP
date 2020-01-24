using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosRecuperacaoParalela
    {
        Task<IEnumerable<RecuperacaoParalelaDto>> Salvar(IEnumerable<RecuperacaoParalelaDto> recuperacaoParalelaDto);
    }
}