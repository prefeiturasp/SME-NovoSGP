﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoTurmaDisciplina : IComandosFechamentoTurmaDisciplina
    {
        private readonly IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina;

        public ComandosFechamentoTurmaDisciplina(IServicoFechamentoTurmaDisciplina servicoFechamentoTurmaDisciplina)
        {
            this.servicoFechamentoTurmaDisciplina = servicoFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(servicoFechamentoTurmaDisciplina));
        }

        public async Task Reprocessar(long fechamentoId)
            => await servicoFechamentoTurmaDisciplina.Reprocessar(fechamentoId);

        public async Task<IEnumerable<AuditoriaPersistenciaDto>> Salvar(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma, bool componenteSemNota = false)
        {
            var listaAuditoria = new List<AuditoriaPersistenciaDto>();
            foreach (var fechamentoTurma in fechamentosTurma)
            {
                try
                {
                    listaAuditoria.Add(await servicoFechamentoTurmaDisciplina.Salvar(fechamentoTurma.Id, fechamentoTurma, componenteSemNota));
                }
                catch (Exception e)
                {
                    listaAuditoria.Add(new AuditoriaPersistenciaDto() { Sucesso = false, MensagemConsistencia = $"{fechamentoTurma.Bimestre}º Bimestre: {e.Message}" });
                }
            }

            if (!listaAuditoria.Any(a => a.Sucesso))
                throw new NegocioException("Erro ao salvar o fechamento da turma: " + string.Join(", ", listaAuditoria.Select(s => s.MensagemConsistencia)));

            return listaAuditoria;
        }
    }
}
