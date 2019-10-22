using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class ServicoPeriodoEscolar : IServicoPeriodoEscolar
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoPeriodoEscolar(IUnitOfWork unitOfWork, IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public void SalvarPeriodoEscolar(IEnumerable<PeriodoEscolar> periodos, long tipoCalendario)
        {
            var tipo = repositorioTipoCalendario.ObterPorId(tipoCalendario);

            if (tipo == null || tipo.Id == 0) throw new NegocioException("O tipo de calendario informado não foi encontrado");

            bool eja = tipo.Modalidade == ModalidadeTipoCalendario.EJA;
            int quantidadeBimestres = eja ? 2 : 4;

            ValidarEntidade(periodos, tipo.AnoLetivo, eja, quantidadeBimestres);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var periodo in periodos)
                {
                    repositorioPeriodoEscolar.Salvar(periodo);
                }

                unitOfWork.PersistirTransacao();
            }
        }

        private void ValidarBimestresRepetidos(IEnumerable<PeriodoEscolar> periodos)
        {
            var bimestres = periodos.Select(x => x.Bimestre).GroupBy(x => x).Where(x => x.Count() > 1);

            if (bimestres.Any())
                throw new NegocioException("Deve ser informado apenas um período por bimestre");
        }

        private void ValidarEntidade(IEnumerable<PeriodoEscolar> periodos, int anoBase, bool eja, int quantidadeBimestres)
        {
            ValidarQuantidadePeriodos(periodos, quantidadeBimestres, eja);

            ValidarBimestresRepetidos(periodos);

            ValidarPeriodos(periodos, anoBase, eja);

            ValidarInicioPeriodoAntesFimPeriodoAnterior(periodos);
        }

        private void ValidarInicioPeriodoAntesFimPeriodoAnterior(IEnumerable<PeriodoEscolar> periodos)
        {
            for (int i = 1; i < periodos.Count() - 1; i++)
            {
                if (periodos.ElementAt(i + 1).PeriodoInicio < periodos.ElementAt(i).PeriodoFim)
                    throw new NegocioException($"O inicio do {i + 1}º Bimestre não pode ser anterior ao fim do {i}º Bimestre");
            }
        }

        private void ValidarPeriodos(IEnumerable<PeriodoEscolar> periodos, int anoBase, bool eja)
        {
            foreach (var periodo in periodos)
            {
                periodo.Validar(anoBase, eja);
            }
        }

        private void ValidarQuantidadePeriodos(IEnumerable<PeriodoEscolar> periodos, int quantidadeBimestres, bool eja)
        {
            bool valido = periodos.Count() == quantidadeBimestres;

            if (!valido)
                throw new NegocioException($"Para período {(eja ? "semestral" : "anual")} devem ser informados {quantidadeBimestres} bimestres");
        }
    }
}