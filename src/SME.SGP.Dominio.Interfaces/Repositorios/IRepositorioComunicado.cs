﻿using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicado : IRepositorioBase<Comunicado>
    {
        Task<ComunicadosTotaisResultado> ObterComunicadosTotaisSme(int anoLetivo, string codigoDre, string codigoUe);
        Task<IEnumerable<ComunicadosTotaisPorDreResultado>> ObterComunicadosTotaisAgrupadosPorDre(int anoLetivo);
        Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> ObterComunicadosParaFiltroDaDashboard(FiltroObterComunicadosParaFiltroDaDashboardDto filtro);        
        Task<bool> VerificaExistenciaComunicadoParaEvento(long eventoId);
        Task<IEnumerable<ComunicadoTurmaDto>> ObterComunicadosTurma(long comunicadoId);
        Task<IEnumerable<ComunicadoAlunoReduzidoDto>> ObterComunicadosReduzidosPorTipo(TipoComunicado tipoComunicado);
        Task<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>> ObterComunicadosReduzidos(string dreCodigo, string ueCodigo, string turmaCodigo, string alunoCodigo, Paginacao paginacao);
        Task<IEnumerable<Comunicado>> ObterComunicadosPorIds(long[] ids);
        Task<IEnumerable<int>> ObterAnosLetivosComHistoricoDeComunicados(DateTime? dataInicio, DateTime dataAtual);
        Task<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>> ListarComunicados(int anoLetivo, string dreCodigo, string ueCodigo, int[] modalidades, int semestre, DateTime? dataEnvioInicio, DateTime? dataEnvioFim, DateTime? dataExpiracaoInicio, DateTime? dataExpiracaoFim, string titulo, string[] turmasCodigo, string[] anosEscolares, int[] tiposEscolas, Paginacao paginacao);
        Task<IEnumerable<int>> ObterSemestresPorAnoLetivoModalidadeEUeCodigo(string login, Guid perfil, int modalidade, bool consideraHistorico, int anoLetivo, string ueCodigo);
    }
}