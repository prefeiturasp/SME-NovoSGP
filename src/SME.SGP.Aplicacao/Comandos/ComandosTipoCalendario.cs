using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoCalendario : IComandosTipoCalendario
    {
        private readonly IRepositorioTipoCalendario repositorio;
        private readonly IServicoFeriadoCalendario servicoFeriadoCalendario;

        public ComandosTipoCalendario(IRepositorioTipoCalendario repositorio, IServicoFeriadoCalendario servicoFeriadoCalendario)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoFeriadoCalendario = servicoFeriadoCalendario ?? throw new ArgumentNullException(nameof(servicoFeriadoCalendario));
        }

        public TipoCalendario MapearParaDominio(TipoCalendarioDto dto)
        {
            TipoCalendario entidade = repositorio.ObterPorId(dto.Id);
            if (entidade == null)
            {
                entidade = new TipoCalendario();
            }
            entidade.Nome = dto.Nome;
            entidade.AnoLetivo = dto.AnoLetivo;
            entidade.Periodo = dto.Periodo;
            entidade.Situacao = dto.Situacao;
            entidade.Modalidade = dto.Modalidade;
            return entidade;
        }

        public void MarcarExcluidos(long[] ids)
        {
            var idsInvalidos = "";
            foreach (long id in ids)
            {
                var tipoCalendario = repositorio.ObterPorId(id);
                if (tipoCalendario != null)
                {
                    tipoCalendario.Excluido = true;
                    repositorio.Salvar(tipoCalendario);
                }
                else
                {
                    idsInvalidos += idsInvalidos.Equals("") ? $"{id}" : $", {id}";
                }
            }
            if (!idsInvalidos.Trim().Equals(""))
            {
                throw new NegocioException($"Houve um erro ao excluir os tipos de calendário ids '{idsInvalidos}'. Um dos tipos de calendário não existe");
            }
        }

        public async Task Salvar(TipoCalendarioDto dto)
        {
            var tipoCalendario = MapearParaDominio(dto);

            bool ehRegistroExistente = await repositorio.VerificarRegistroExistente(dto.Id, dto.Nome);

            if (ehRegistroExistente)
                throw new NegocioException($"O Tipo de Calendário Escolar '{dto.Nome}' já existe");

            await servicoFeriadoCalendario.VerficaSeExisteFeriadosMoveisEInclui(dto.AnoLetivo).ConfigureAwait(false);

            repositorio.Salvar(tipoCalendario);
        }
    }
}