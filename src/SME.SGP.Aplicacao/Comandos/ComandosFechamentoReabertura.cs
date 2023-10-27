using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoReabertura : IComandosFechamentoReabertura
    {
        public readonly IRepositorioDreConsulta repositorioDre;
        public readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendarioConsulta;
        public readonly IRepositorioUeConsulta repositorioUe;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoFechamentoReabertura servicoFechamentoReabertura;

        public ComandosFechamentoReabertura(IRepositorioDreConsulta repositorioDre, IRepositorioUeConsulta repositorioUe,
                                            IRepositorioTipoCalendarioConsulta repositorioTipoCalendarioConsulta, IServicoFechamentoReabertura servicoFechamentoReabertura,
                                            IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioTipoCalendarioConsulta = repositorioTipoCalendarioConsulta ?? throw new ArgumentNullException(nameof(repositorioTipoCalendarioConsulta));
            this.servicoFechamentoReabertura = servicoFechamentoReabertura ?? throw new ArgumentNullException(nameof(servicoFechamentoReabertura));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<string> Alterar(FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto, long id)
        {
            var fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(id, 0);
            if (fechamentoReabertura.EhNulo())
                throw new NegocioException("Não foi possível localizar esta Reabertura de Fechamento.");

            fechamentoReabertura = await TransformarDtoEmEntidadeParaPersistencia(fechamentoReaberturaPersistenciaDto, fechamentoReabertura);

            return await servicoFechamentoReabertura.AlterarAsync(fechamentoReabertura, fechamentoReaberturaPersistenciaDto.Bimestres);
        }

        public async Task<string> Excluir(long[] ids)
        {
            var Mensagens = new List<string>();

            var fechamentos = await repositorioFechamentoReabertura.ObterPorIds(ids);
            if (fechamentos.EhNulo() || !fechamentos.Any())
                throw new NegocioException("Não foram localizados fechamento(s) válido(s) para exclusão.");
            else
            {
                foreach (var fechamento in fechamentos)
                {
                    var mensagem = await servicoFechamentoReabertura.ExcluirAsync(fechamento);
                    if (!string.IsNullOrEmpty(mensagem))
                        Mensagens.Add(mensagem);
                }
            }

            return string.Join(" <br />", Mensagens);
        }

        public async Task<string> Salvar(FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto)
        {
            FechamentoReabertura entidade = await TransformarDtoEmEntidadeParaPersistencia(fechamentoReaberturaPersistenciaDto, null);
            return await servicoFechamentoReabertura.SalvarAsync(entidade);
        }

        private async Task<FechamentoReabertura> TransformarDtoEmEntidadeParaPersistencia(FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto, FechamentoReabertura fechamentoReaberturaExistenteDto)
        {
            Dre dre = null;
            Ue ue = null;

            if (!string.IsNullOrEmpty(fechamentoReaberturaPersistenciaDto.DreCodigo))
            {
                dre = await repositorioDre.ObterPorCodigo(fechamentoReaberturaPersistenciaDto.DreCodigo);
                if (dre.EhNulo())
                    throw new NegocioException("Não foi possível localizar a Dre.");
            }

            if (!string.IsNullOrEmpty(fechamentoReaberturaPersistenciaDto.UeCodigo))
            {
                ue = repositorioUe.ObterPorCodigo(fechamentoReaberturaPersistenciaDto.UeCodigo);
                if (ue.EhNulo())
                    throw new NegocioException("Não foi possível localizar a UE.");
            }

            var tipoCalendario = repositorioTipoCalendarioConsulta.ObterPorId(fechamentoReaberturaPersistenciaDto.TipoCalendarioId);
            if (tipoCalendario.EhNulo())
                throw new NegocioException("Não foi possível localizar o Tipo de Calendário.");

            FechamentoReabertura fechamentoReabertura;          

            if (fechamentoReaberturaExistenteDto.NaoEhNulo())
                fechamentoReabertura = fechamentoReaberturaExistenteDto;
            else
            {
                fechamentoReabertura = new FechamentoReabertura();

                fechamentoReaberturaPersistenciaDto.Bimestres.ToList().ForEach(bimestre =>
                {
                    fechamentoReabertura.Adicionar(new FechamentoReaberturaBimestre()
                    {
                        Bimestre = bimestre
                    });
                });
            }

            fechamentoReabertura.Descricao = fechamentoReaberturaPersistenciaDto.Descricao;
            fechamentoReabertura.Fim = fechamentoReaberturaPersistenciaDto.Fim;
            fechamentoReabertura.Inicio = fechamentoReaberturaPersistenciaDto.Inicio;

            fechamentoReabertura.AtualizarDre(dre);
            fechamentoReabertura.AtualizarUe(ue);
            fechamentoReabertura.AtualizarTipoCalendario(tipoCalendario);

            return fechamentoReabertura;
        }
    }
}