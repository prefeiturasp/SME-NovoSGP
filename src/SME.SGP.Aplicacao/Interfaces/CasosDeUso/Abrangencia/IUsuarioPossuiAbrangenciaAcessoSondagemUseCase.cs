using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IUsuarioPossuiAbrangenciaAcessoSondagemUseCase
    {
        public Task<bool> Executar(string usuarioRF, Guid usuarioPerfil);
    }
}
