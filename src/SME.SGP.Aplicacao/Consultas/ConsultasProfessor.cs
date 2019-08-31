using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasProfessor : IConsultasProfessor
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoEOL servicoEOL;

        public ConsultasProfessor(IServicoEOL servicoEOL,
                                   IRepositorioCache repositorioCache)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
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
    }
}