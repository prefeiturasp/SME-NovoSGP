using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasTipoCalendario
    {
        TipoCalendarioCompletoDto BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade);

        TipoCalendarioCompletoDto BuscarPorId(long id);

        IEnumerable<TipoCalendarioDto> BuscarPorAnoLetivo(int anoLetivo);

        IEnumerable<TipoCalendarioDto> Listar();

        IEnumerable<TipoCalendarioDto> ListarPorAnoLetivo(int anoLetivo);
        Task<TipoCalendario> ObterPorTurma(Turma turma, DateTime dataReferencia);
        Task<bool> PeriodoEmAberto(TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0);
    }
}