using System;
using System.Data;

namespace SME.SGP.Dominio
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction IniciarTransacao();

        void PersistirTransacao();

        void Rollback();
    }
}