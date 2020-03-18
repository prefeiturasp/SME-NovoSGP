using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAula
    {
        Task<string> Excluir(Aula aula, RecorrenciaAula recorrencia, Usuario usuario);

        Task GravarRecorrencia(bool inclusao, Aula aula, Usuario usuario, RecorrenciaAula recorrencia);

        Task ExcluirRecorrencia(Aula aula, RecorrenciaAula recorrencia, Usuario usuario);

        Task<string> Salvar(Aula aula, Usuario usuario, RecorrenciaAula recorrencia, int quantidadeOriginal = 0, bool ehRecorrencia = false);
    }
}