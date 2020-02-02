using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosRecuperacaoParalela
    {
        Task<RecuperacaoParalelaListagemDto> Salvar(RecuperacaoParalelaDto recuperacaoParalelaDto);
    }
}