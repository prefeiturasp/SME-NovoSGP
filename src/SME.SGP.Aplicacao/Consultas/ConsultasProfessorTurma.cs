using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasProfessor : IConsultasProfessor
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoEol servicoEOL;

        public ConsultasProfessor(IServicoEol servicoEOL, IRepositorioCache repositorioCache)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public IEnumerable<ProfessorTurmaDto> Listar(string codigoRf)
        {
            return MapearParaDto(servicoEOL.ObterListaTurmasPorProfessor(codigoRf));
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterResumoAutoComplete(int anoLetivo, string dreId, string nomeProfessor)
        {
            if (nomeProfessor.Length < 2)
                return null;

            return await servicoEOL.ObterProfessoresAutoComplete(anoLetivo, dreId, nomeProfessor);
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterResumoAutoComplete(int anoLetivo, string dreId, string nomeProfessor, bool incluirEmei)
        {
            if (nomeProfessor.Length < 2)
                return null;

            return await servicoEOL.ObterProfessoresAutoComplete(anoLetivo, dreId, nomeProfessor, incluirEmei);
        }

        public async Task<ProfessorResumoDto> ObterResumoPorRFAnoLetivo(string codigoRF, int anoLetivo)
        {
            return await servicoEOL.ObterResumoProfessorPorRFAnoLetivo(codigoRF, anoLetivo);
        }

        public async Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo)
        {
            IEnumerable<TurmaDto> turmasDto = null;
            var chaveCache = $"Turmas-Professor-{rfProfessor}-ano-{anoLetivo}-escolal-{codigoEscola}";
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            {
                turmasDto = JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(disciplinasCacheString);
            }
            else
            {
                turmasDto = await servicoEOL.ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(rfProfessor, codigoEscola, anoLetivo);
                if (turmasDto != null && turmasDto.Any())
                {
                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(turmasDto));
                }
            }
            return turmasDto;
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
                Semestre = m.Semestre,
                TipoUE = m.TipoUE,
                Ue = m.Ue,
                UeAbrev = m.UeAbrev
            });
        }
    }
}