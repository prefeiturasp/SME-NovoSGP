using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAutenticacaoFrequenciaSGA
    {
        Task<UsuarioAutenticacaoFrequenciaSGARetornoDto> Executar(Guid guid);
    }
}
