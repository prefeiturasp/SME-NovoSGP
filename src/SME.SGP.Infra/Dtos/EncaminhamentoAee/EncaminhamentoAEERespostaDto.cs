using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class EncaminhamentoAEERespostaDto
    {
        public SituacaoAEE Situacao { get; set; }
        public bool PodeEditar { get; set; }
        public AlunoReduzidoDto Aluno { get; set; }
        public TurmaAnoDto Turma { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
