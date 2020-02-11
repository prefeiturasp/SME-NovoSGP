using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoFinal : IConsultasFechamentoFinal
    {
        private readonly IRepositorioFechamentoFinal repositorioFechamentoFinal;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasFechamentoFinal(IRepositorioTurma repositorioTurma, IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
            IServicoEOL servicoEOL, IServicoUsuario servicoUsuario, IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre,
            IRepositorioFechamentoFinal repositorioFechamentoFinal, IServicoAluno servicoAluno, IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioNotaConceitoBimestre = repositorioNotaConceitoBimestre ?? throw new System.ArgumentNullException(nameof(repositorioNotaConceitoBimestre));
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoFinal));
            this.servicoAluno = servicoAluno ?? throw new System.ArgumentNullException(nameof(servicoAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<FechamentoFinalConsultaRetornoDto> ObterFechamentos(FechamentoFinalConsultaFiltroDto filtros)
        {
            var retorno = new FechamentoFinalConsultaRetornoDto();
            var turma = repositorioTurma.ObterPorCodigo(filtros.TurmaCodigo);

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a UE.");

            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario());
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            retorno.EventoData = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().PeriodoFim;

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrandos alunos para a turma informada.");

            //Codigo aluno / NotaConceito / Código Disciplina / bimestre

            var listaAlunosNotas = new List<(string, string, long, int)>();

            foreach (var periodo in periodosEscolares)
            {
                var fechamentoDoBimestre = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(turma.CodigoTurma, filtros.DisciplinaCodigo, periodo.Bimestre);
                if (fechamentoDoBimestre == null)
                    throw new NegocioException($"Não foi encontrado fechamento para o bimestre {periodo.Bimestre}.");

                var notasEConceitos = await repositorioNotaConceitoBimestre.ObterPorFechamentoTurma(fechamentoDoBimestre.Id);
                if (notasEConceitos == null || !notasEConceitos.Any())
                    throw new NegocioException($"Não foram encontrados notas e conceitos para a turma e bimestre {periodo.Bimestre}.");

                foreach (var notaEConceito in notasEConceitos)
                {
                    var fechamentoFinalConsultaRetornoAlunoDto = new FechamentoFinalConsultaRetornoAlunoDto();
                    listaAlunosNotas.Add((notaEConceito.CodigoAluno, (notaEConceito.ConceitoId.HasValue ? notaEConceito.ConceitoId.ToString() : notaEConceito.Nota.ToString()), notaEConceito.DisciplinaId, periodo.Bimestre));
                }
            }

            var fechamentos = await repositorioFechamentoFinal.ObterPorFiltros(turma.CodigoTurma);
            var idsDisciplinas = listaAlunosNotas.Select(a => a.Item3).Distinct().ToList();
            if (filtros.EhRegencia)
                idsDisciplinas.Add(filtros.DisciplinaCodigo);

            var disciplinas = servicoEOL.ObterDisciplinasPorIds(idsDisciplinas.ToArray());

            foreach (var aluno in alunosDaTurma)
            {
                var totalAusencias = 0;
                var totalAusenciasCompensadas = 0;
                var totalDeAulas = 0;

                foreach (var periodo in periodosEscolares)
                {
                    var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(aluno.CodigoAluno, periodo.PeriodoFim, TipoFrequenciaAluno.PorDisciplina, filtros.DisciplinaCodigo.ToString());
                    if (frequenciaAluno != null)
                    {
                        totalAusencias += frequenciaAluno.TotalAusencias;
                        totalAusenciasCompensadas += frequenciaAluno.TotalCompensacoes;
                        totalDeAulas += frequenciaAluno.TotalAulas;
                    }
                }
                var percentualFrequencia = 100;
                if (totalDeAulas != 0)
                    percentualFrequencia = (((totalDeAulas - totalAusencias) + totalAusenciasCompensadas) / totalDeAulas) * 100;

                var fechamentoFinalAluno = new FechamentoFinalConsultaRetornoAlunoDto();
                fechamentoFinalAluno.Nome = aluno.NomeAluno;
                fechamentoFinalAluno.TotalAusenciasCompensadas = totalAusenciasCompensadas;
                fechamentoFinalAluno.Frequencia = percentualFrequencia;
                fechamentoFinalAluno.TotalFaltas = totalAusencias;

                fechamentoFinalAluno.NumeroChamada = aluno.NumeroAlunoChamada;

                var marcador = servicoAluno.ObterMarcadorAluno(aluno, new PeriodoEscolarDto() { PeriodoFim = retorno.EventoData });
                if (marcador != null)
                {
                    fechamentoFinalAluno.Informacao = marcador.Descricao;
                }

                var notasDosBimestres = listaAlunosNotas.Where(a => a.Item1 == aluno.CodigoAluno).ToList();
                foreach (var notaDoBimestre in notasDosBimestres)
                {
                    var alunoNotaConceitoDoBimestre = new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto() { Bimestre = notaDoBimestre.Item4 };
                    var disciplinaDto = disciplinas.FirstOrDefault(a => a.CodigoComponenteCurricular == notaDoBimestre.Item3);
                    if (disciplinaDto == null)
                        throw new NegocioException("Não foi possível localizar o componente curricular.");

                    alunoNotaConceitoDoBimestre.Disciplina = disciplinaDto.Nome;
                    alunoNotaConceitoDoBimestre.DisciplinaCodigo = disciplinaDto.CodigoComponenteCurricular;
                    alunoNotaConceitoDoBimestre.NotaConceito = notaDoBimestre.Item2;

                    fechamentoFinalAluno.NotasConceitoBimestre.Add(alunoNotaConceitoDoBimestre);
                }

                var notasFechamentoFinal = fechamentos.Where(a => a.AlunoCodigo == aluno.CodigoAluno).ToList();
                foreach (var notaFechamentoFinal in notasFechamentoFinal)
                {
                    var disciplinaFechamentoFinalDto = disciplinas.FirstOrDefault(a => a.CodigoComponenteCurricular == notaFechamentoFinal.DisciplinaCodigo);
                    if (disciplinaFechamentoFinalDto == null)
                        throw new NegocioException("Não foi possível localizar o componente curricular.");

                    var fechamentoFinalConsultaRetornoAlunoNotaConceitoDto = new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto();
                    fechamentoFinalConsultaRetornoAlunoNotaConceitoDto.Disciplina = disciplinaFechamentoFinalDto.Nome;
                    fechamentoFinalConsultaRetornoAlunoNotaConceitoDto.DisciplinaCodigo = disciplinaFechamentoFinalDto.CodigoComponenteCurricular;
                    fechamentoFinalConsultaRetornoAlunoNotaConceitoDto.NotaConceito = notaFechamentoFinal.ConceitoId.HasValue ? notaFechamentoFinal.ConceitoId.Value.ToString() : notaFechamentoFinal.Nota.ToString();

                    fechamentoFinalAluno.NotasConceitoFinal.Add(fechamentoFinalConsultaRetornoAlunoNotaConceitoDto);
                }

                retorno.Alunos.Add(fechamentoFinalAluno);
            }

            return retorno;
        }
    }
}