using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFrequencia : IConsultasFrequencia
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoFrequencia servicoFrequencia;

        public ConsultasFrequencia(IServicoFrequencia servicoFrequencia,
                                   IServicoEOL servicoEOL,
                                   IRepositorioAula repositorioAula,
                                   IRepositorioTurma repositorioTurma)
        {
            this.servicoFrequencia = servicoFrequencia ?? throw new ArgumentNullException(nameof(servicoFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasLecionadasPeloProfessorPorTurma(string turmaId)
        {
            return await servicoFrequencia.ObterDisciplinasLecionadasPeloProfessorPorTurma(turmaId);
        }

        public async Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
                throw new NegocioException("Aula não encontrada.");

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(aula.TurmaId);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
            {
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");
            }
            var turma = repositorioTurma.ObterPorId(aula.TurmaId);
            if (turma == null)
                throw new NegocioException("Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.");
            FrequenciaDto registroFrequenciaDto = ObterRegistroFrequencia(aulaId, aula, turma);

            var ausencias = servicoFrequencia.ObterListaAusenciasPorAula(aulaId);
            if (ausencias == null)
            {
                ausencias = new List<RegistroAusenciaAluno>();
            }

            foreach (var aluno in alunosDaTurma)
            {
                var registroFrequenciaAluno = new RegistroFrequenciaAlunoDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeAluno,
                    NumeroAlunoChamada = aluno.NumeroAlunoChamada,
                    CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                    SituacaoMatricula = aluno.SituacaoMatricula,
                    Desabilitado = aluno.EstaInativo()
                };

                if (aula.PermiteRegistroFrequencia(turma))
                {
                    var ausenciasAluno = ausencias.Where(c => c.CodigoAluno == aluno.CodigoAluno);

                    for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
                    {
                        registroFrequenciaAluno.Aulas.Add(new FrequenciaAulaDto
                        {
                            NumeroAula = numeroAula,
                            Compareceu = !ausenciasAluno.Any(c => c.NumeroAula == numeroAula)
                        });
                    }
                }
                registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
            }
            if (registroFrequenciaDto.ListaFrequencia.All(c => c.Desabilitado))
            {
                registroFrequenciaDto.Desabilitado = true;
            }
            return registroFrequenciaDto;
        }

        private FrequenciaDto ObterRegistroFrequencia(long aulaId, Aula aula, Turma turma)
        {
            var registroFrequencia = servicoFrequencia.ObterRegistroFrequenciaPorAulaId(aulaId);
            if (registroFrequencia == null)
            {
                registroFrequencia = new RegistroFrequencia(aula);
            }
            var registroFrequenciaDto = new FrequenciaDto(aulaId)
            {
                AlteradoEm = registroFrequencia.AlteradoEm,
                AlteradoPor = registroFrequencia.AlteradoPor,
                AlteradoRF = registroFrequencia.AlteradoRF,
                CriadoEm = registroFrequencia.CriadoEm,
                CriadoPor = registroFrequencia.CriadoPor,
                CriadoRF = registroFrequencia.CriadoRF,
                Id = registroFrequencia.Id,
                Desabilitado = !aula.PermiteRegistroFrequencia(turma)
            };
            return registroFrequenciaDto;
        }
    }
}