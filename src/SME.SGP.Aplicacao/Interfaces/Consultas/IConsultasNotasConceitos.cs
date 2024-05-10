using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasNotasConceitos
    {
        Task<NotasConceitosRetornoDto> ListarNotasConceitos(ListaNotasConceitosConsultaDto filtro);

        Task<TipoNota> ObterNotaTipo(long turmaId, int anoLetivo, bool consideraHistorico);

        Task<double> ObterValorArredondado(long atividadeAvaliativaId, double nota);

        Task<double> ObterValorArredondado(DateTime data, double nota);

        Task<IEnumerable<ConceitoDto>> ObterConceitos(DateTime data);
    }
}