using System;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class ComponenteCurricular 
    {
        public long Id { get; set; }
        public long? AreaConhecimentoId { get; set; }
        public long ComponenteCurricularPaiId { get; set; }
        public string Descricao { get; set; }
        public bool EhBaseNacional { get; set; }
        public bool EhCompatilhado { get; set; }
        public bool EhRegenciaClasse { get; set; }
        public bool EhTerritorio { get; set; }
        public long? GrupoMatrizId { get; set; }
        public bool PermiteLancamentoNota { get; set; }
        public bool PermiteRegistroFrequencia { get; set; }
        //TODO: VERIFICAR SE EH A MELHOR FORMA POIS NAO EH DESTA ENTIDADE E RESOLVO NA QUERY
        public bool TemCurriculoCidade { get; set; }

        public bool PossuiObjetivosDeAprendizagem(bool turmaPrograma, Modalidade modalidadeCodigo, string turmaAno)
        {
            return TemCurriculoCidade && !turmaPrograma && !new[] { Modalidade.EJA, Modalidade.Medio }.Contains(modalidadeCodigo) && turmaAno != "0";
        }

        public bool PossuiObjetivosDeAprendizagemOpcionais(bool ensinoEspecial)
        {
            return ensinoEspecial && (TemCurriculoCidade || new long[] { 218, 138, 1116 }.Contains(Id)|| EhRegenciaClasse);
        }
    }
}
