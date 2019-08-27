using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAnual : IComandosPlanoAnual
    {
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                  IUnitOfWork unitOfWork)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanoAnual));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Salvar(PlanoAnualDto planoAnualDto)
        {
            PlanoAnual planoAnual = ObterPlanoAnual(planoAnualDto);
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                planoAnualDto.Id = repositorioPlanoAnual.Salvar(planoAnual);
                unitOfWork.PersistirTransacao();
            }
        }

        private void MapearParaDominio(PlanoAnualDto planoAnualDto, PlanoAnual planoAnual)
        {
            planoAnual.Ano = planoAnualDto.Ano;
            planoAnual.Bimestre = planoAnualDto.Bimestre;
            planoAnual.Descricao = planoAnualDto.Descricao;
            planoAnual.EscolaId = planoAnualDto.EscolaId;
            planoAnual.TurmaId = planoAnualDto.TurmaId;
        }

        private PlanoAnual ObterPlanoAnual(PlanoAnualDto planoAnualDto)
        {
            var planoAnual = new PlanoAnual();
            if (planoAnualDto.Id > 0)
            {
                var planoAnualCompleto = repositorioPlanoAnual.ObterPorAnoEscolaBimestreETurma(planoAnualDto.Ano,
                                                                                               planoAnualDto.EscolaId,
                                                                                               planoAnualDto.TurmaId,
                                                                                               planoAnualDto.Bimestre);
            }
            else
            {
                var planoExistente = repositorioPlanoAnual.ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(planoAnualDto.Ano, planoAnualDto.EscolaId, planoAnualDto.TurmaId, planoAnualDto.Bimestre);
                if (planoExistente)
                {
                    throw new NegocioException("Já existe um plano anual com os dados informados.");
                }
            }
            MapearParaDominio(planoAnualDto, planoAnual);
            return planoAnual;
        }
    }
}