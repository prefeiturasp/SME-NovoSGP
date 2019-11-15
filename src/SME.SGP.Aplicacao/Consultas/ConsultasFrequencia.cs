using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasFrequencia
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoFrequencia servicoFrequencia;

        public ConsultasFrequencia(IServicoFrequencia servicoFrequencia,
                                   IServicoEOL servicoEOL,
                                   IRepositorioAula repositorioAula)
        {
            this.servicoFrequencia = servicoFrequencia ?? throw new ArgumentNullException(nameof(servicoFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<RegistroFrequenciaDto>> Listar(long aulaId)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
                throw new NegocioException("Aula não encontrada.");

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurna(aula.TurmaId);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
            {
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");
            }

            var frequencias = servicoFrequencia.ObterListaFrequenciaPorAula(aulaId);
            if (frequencias != null || frequencias.Any())
            {
            }

            return null;
        }
    }
}