using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoCompensacaoAusencia
    {
        void Inserir(long notificacaoId, long compensacaoAusenciaId);
        void Excluir(long compensacaoAusenciaId);
    }
}
