using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandoFrequencia : IComandoFrequencia
    {
        private readonly IServicoFrequencia servicoFrequencia;

        public ComandoFrequencia(IServicoFrequencia servicoFrequencia)
        {
            this.servicoFrequencia = servicoFrequencia ?? throw new System.ArgumentNullException(nameof(servicoFrequencia));
        }

        public async Task Registrar(FrequenciaDto frequenciaDto)
        {
            List<RegistroAusenciaAluno> registrosAusenciaAlunos = ObtemListaDeAusencias(frequenciaDto);
            await servicoFrequencia.Registrar(frequenciaDto.AulaId, registrosAusenciaAlunos);
        }

        private static List<RegistroAusenciaAluno> ObtemListaDeAusencias(FrequenciaDto frequenciaDto)
        {
            var registrosAusenciaAlunos = new List<RegistroAusenciaAluno>();

            foreach (var frequencia in frequenciaDto.ListaFrequencia.Where(c => c.Aulas.Any(a => !a.Compareceu)))
            {
                foreach (var ausenciaNaAula in frequencia.Aulas.Where(c => !c.Compareceu))
                {
                    registrosAusenciaAlunos.Add(new RegistroAusenciaAluno(frequencia.CodigoAluno, ausenciaNaAula.NumeroAula));
                }
            }

            return registrosAusenciaAlunos;
        }
    }
}