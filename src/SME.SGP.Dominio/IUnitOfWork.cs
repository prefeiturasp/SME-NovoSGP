using System;
using System.Data;

namespace SME.SGP.Dominio
{
    public interface IUnitOfWork
    {
        void PersistirTransacao();
        void Rollback();
        IDbTransaction IniciarTransacao();
    }
}
