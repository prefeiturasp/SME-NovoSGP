using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class AtribuicaoEsporadica : EntidadeBase
    {
        public DateTime DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string DreId { get; set; }
        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
        public string ProfessorRf { get; set; }
        public string UeId { get; set; }

        public void Validar(bool ehSme, int anoLetivo, IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            ValidarDataInicio(ehSme, anoLetivo, periodosEscolares);
            ValidarDataFim(ehSme, anoLetivo, periodosEscolares);
        }

        private void ValidarDataFim(bool ehSme, int anoLetivo, IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dentroPeriodo = periodosEscolares.Any(x => x.PeriodoInicio <= DataFim && DataFim <= x.PeriodoFim);

            if (!dentroPeriodo)
                throw new NegocioException("O Inicio da atribuição deve estar dentro de um periodo escolar cadastrado");

            if (ehSme && anoLetivo == DateTime.Now.Year)
                return;
                throw new NegocioException("O ano informado da data não esta dentro do ano vigente");

            if (DataFim < DataInicio)
                throw new NegocioException("O fim da atribuição não pode ser anterior ao inicio");
        }

        private void ValidarDataInicio(bool ehSme, int anoLetivo, IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dentroPeriodo = periodosEscolares.Any(x => x.PeriodoInicio >= DataInicio && DataInicio >= x.PeriodoFim);

            if (!dentroPeriodo)
                throw new NegocioException("O Inicio da atribuição deve estar dentro de um periodo escolar cadastrado");

            if (ehSme && anoLetivo == DateTime.Now.Year)
                return;

            if (DataInicio < DateTime.Now)
                throw new NegocioException("Não pode ser informada uma data passada para o inicio do periodo");

            if (DataInicio.Year != DateTime.Now.Year)
                throw new NegocioException("O ano informado da data não esta dentro do ano vigente");
        }
    }
}