using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasProfessor : IConsultasProfessor
    {
        private readonly IServicoEOL servicoEOL;

        public ConsultasProfessor(IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public IEnumerable<ProfessorTurmaDto> Listar(string codigoRf)
        {
            return MapearParaDto(servicoEOL.ObterListaTurmasPorProfessor(codigoRf));
        }

        private IEnumerable<ProfessorTurmaDto> MapearParaDto(IEnumerable<ProfessorTurmaReposta> turmas)
        {
            return turmas?.Select(m => new ProfessorTurmaDto()
            {
                Ano = m.Ano,
                AnoLetivo = m.AnoLetivo,
                CodDre = m.CodDre,
                CodEscola = m.CodEscola,
                CodModalidade = m.CodModalidade,
                CodTipoEscola = m.CodTipoEscola,
                CodTipoUE = m.CodTipoUE,
                CodTurma = m.CodTurma,
                Dre = m.Dre,
                DreAbrev = m.DreAbrev,
                Modalidade = m.Modalidade,
                NomeTurma = m.NomeTurma,
                TipoEscola = m.TipoEscola,
                TipoSemestre = m.TipoSemestre,
                TipoUE = m.TipoUE,
                Ue = m.Ue,
                UeAbrev = m.UeAbrev
            });
        }
    }
}