using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
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
        private readonly IRepositorioNotaTipoValor repositorioNotaTipoValor;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasFechamentoFinal(IRepositorioTurma repositorioTurma, IRepositorioTipoCalendario repositorioTipoCalendario,
                            IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
            IServicoEOL servicoEOL, IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre,
            IRepositorioFechamentoFinal repositorioFechamentoFinal, IServicoAluno servicoAluno,
            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo, IRepositorioNotaTipoValor repositorioNotaTipoValor,
            IServicoUsuario servicoUsuario, IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioNotaConceitoBimestre = repositorioNotaConceitoBimestre ?? throw new System.ArgumentNullException(nameof(repositorioNotaConceitoBimestre));
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoFinal));
            this.servicoAluno = servicoAluno ?? throw new System.ArgumentNullException(nameof(servicoAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new System.ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new System.ArgumentNullException(nameof(repositorioParametrosSistema));
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

            VerificaSePodeFazerFechamentoFinal(periodosEscolares, turma);

            retorno.EventoData = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().PeriodoFim;

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrandos alunos para a turma informada.");

            var tipoNota = repositorioNotaTipoValor.ObterPorTurmaId(turma.Id);
            if (tipoNota == null)
                throw new NegocioException("Não foi possível localizar o tipo de nota para esta turma.");

            retorno.EhNota = tipoNota.EhNota();
            //Codigo aluno / NotaConceito / Código Disciplina / bimestre

            var listaAlunosNotas = new List<(string, string, long, int)>();

            var disciplinas = new List<DisciplinaResposta>();

            if (filtros.EhRegencia)
            {
                var disciplinasRegencia = await servicoEOL.ObterDisciplinasParaPlanejamento(long.Parse(turma.CodigoTurma), servicoUsuario.ObterLoginAtual(), servicoUsuario.ObterPerfilAtual());
                if (disciplinasRegencia == null || !disciplinasRegencia.Any())
                    throw new NegocioException("Não foi encontrado componentes curriculares para a regencia informada.");

                disciplinas.AddRange(disciplinasRegencia);
            }
            else
            {
                var disciplinaEol = servicoEOL.ObterDisciplinasPorIds(new long[] { filtros.DisciplinaCodigo });
                if (disciplinaEol == null || !disciplinaEol.Any())
                    throw new NegocioException("Disciplina não localizada.");

                var disciplinaParaAdicionar = disciplinaEol.FirstOrDefault();
                disciplinas.Add(new DisciplinaResposta() { Nome = disciplinaParaAdicionar.Nome, CodigoComponenteCurricular = disciplinaParaAdicionar.CodigoComponenteCurricular });
            }

            var notasFechamentosFinais = await repositorioFechamentoFinal.ObterPorFiltros(turma.CodigoTurma, disciplinas.Select(a => a.CodigoComponenteCurricular.ToString()).ToArray());
            var notasFechamentosBimestres = await ObterNotasFechamentosBimestres(filtros.DisciplinaCodigo, turma, periodosEscolares, retorno.EhNota);

            foreach (var aluno in alunosDaTurma.OrderBy(a => a.NumeroAlunoChamada).ThenBy(a => a.NomeAluno))
            {
                var totalAusencias = 0;
                var totalAusenciasCompensadas = 0;
                var totalDeAulas = 0;

                FechamentoFinalConsultaRetornoAlunoDto fechamentoFinalAluno = TrataFrequenciaAluno(filtros, periodosEscolares, aluno, ref totalAusencias, ref totalAusenciasCompensadas, ref totalDeAulas);

                var marcador = servicoAluno.ObterMarcadorAluno(aluno, new PeriodoEscolarDto() { PeriodoFim = retorno.EventoData });
                if (marcador != null)
                {
                    fechamentoFinalAluno.Informacao = marcador.Descricao;
                }

                foreach (var periodo in periodosEscolares.OrderBy(a => a.Bimestre))
                {
                    foreach (var disciplinaParaAdicionar in disciplinas)
                    {
                        //BIMESTRE / NOTA / DISCIPLINA ID / ALUNO CODIGO
                        var nota = notasFechamentosBimestres.FirstOrDefault(a => a.Item1 == periodo.Bimestre && a.Item3 == disciplinaParaAdicionar.CodigoComponenteCurricular && a.Item4 == aluno.CodigoAluno);
                        var notaParaAdicionar = (nota.GetType() == null ? null : nota.Item2);

                        fechamentoFinalAluno.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                        {
                            Bimestre = periodo.Bimestre,
                            Disciplina = disciplinaParaAdicionar.Nome,
                            DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                            NotaConceito = notaParaAdicionar
                        });
                    }
                }

                foreach (var disciplinaParaAdicionar in disciplinas)
                {
                    var nota = notasFechamentosFinais.FirstOrDefault(a => a.DisciplinaCodigo == disciplinaParaAdicionar.CodigoComponenteCurricular && a.AlunoCodigo == aluno.CodigoAluno);
                    string notaParaAdicionar = string.Empty;
                    if (nota != null)
                        notaParaAdicionar = (tipoNota.EhNota() ? nota.Nota.ToString() : nota.ConceitoId.ToString());

                    fechamentoFinalAluno.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                    {
                        Disciplina = disciplinaParaAdicionar.Nome,
                        DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                        NotaConceito = notaParaAdicionar == string.Empty ? null : notaParaAdicionar
                    });
                }

                retorno.Alunos.Add(fechamentoFinalAluno);
            }

            var ultimaInclusao = notasFechamentosFinais.OrderBy(a => a.CriadoEm).FirstOrDefault();
            var ultimaAlteracao = notasFechamentosFinais.OrderBy(a => a.AlteradoEm).FirstOrDefault();

            retorno.AuditoriaAlteracao = ultimaAlteracao == null ? string.Empty : MontaTextoAuditoriaAlteracao(ultimaAlteracao);
            retorno.AuditoriaInclusao = ultimaInclusao == null ? string.Empty : MontaTextoAuditoriaInclusao(ultimaInclusao);

            retorno.NotaMedia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));
            retorno.FrequenciaMedia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(filtros.EhRegencia ? TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse : TipoParametroSistema.CompensacaoAusenciaPercentualFund2));

            return retorno;
        }

        private string MontaTextoAuditoriaAlteracao(FechamentoFinal ultimaAlteracao)
        {
            return $"Notas(ou conceitos) finais alterados por {ultimaAlteracao.AlteradoPor}({ultimaAlteracao.AlteradoRF}) em {ultimaAlteracao.AlteradoEm.Value.ToString("dd/MM/yyyy")},às {ultimaAlteracao.AlteradoEm.Value.ToString("H:mm")}.";
        }

        private string MontaTextoAuditoriaInclusao(FechamentoFinal ultimaInclusao)
        {
            return $"Notas(ou conceitos) finais incluidos por {ultimaInclusao.CriadoPor}({ultimaInclusao.CriadoRF}) em {ultimaInclusao.CriadoEm.ToString("dd/MM/yyyy")},às {ultimaInclusao.CriadoEm.ToString("H:mm")}.";
        }

        private async Task<IEnumerable<(int, string, long, string)>> ObterNotasFechamentosBimestres(long disciplinaCodigo, Turma turma, IEnumerable<PeriodoEscolar> periodosEscolares, bool ehNota)
        {
            var listaRetorno = new List<(int, string, long, string)>();

            //BIMESTRE / NOTA / DISCIPLINA ID / ALUNO CODIGO
            foreach (var periodo in periodosEscolares)
            {
                var fechamentoTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(turma.CodigoTurma, disciplinaCodigo, periodo.Bimestre);

                if (fechamentoTurmaDisciplina != null)
                {
                    var notasDoBimestre = await repositorioNotaConceitoBimestre.ObterPorFechamentoTurma(fechamentoTurmaDisciplina.Id);
                    if (notasDoBimestre != null && notasDoBimestre.Any())
                    {
                        foreach (var nota in notasDoBimestre)
                        {
                            var notaParaAdicionar = ehNota ? nota.Nota.ToString() : nota.ConceitoId.Value.ToString();
                            listaRetorno.Add((periodo.Bimestre, notaParaAdicionar, nota.DisciplinaId, nota.CodigoAluno));
                        }
                    }
                }
            }

            return listaRetorno;
        }

        private FechamentoFinalConsultaRetornoAlunoDto TrataFrequenciaAluno(FechamentoFinalConsultaFiltroDto filtros, IEnumerable<PeriodoEscolar> periodosEscolares, AlunoPorTurmaResposta aluno, ref int totalAusencias, ref int totalAusenciasCompensadas, ref int totalDeAulas)
        {
            foreach (var periodo in periodosEscolares)
            {
                TrataPeriodosEscolaresParaAluno(filtros, aluno, ref totalAusencias, ref totalAusenciasCompensadas, ref totalDeAulas, periodo);
            }
            var percentualFrequencia = 100;
            if (totalDeAulas != 0)
                percentualFrequencia = (((totalDeAulas - totalAusencias) + totalAusenciasCompensadas) / totalDeAulas) * 100;

            var fechamentoFinalAluno = new FechamentoFinalConsultaRetornoAlunoDto
            {
                Nome = aluno.NomeAluno,
                TotalAusenciasCompensadas = totalAusenciasCompensadas,
                Frequencia = percentualFrequencia,
                TotalFaltas = totalAusencias,
                NumeroChamada = aluno.NumeroAlunoChamada
            };
            return fechamentoFinalAluno;
        }

        private void TrataPeriodosEscolaresParaAluno(FechamentoFinalConsultaFiltroDto filtros, AlunoPorTurmaResposta aluno, ref int totalAusencias, ref int totalAusenciasCompensadas, ref int totalDeAulas, PeriodoEscolar periodo)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(aluno.CodigoAluno, periodo.PeriodoFim, TipoFrequenciaAluno.PorDisciplina, filtros.DisciplinaCodigo.ToString());
            if (frequenciaAluno != null)
            {
                totalAusencias += frequenciaAluno.TotalAusencias;
                totalAusenciasCompensadas += frequenciaAluno.TotalCompensacoes;
                totalDeAulas += frequenciaAluno.TotalAulas;
            }
        }

        private async void VerificaSePodeFazerFechamentoFinal(IEnumerable<PeriodoEscolar> periodosEscolares, Turma turma)
        {
            var ultimoBimestre = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().Bimestre;

            var fechamentoDoUltimoBimestre = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.Id, null, ultimoBimestre);

            if (fechamentoDoUltimoBimestre == null)
                throw new NegocioException($"Para acessar este aba você precisa realizar o fechamento do {ultimoBimestre}º  bimestre.");
        }
    }
}