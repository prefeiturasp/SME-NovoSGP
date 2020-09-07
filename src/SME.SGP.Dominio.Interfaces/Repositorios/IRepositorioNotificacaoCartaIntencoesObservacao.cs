using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoCartaIntencoesObservacao
    {
        void Inserir(long notificacaoId, long cartaIntencoesObservacaoId);
        void Excluir(long cartaIntencoesObservacaoId);
    }
}
