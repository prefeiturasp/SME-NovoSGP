using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicadoConsulta : IRepositorioBase<Comunicado>
    {
        Task<IEnumerable<ComunicadoTurmaAlunoDto>> ObterComunicadosAnoAtual();
    }
}