﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUe
    {
        IEnumerable<Ue> ListarPorCodigos(string[] codigos);

        IEnumerable<Ue> MaterializarCodigosUe(string[] idUes, out string[] codigosNaoEncontrados);

        Task<IEnumerable<Modalidade>> ObterModalidades(string ueCodigo, int ano);

        Ue ObterPorCodigo(string ueId);

        Ue ObterPorId(long id);

        IEnumerable<Ue> ObterPorDre(long dreId);

        IEnumerable<Ue> ObterTodas();

        Task<IEnumerable<Turma>> ObterTurmas(string ueCodigo, Modalidade modalidade, int ano);

        Ue ObterUEPorTurma(string turmaId);

        IEnumerable<Ue> Sincronizar(IEnumerable<Ue> entidades, IEnumerable<Dre> dres);
    }
}