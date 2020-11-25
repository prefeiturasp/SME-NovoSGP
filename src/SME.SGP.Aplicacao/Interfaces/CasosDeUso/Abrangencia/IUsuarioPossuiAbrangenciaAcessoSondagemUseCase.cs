using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IUsuarioPossuiAbrangenciaAcessoSondagemUseCase
    {
        Task<bool> Executar(string usuarioRF, Guid usuarioPerfil);
    }
}
