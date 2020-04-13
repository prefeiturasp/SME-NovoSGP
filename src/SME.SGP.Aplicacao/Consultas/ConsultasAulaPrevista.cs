﻿using SME.SGP.Dominio;
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
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IConsultasTurma consultasTurma;

        public ConsultasAulaPrevista(IRepositorioAulaPrevista repositorio,
                                     IRepositorioAulaPrevistaBimestre repositorioBimestre,
                                     IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                     IRepositorioTurma repositorioTurma,
                                     IRepositorioTipoCalendario repositorioTipoCalendario,
                                     IConsultasTurma consultasTurma)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioBimestre = repositorioBimestre ?? throw new ArgumentNullException(nameof(repositorioBimestre));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
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

        public async Task<AulasPrevistasDadasAuditoriaDto> ObterAulaPrevistaDada(Modalidade modalidade, string turmaId, string disciplinaId, int semestre = 0)
        {
            var turma = ObterTurma(turmaId);

            var tipoCalendario = ObterTipoCalendarioPorTurmaAnoLetivo(turma.AnoLetivo, turma.ModalidadeCodigo, semestre);

            AulasPrevistasDadasAuditoriaDto aulaPrevistaDto;

            var aulaPrevista = await repositorio.ObterAulaPrevistaFiltro(tipoCalendario.Id, turmaId, disciplinaId);

            var periodosAbertos = await consultasTurma.PeriodosEmAbertoTurma(turmaId, DateTime.Now);

            IEnumerable<AulaPrevistaBimestreQuantidade> aulaPrevistaBimestres;

            if (aulaPrevista != null)
                aulaPrevistaBimestres = await ObterBimestres(aulaPrevista.Id);
            else
            {
                aulaPrevista = new AulaPrevista();

                var periodosBimestre = ObterPeriodosEscolares(tipoCalendario.Id);
                aulaPrevistaBimestres = MapearPeriodoParaBimestreDto(periodosBimestre);
            }

            aulaPrevistaDto = MapearDtoRetorno(aulaPrevista, aulaPrevistaBimestres, periodosAbertos);

            return aulaPrevistaDto;
        }

        private AulasPrevistasDadasAuditoriaDto MapearDtoRetorno(AulaPrevista aulaPrevista, IEnumerable<AulaPrevistaBimestreQuantidade> aulasPrevistasBimestre, IEnumerable<PeriodoEscolarAbertoDto> periodosAbertos = null)
        {
            AulasPrevistasDadasAuditoriaDto aulaPrevistaDto = MapearParaDto(aulaPrevista, aulasPrevistasBimestre, periodosAbertos) ?? new AulasPrevistasDadasAuditoriaDto();
            aulaPrevistaDto = MapearMensagens(aulaPrevistaDto);

            return aulaPrevistaDto;
        }

        private AulasPrevistasDadasAuditoriaDto MapearMensagens(AulasPrevistasDadasAuditoriaDto aulaPrevistaDto)
        {
            foreach (var aula in aulaPrevistaDto.AulasPrevistasPorBimestre)
            {
                List<string> mensagens = new List<string>();

                if (aula.Previstas.Quantidade != (aula.Criadas.QuantidadeCJ + aula.Criadas.QuantidadeTitular) && aula.Fim.Date >= DateTime.Today)
                    mensagens.Add("Quantidade de aulas previstas diferente da quantidade de aulas criadas.");

                if (aula.Previstas.Quantidade != (aula.Cumpridas + aula.Reposicoes) && aula.Fim.Date < DateTime.Today)
                    mensagens.Add("Quantidade de aulas previstas diferente do somatório de aulas dadas + aulas repostas, após o final do bimestre.");

                if (mensagens.Any())
                    aula.Previstas.Mensagens = mensagens.ToArray();
            }

            return aulaPrevistaDto;
        }

        private AulasPrevistasDadasAuditoriaDto MapearParaDto(AulaPrevista aulaPrevista, IEnumerable<AulaPrevistaBimestreQuantidade> bimestres = null, IEnumerable<PeriodoEscolarAbertoDto> periodosAbertos = null)
        {
            var bimestre = bimestres.FirstOrDefault();

            return aulaPrevista == null ? null : new AulasPrevistasDadasAuditoriaDto
            {
                Id = aulaPrevista.Id,
                AlteradoEm = bimestre?.AlteradoEm ?? DateTime.MinValue,
                AlteradoPor = bimestre?.AlteradoPor ?? "",
                AlteradoRF = bimestre?.AlteradoRF ?? "",
                CriadoEm = bimestre?.CriadoEm ?? aulaPrevista.CriadoEm,
                CriadoPor = bimestre?.CriadoPor ?? aulaPrevista.CriadoPor,
                CriadoRF = bimestre?.CriadoRF ?? aulaPrevista.CriadoRF,
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
                    Reposicoes = x.Reposicoes,
                    PodeEditar = periodosAbertos != null ? periodosAbertos.FirstOrDefault(p => p.Bimestre == x.Bimestre).Aberto : false
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

        private async Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestres(long? aulaPrevistaId)
        {
            return await repositorioBimestre.ObterBimestresAulasPrevistasPorId(aulaPrevistaId);
        }

        private IEnumerable<PeriodoEscolar> ObterPeriodosEscolares(long tipoCalendarioId)
        {
            return repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
        }

        private TipoCalendario ObterTipoCalendarioPorTurmaAnoLetivo(int anoLetivo, Modalidade turmaModalidade, int semestre)
        {
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, ModalidadeParaModalidadeTipoCalendario(turmaModalidade), semestre);

            if (tipoCalendario == null)
                throw new NegocioException("Tipo calendário não encontrado!");

            return tipoCalendario;
        }

        private Turma ObterTurma(string turmaId)
        {
            var turma = repositorioTurma.ObterPorCodigo(turmaId);

            if (turma == null)
                throw new NegocioException("Turma não encontrada!");

            return turma;
        }
    }
}