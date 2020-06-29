using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasTipoCalendario : IConsultasTipoCalendario
    {
        private readonly IRepositorioTipoCalendario repositorio;
        private readonly IRepositorioEvento repositorioEvento;

        public ConsultasTipoCalendario(IRepositorioTipoCalendario repositorio, IRepositorioEvento repositorioEvento)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<IEnumerable<TipoCalendarioDto>> BuscarPorAnoLetivo(int anoLetivo)
        {
            var retorno = await repositorio.BuscarPorAnoLetivo(anoLetivo);
            return from t in retorno
                   select EntidadeParaDto(t);
        }

        public async Task<TipoCalendarioCompletoDto> BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0)
        {
            var entidade = await repositorio.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade, semestre);

            if (entidade != null)
                return EntidadeParaDtoCompleto(entidade);

            return null;
        }

        public async Task<TipoCalendarioCompletoDto> BuscarPorId(long id)
        {
            var entidade = await repositorio.ObterPorIdAsync(id);

            TipoCalendarioCompletoDto dto = new TipoCalendarioCompletoDto();

            if (entidade != null)
                dto = EntidadeParaDtoCompleto(entidade);

            return dto;
        }

        private TipoCalendarioDto EntidadeParaDto(TipoCalendario entidade)
        {
            return new TipoCalendarioDto()
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                AnoLetivo = entidade.AnoLetivo,
                Modalidade = entidade.Modalidade,
                DescricaoPeriodo = entidade.Periodo.GetAttribute<DisplayAttribute>().Name,
                Periodo = entidade.Periodo,
                Migrado = entidade.Migrado
            };
        }

        private TipoCalendarioCompletoDto EntidadeParaDtoCompleto(TipoCalendario entidade)
        {
            bool possuiEventos = repositorioEvento.ExisteEventoPorTipoCalendarioId(entidade.Id);
            return new TipoCalendarioCompletoDto
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                AnoLetivo = entidade.AnoLetivo,
                Periodo = entidade.Periodo,
                Modalidade = entidade.Modalidade,
                Situacao = entidade.Situacao,
                AlteradoPor = entidade.AlteradoPor,
                CriadoRF = entidade.CriadoRF,
                AlteradoRF = entidade.AlteradoRF,
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor,
                DescricaoPeriodo = entidade.Periodo.GetAttribute<DisplayAttribute>().Name,
                PossuiEventos = possuiEventos
            };
        }

        public async Task<IEnumerable<TipoCalendarioDto>> Listar()
        {
            var retorno = await repositorio.ObterTiposCalendario();
            return from t in retorno
                   select EntidadeParaDto(t);
        }

        public async Task<IEnumerable<TipoCalendarioDto>> ListarPorAnoLetivo(int anoLetivo)
        {
            var retorno = await repositorio.ListarPorAnoLetivo(anoLetivo);
            return from t in retorno
                   select EntidadeParaDto(t);
        }

        public async Task<TipoCalendario> ObterPorTurma(Turma turma)
            => await repositorio.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo
                    , turma.ModalidadeTipoCalendario
                    , turma.Semestre);

        public async Task<bool> PeriodoEmAberto(TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false)
            => await repositorio.PeriodoEmAberto(tipoCalendario.Id, dataReferencia, bimestre, ehAnoLetivo);
    }
}