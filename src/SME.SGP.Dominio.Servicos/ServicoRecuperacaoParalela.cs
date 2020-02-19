using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoRecuperacaoParalela : IServicoRecuperacaoParalela
    {
        private const string RecuperacaoParalelaFrequente = "RecuperacaoParalelaFrequente";
        private const string RecuperacaoParalelaNaoComparece = "RecuperacaoParalelaNaoComparece";
        private const string RecuperacaoParalelaPoucoFrequente = "RecuperacaoParalelaPoucoFrequente";
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ServicoRecuperacaoParalela(IRepositorioFrequencia repositorioFrequencia, IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> ObterFrequencias(string[] CodigoAlunos, string CodigoDisciplina, int Ano, PeriodoRecuperacaoParalela Periodo)
        {
            var retorno = new List<KeyValuePair<string, int>>();
            var parametrosFrequencia = repositorioParametrosSistema.ObterChaveEValorPorTipo(TipoParametroSistema.RecuperacaoParalelaFrequencia);
            var frequente = double.Parse(parametrosFrequencia.FirstOrDefault(w => w.Key == RecuperacaoParalelaFrequente).Value);
            var poucoFrequente = double.Parse(parametrosFrequencia.FirstOrDefault(w => w.Key == RecuperacaoParalelaPoucoFrequente).Value);
            var naoComparece = double.Parse(parametrosFrequencia.FirstOrDefault(w => w.Key == RecuperacaoParalelaNaoComparece).Value);

            var frequencias = await repositorioFrequencia.ObterFrequenciaAusencias(CodigoAlunos, CodigoDisciplina, Ano, Periodo);

            foreach (var aluno in frequencias)
            {
                if (CodigoAlunos.Contains(aluno.CodigoAluno))
                {
                    double frequencia = 100 - (aluno.TotalAusencias / (double)aluno.TotalAulas * 100);
                    if (frequencia >= frequente)
                        retorno.Add(new KeyValuePair<string, int>(aluno.CodigoAluno, (int)RecuperacaoParalelaFrequencia.Frequente));
                    else if (frequencia >= naoComparece)
                        retorno.Add(new KeyValuePair<string, int>(aluno.CodigoAluno, (int)RecuperacaoParalelaFrequencia.PoucoFrequente));
                    else
                        retorno.Add(new KeyValuePair<string, int>(aluno.CodigoAluno, (int)RecuperacaoParalelaFrequencia.NaoComparete));
                }
            }

            if (!retorno.Any())
                return retorno;

            foreach (var codigoAluno in CodigoAlunos)
            {
                if (retorno.Any(x => x.Key.Equals(codigoAluno)))
                    retorno.Add(new KeyValuePair<string, int>(codigoAluno, (int)RecuperacaoParalelaFrequencia.Frequente));
            }

            return retorno;
        }

        public RecuperacaoParalelaStatus ObterStatusRecuperacaoParalela(int RespostasRecuperacaoParalela, int Objetivos)
        {
            if (RespostasRecuperacaoParalela == Objetivos)
                return RecuperacaoParalelaStatus.Concluido;
            if (RespostasRecuperacaoParalela > 0)
                return RecuperacaoParalelaStatus.Alerta;
            return RecuperacaoParalelaStatus.NaoAlterado;
        }

        public long ValidarParecerConclusivo(char Parecer)
        {
            if (Parecer == (char)ParecerConclusivo.Aprovado)
                return 3;
            if (Parecer == (char)ParecerConclusivo.Reprovado)
                return 6;
            else return 7;
        }
    }
}