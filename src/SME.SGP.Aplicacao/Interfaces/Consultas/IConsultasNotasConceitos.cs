using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasNotasConceitos
    {
        Task<NotasConceitosRetornoDto> ListarNotasConceitos(ListaNotasConceitosConsultaDto filtro);

        Task<TipoNota> ObterNotaTipo(long turmaId, int anoLetivo, bool consideraHistorico);

        double ObterValorArredondado(long atividadeAvaliativaId, double nota);
        double ObterValorArredondado(DateTime data, double nota);

        Task<IEnumerable<ConceitoDto>> ObterConceitos(DateTime data);
    }
}