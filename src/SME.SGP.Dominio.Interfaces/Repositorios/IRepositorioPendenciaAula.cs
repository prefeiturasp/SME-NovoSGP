﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaAula
    {
        Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendenciaAula tipoPendenciaAula, string tabelaReferencia, long[] modalidades);
        Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa();

        Task<long[]> ListarPendenciasPorAulaId(long aulaId);
        Task<long[]> ListarPendenciasPorAulasId(long[] aulaId);

        Task Salvar(PendenciaAula pendenciaAula);
        Task Excluir(TipoPendenciaAula tipoPendenciaAula, long aulaId);

        

        Task<PendenciaAula> ObterPendenciaPorAulaIdETipo(TipoPendenciaAula tipoPendenciaAula, long aulaId);

        void SalvarVarias(IEnumerable<Aula> aulas, TipoPendenciaAula tipoPendenciaAula);
    }
}