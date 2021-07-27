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
        public int AnoLetivo { get; set; }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Estra atribuição já está excluida.");
            Excluido = true;
        }

        public void Validar(bool ehSme, int anoLetivo, IEnumerable<PeriodoEscolar> periodosEscolares, ModalidadeTipoCalendario modalidade = ModalidadeTipoCalendario.FundamentalMedio)
        {
            if (modalidade == ModalidadeTipoCalendario.Infantil)
                if (anoLetivo < 2021)
                    throw new NegocioException("Apenas é possível inserir atribuição esporádica para Educação Infantil a partir de 2021.");
            ValidarDataInicio(ehSme, anoLetivo, periodosEscolares);
            ValidarDataFim(ehSme, anoLetivo, periodosEscolares);
        }

        private void ValidarDataFim(bool ehSme, int anoLetivo, IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dentroPeriodo = periodosEscolares.Any(x => x.PeriodoInicio <= DataFim && DataFim <= x.PeriodoFim);

            if (!dentroPeriodo)
                throw new NegocioException("O Fim da atribuição deve estar dentro de um periodo escolar cadastrado");

            if (DataFim.Year != anoLetivo)
                throw new NegocioException("O ano informado da data fim não esta dentro do ano vigente");

            if (DataFim < DataInicio)
                throw new NegocioException("O fim da atribuição não pode ser anterior ao inicio");

            if (ehSme && anoLetivo == DateTime.Today.Year)
                return;
        }

        private void ValidarDataInicio(bool ehSme, int anoLetivo, IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dentroPeriodo = periodosEscolares.Any(x => x.PeriodoInicio <= DataInicio && DataInicio <= x.PeriodoFim);

            if (!dentroPeriodo)
                throw new NegocioException("O Inicio da atribuição deve estar dentro de um periodo escolar cadastrado");

            if (DataInicio.Year != anoLetivo)
                throw new NegocioException("O ano informado da data inicio não esta dentro do ano vigente");

            if (ehSme && anoLetivo == DateTime.Today.Year)
                return;

        }
    }
}