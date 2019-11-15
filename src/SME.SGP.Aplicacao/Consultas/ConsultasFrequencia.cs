using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasFrequencia : IConsultasFrequencia
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

        public async Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
                throw new NegocioException("Aula não encontrada.");

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurna(aula.TurmaId);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
            {
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");
            }

            var registroFrequencia = new FrequenciaDto(aulaId);

            var frequencias = servicoFrequencia.ObterListaFrequenciaPorAula(aulaId);
            if (frequencias == null)
            {
                frequencias = new List<RegistroFrequenciaDto>();
            }
            foreach (var aluno in alunosDaTurma)
            {
                var frequenciaAula = new RegistroFrequenciaAlunoDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeAluno
                };
                var ausenciasAluno = frequencias.Where(c => c.CodigoAluno == aluno.CodigoAluno);
                for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
                {
                    var aulaAusente = ausenciasAluno.FirstOrDefault(c => c.NumeroAula == numeroAula);
                    var compareceu = true;
                    if (aulaAusente != null)
                    {
                        compareceu = false;
                    }
                    frequenciaAula.Aulas.Add(new FrequenciaAulaDto
                    {
                        NumeroAula = numeroAula,
                        Compareceu = compareceu
                    });
                }
                registroFrequencia.ListaFrequencia.Add(frequenciaAula);
            }

            return registroFrequencia;
        }
    }
}