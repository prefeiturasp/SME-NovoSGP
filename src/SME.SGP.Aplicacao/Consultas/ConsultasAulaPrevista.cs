using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAulaPrevista : IConsultasAulaPrevista
    {
        private readonly IRepositorioAulaPrevista repositorio;
        private readonly IRepositorioAulaPrevistaBimestre repositorioBimestre;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ConsultasAulaPrevista(IRepositorioAulaPrevista repositorio,
                                     IRepositorioAulaPrevistaBimestre repositorioBimestre,
                                     IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioBimestre = repositorioBimestre ?? throw new ArgumentNullException(nameof(repositorioBimestre));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<AulasPrevistasDadasAuditoriaDto> BuscarPorId(long id)
        {
            AulasPrevistasDadasAuditoriaDto aulaPrevistaDto = null;
            var aulaPrevista = repositorio.ObterPorId(id);

            if (aulaPrevista != null)
            {
                var aulaPrevistaBimestres = await ObterBimestres(aulaPrevista.Id);
                aulaPrevistaDto = MapearDtoRetorno(aulaPrevista, aulaPrevistaBimestres);
            }

            return aulaPrevistaDto;
        }

        private async Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestres(long? aulaPrevistaId)
        {
            return await repositorioBimestre.ObterBimestresAulasPrevistasPorId(aulaPrevistaId);
        }

        private IEnumerable<PeriodoEscolar> ObterPeriodosEscolares(long tipoCalendarioId)
        {
            return repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
        }

        private AulasPrevistasDadasAuditoriaDto MapearDtoRetorno(AulaPrevista aulaPrevista, IEnumerable<AulaPrevistaBimestreQuantidade> aulasPrevistasBimestre)
        {
            AulasPrevistasDadasAuditoriaDto aulaPrevistaDto = MapearParaDto(aulaPrevista, aulasPrevistasBimestre) ?? new AulasPrevistasDadasAuditoriaDto();
            aulaPrevistaDto = MapearMensagens(aulaPrevistaDto);

            return aulaPrevistaDto;
        }

        public async Task<AulasPrevistasDadasAuditoriaDto> ObterAulaPrevistaDada(Modalidade modalidade, string turmaId, string disciplinaId)
        {
            AulasPrevistasDadasAuditoriaDto aulaPrevistaDto;

            int tipoCalendarioId = (int)ModalidadeParaModalidadeTipoCalendario(modalidade);

            var aulaPrevista = await repositorio.ObterAulaPrevistaFiltro(tipoCalendarioId, turmaId, disciplinaId);

            IEnumerable<AulaPrevistaBimestreQuantidade> aulaPrevistaBimestres;

            if (aulaPrevista != null)
                aulaPrevistaBimestres = await ObterBimestres(aulaPrevista.Id);
            else
            {
                aulaPrevista = new AulaPrevista();

                var periodosBimestre = ObterPeriodosEscolares(tipoCalendarioId);
                aulaPrevistaBimestres = MapearPeriodoParaBimestreDto(periodosBimestre);
            }

            aulaPrevistaDto = MapearDtoRetorno(aulaPrevista, aulaPrevistaBimestres);

            return aulaPrevistaDto;
        }

        private AulasPrevistasDadasAuditoriaDto MapearMensagens(AulasPrevistasDadasAuditoriaDto aulaPrevistaDto)
        {
            foreach (var aula in aulaPrevistaDto.AulasPrevistasPorBimestre)
            {
                List<string> mensagens = new List<string>();

                if (aula.Previstas.Quantidade != (aula.Criadas.QuantidadeCJ + aula.Criadas.QuantidadeTitular))
                    mensagens.Add("Quantidade de aulas previstas diferente da quantidade de aulas criadas.");

                if (aula.Previstas.Quantidade != (aula.Cumpridas + aula.Reposicoes))
                    mensagens.Add("Quantidade de aulas previstas diferente do somatório de aulas dadas + aulas repostas, após o final do bimestre.");

                if (mensagens.Any())
                    aula.Previstas.Mensagens = mensagens.ToArray();
            }

            return aulaPrevistaDto;
        }

        private ModalidadeTipoCalendario ModalidadeParaModalidadeTipoCalendario(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;

                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }

        private AulasPrevistasDadasAuditoriaDto MapearParaDto(AulaPrevista aulaPrevista, IEnumerable<AulaPrevistaBimestreQuantidade> bimestres = null)
        {
            return aulaPrevista == null ? null : new AulasPrevistasDadasAuditoriaDto
            {
                Id = aulaPrevista.Id,
                AlteradoEm = aulaPrevista.AlteradoEm.HasValue ? aulaPrevista.AlteradoEm.Value : DateTime.MinValue,
                AlteradoPor = aulaPrevista.AlteradoPor,
                AlteradoRF = aulaPrevista.AlteradoRF,
                CriadoEm = aulaPrevista.CriadoEm,
                CriadoPor = aulaPrevista.CriadoPor,
                CriadoRF = aulaPrevista.CriadoRF,
                AulasPrevistasPorBimestre = bimestres?.Select(x => new AulasPrevistasDadasDto
                {
                    Bimestre = x.Bimestre,
                    Criadas = new AulasQuantidadePorProfessorDto()
                    {
                        QuantidadeCJ = x.CriadasCJ,
                        QuantidadeTitular = x.CriadasTitular
                    },
                    Cumpridas = x.Cumpridas,
                    Inicio = x.Inicio,
                    Fim = x.Fim,
                    Previstas = new AulasPrevistasDto() { Quantidade = x.Previstas },
                    Reposicoes = x.Reposicoes
                }).ToList()
            };
        }

        private IEnumerable<AulaPrevistaBimestreQuantidade> MapearPeriodoParaBimestreDto(IEnumerable<PeriodoEscolar> periodoEscolares)
        {
            IEnumerable<AulaPrevistaBimestreQuantidade> bimestreQuantidades = new List<AulaPrevistaBimestreQuantidade>();

            return periodoEscolares?.Select(x => new AulaPrevistaBimestreQuantidade
            {
                Bimestre = x.Bimestre,
                Inicio = x.PeriodoInicio,
                Fim = x.PeriodoFim
            }).ToList();
        }
    }
}
