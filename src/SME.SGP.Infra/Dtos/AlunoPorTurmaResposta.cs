using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class AlunoPorTurmaResposta
    {
        private string nomeAluno;

        public int Ano { get; set; }
        public string CodigoAluno { get; set; }
        public int? CodigoComponenteCurricular { get; set; }
        public SituacaoMatriculaAluno CodigoSituacaoMatricula { get; set; }
        public long CodigoTurma { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataSituacao { get; set; }
        public DateTime DataMatricula { get; set; }
        public string EscolaTransferencia { get; set; }

        public string NomeAluno 
        { 
            get => !string.IsNullOrWhiteSpace(NomeSocialAluno) ? NomeSocialAluno : nomeAluno; 
            set { nomeAluno = value; } 
        }

        public string NomeSocialAluno { get; set; }
        public int? NumeroAlunoChamada { get; set; }
        public int ObterNumeroAlunoChamada() => NumeroAlunoChamada.HasValue ? NumeroAlunoChamada.Value : 0;
        public char? ParecerConclusivo { get; set; }
        public bool PossuiDeficiencia { get; set; }
        public string SituacaoMatricula { get; set; }
        public bool Transferencia_Interna { get; set; }
        public string TurmaEscola { get; set; }
        public string TurmaRemanejamento { get; set; }
        public string TurmaTransferencia { get; set; }
        public string NomeResponsavel { get; set; }
        public string TipoResponsavel { get; set; }
        public string CelularResponsavel { get; set; }
        public DateTime? DataAtualizacaoContato { get; set; }
        public string CodigoEscola { get; set; }
        public int CodigoTipoTurma { get; set; }
        public DateTime DataAtualizacaoTabela { get; set; }
        public string DocumentoCpf { get; set; } = null;

        public bool Inativo
        {
            get => !(new[] { SituacaoMatriculaAluno.Ativo,
                             SituacaoMatriculaAluno.PendenteRematricula,
                             SituacaoMatriculaAluno.Rematriculado,
                             SituacaoMatriculaAluno.SemContinuidade,
                             SituacaoMatriculaAluno.Concluido }.Contains(this.CodigoSituacaoMatricula));
        }

        public bool Ativo { get => !Inativo; }

        public int Idade 
        { 
            get 
            { 
                return ((int.Parse(DateTime.Now.ToString("yyyyMMdd")) - int.Parse(DataNascimento.ToString("yyyyMMdd"))) / 10000); 
            } 
        }

        public bool Maioridade => Idade > 18;

        private SituacaoMatriculaAluno[] SituacoesAtiva => new[] {
            SituacaoMatriculaAluno.Concluido,
            SituacaoMatriculaAluno.Ativo,
            SituacaoMatriculaAluno.Rematriculado,
            SituacaoMatriculaAluno.PendenteRematricula,
            SituacaoMatriculaAluno.SemContinuidade
        };

        public bool PossuiSituacaoAtiva()
        {
            return SituacoesAtiva.Contains(CodigoSituacaoMatricula);
        }

        public bool PossuiSituacaoDispensadoTurmaEdFisica(Func<Task<Turma>> instanciarTurma)
        {
            if (CodigoSituacaoMatricula == SituacaoMatriculaAluno.DispensadoEdFisica)
                return (instanciarTurma().Result).EhTurmaEdFisica();
            return false;
        }

        public bool DeveMostrarNaChamada(DateTime dataAula, DateTime periodoInicio)
        {
            return EstaAtivo(dataAula) || (!PossuiSituacaoAtiva() && DataSituacao.Date > periodoInicio.Date);
        }

        /// <summary>
        /// Verifica se o aluno está ativo
        /// </summary>
        /// <param name="dataBase">Data a se considerar para verificar a situação do aluno, Ex: Data da aula</param>
        /// <returns></returns>
        public bool EstaAtivo(DateTime dataBase) => TratarExcepcionalmenteSituacaoAtivo(dataBase) ? SituacoesAtiva.Contains(CodigoSituacaoMatricula) :
                                                    (SituacoesAtiva.Contains(CodigoSituacaoMatricula) && DataMatricula.Date < dataBase.Date) ||
                                                    (!SituacoesAtiva.Contains(CodigoSituacaoMatricula) && DataSituacao.Date >= dataBase.Date) ||
                                                    ChecarSituacaoConcluidoEMatriculaNaMesmaData(dataBase);

        /// <summary>
        /// Verifica se o aluno está ativo para Notas e Frequencia
        /// </summary>
        /// <param name="periodoInicio">Data a se considerar para verificar a situação do aluno no periodo, Ex: Data do inicio do bimestre</param>
        /// <param name="periodoFim">Data a se considerar para verificar a situação do aluno no periodo, Ex: Data do fim do bimestre</param>
        /// <returns></returns>
        public bool EstaAtivo(DateTime periodoInicio, DateTime periodoFim) => TratarExcepcionalmenteSituacaoAtivo(periodoFim) ? SituacoesAtiva.Contains(CodigoSituacaoMatricula) :
                                                    SituacoesAtiva.Contains(CodigoSituacaoMatricula) && (DataSituacao.Date <= periodoInicio.Date || (DataSituacao.Date > periodoFim.Date && DataMatricula.Date < periodoFim.Date)) 
                                                    || (DataSituacao.Date >= periodoInicio.Date && DataSituacao.Date <= periodoFim.Date);

        /// <summary>
        /// Verifica se o aluno está inativo
        /// </summary>
        /// <param name="dataBase">Data a se considerar para verificar a situação do aluno, Ex: Data da aula</param>
        /// <returns></returns>
        public bool EstaInativo(DateTime dataBase) => !EstaAtivo(dataBase);

        /// <summary>
        /// Verifica se o aluno está inativo por periodo
        /// </summary>
        /// <param name="periodoInicio">Data a se considerar para verificar a situação do aluno no periodo, Ex: Data do inicio do bimestre</param>
        /// <param name="periodoFim">Data a se considerar para verificar a situação do aluno no periodo, Ex: Data do fim do bimestre</param>
        /// <returns></returns>
        public bool EstaInativo(DateTime periodoInicio, DateTime periodoFim) => !EstaAtivo(periodoInicio, periodoFim);

        public string NomeValido()
        {
            return string.IsNullOrEmpty(NomeSocialAluno) ? NomeAluno : NomeSocialAluno;
        }

        public bool PodeEditarNotaConceito()
        {
            if (CodigoSituacaoMatricula != SituacaoMatriculaAluno.Ativo &&
                CodigoSituacaoMatricula != SituacaoMatriculaAluno.PendenteRematricula &&
                CodigoSituacaoMatricula != SituacaoMatriculaAluno.Rematriculado &&
                CodigoSituacaoMatricula != SituacaoMatriculaAluno.SemContinuidade &&
                CodigoSituacaoMatricula != SituacaoMatriculaAluno.Concluido)
                return false;

            return true;
        }

        public bool PodeEditarNotaConceitoNoPeriodo(PeriodoEscolar periodoEscolar, bool temPeriodoAberto)
        {
            return PodeEditarNotaConceito() || temPeriodoAberto;
        }

        /// <summary>
        /// Identifica se o aluno precisa de um tratamento excepcional ao feito comumente,
        /// pois nos deparamos com a situaçã de alunos de uma turma possuírem a data de matrícula e situação praticamente iguais e com
        /// ano posterior ao ano da turma. Dessa forma não é possível determinar quando cada aluno iniciou e terminou na turma.
        /// </summary>
        /// <param name="dataReferencia">Data de referência utilzada para verificar se o ano coincide com os anos de matricula e situação.</param>
        /// <returns></returns>
        private bool TratarExcepcionalmenteSituacaoAtivo(DateTime dataReferencia)
            => DataMatricula.Year.Equals(DataSituacao.Year) && DataMatricula.Year > dataReferencia.Year;


        /// <summary>
        /// Verifica se o aluno esteve na turma mas por algum motivo a sua matrícula foi alterada pela mesma data da conclusão de seus estudos na turma.
        /// </summary>
        /// <param name="dataReferencia">Data de referência utilzada para verificar a posterioridade da situação com o período bimestral </param>
        /// <returns></returns>
        private bool ChecarSituacaoConcluidoEMatriculaNaMesmaData(DateTime dataReferencia)
            => DataMatricula.Date > dataReferencia && DataMatricula.Date.Equals(DataSituacao.Date) && CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido);

        public bool VerificaSeMatriculaEstaDentroDoPeriodoSelecionado(DateTime dataReferencia)
            => DataMatricula.Date <= dataReferencia || Ano == dataReferencia.Year;

        public bool VerificaSePodeEditarAluno(PeriodoEscolar ultimoPeriodoEscolar)
        {
            if (!PodeEditarNotaConceito() && ultimoPeriodoEscolar.NaoEhNulo())
                return EstaAtivo(ultimoPeriodoEscolar.PeriodoInicio, ultimoPeriodoEscolar.PeriodoFim);

            return PodeEditarNotaConceito();
        }
    }
}