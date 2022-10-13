﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoPendenciaFechamento
    {
        Task<int> ValidarAulasReposicaoPendente(long fechamentoId, string turmaCodigo, string turmaNome, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre, long turmaId);

        Task<int> ValidarAulasSemFrequenciaRegistrada(long fechamentoId, string turmaCodigo, string turmaNome, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre, long turmaId);

        Task<int> ValidarAulasSemPlanoAulaNaDataDoFechamento(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre, long turmaId);

        Task<int> ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int bimestre, long turmaId);

        Task<int> ValidarPercentualAlunosAbaixoDaMedia(long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF, int bimestre, long turmaId);
        Task<AuditoriaPersistenciaDto> Aprovar(long pendenciaId);
        bool VerificarPendenciasEmAbertoPorFechamento(long fechamentoId);

        Task<AuditoriaPersistenciaDto> AtualizarPendencia(long pendenciaId, SituacaoPendencia situacaoPendencia);

        int ObterQuantidadePendenciasGeradas();

        bool AvaliacoesSemNota();
        bool AulasReposicaoPendentes();
        bool AulasSemPlanoAula();
        bool AulasSemFrequencia();
        bool AlunosAbaixoMedia();
        bool NotasExtemporaneasAlteradas();

        Task<int> ValidarAlteracaoExtemporanea(long fechamentoId, string turmaCodigo, string professorRf, int bimestre, long turmaId);
        IEnumerable<string> ObterDescricaoPendenciasGeradas();
    }
}