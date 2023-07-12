using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAutenticacaoFrequencia
    {
        Task<UsuarioAutenticacaoFrequenciaRetornoDto> Executar(Guid guid);
    }
}
