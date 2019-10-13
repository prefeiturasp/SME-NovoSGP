using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAbrangencia
    {
        Task Salvar(AbrangenciaRetornoEolDto abrangenciaRetornoEolDto, long usuarioId);
    }
}