using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasTipoCalendario
    {
        Task<TipoCalendarioCompletoDto> BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0);

        Task<TipoCalendarioCompletoDto> BuscarPorId(long id);

       Task<IEnumerable<TipoCalendarioDto>> BuscarPorAnoLetivo(int anoLetivo);

       Task<IEnumerable<TipoCalendarioDto>> Listar();

        Task<IEnumerable<TipoCalendarioDto>> ListarPorAnoLetivo(int anoLetivo);

        Task<TipoCalendario> ObterPorTurma(Turma turma);

        Task<bool> PeriodoEmAberto(TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false);
    }
}