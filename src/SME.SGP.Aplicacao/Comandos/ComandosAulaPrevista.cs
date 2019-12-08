using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAulaPrevista : IComandosAulaPrevista
    {
        private readonly IRepositorioAulaPrevista repositorio;
        private readonly IUnitOfWork unitOfWork;

        public ComandosAulaPrevista(IRepositorioAulaPrevista repositorio,
                                    IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Inserir(IEnumerable<AulaPrevistaDto> dtos)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var aulaPrevista in dtos)
                {
                    AulaPrevista aula = await ObterAulaPrevistaPorFiltro(aulaPrevista.Bimestre, aulaPrevista.TipoCalendarioId, aulaPrevista.TurmaId, aulaPrevista.DisciplinaId);
                    aula = MapearParaDominio(aulaPrevista, aula);
                    repositorio.Salvar(aula);
                }
                unitOfWork.PersistirTransacao();
            }
        }

        private async Task<AulaPrevista> ObterAulaPrevistaPorFiltro(int bimestre, long tipoCalendarioId, string turmaId, string disciplinaId)
        {
            return await repositorio.ObterAulaPrevistaPorFiltro(bimestre, tipoCalendarioId, turmaId, disciplinaId);
        }

        private AulaPrevista MapearParaDominio(AulaPrevistaDto aulaPrevistaDto, AulaPrevista aulaPrevista)
        {
            if (aulaPrevista == null)
            {
                aulaPrevista = new AulaPrevista();
            }
            aulaPrevista.Bimestre = aulaPrevistaDto.Bimestre;
            aulaPrevista.DisciplinaId = aulaPrevistaDto.DisciplinaId;
            aulaPrevista.Quantidade = aulaPrevistaDto.Quantidade;
            aulaPrevista.TipoCalendarioId = aulaPrevistaDto.TipoCalendarioId;
            aulaPrevista.TurmaId = aulaPrevistaDto.TurmaId;
            return aulaPrevista;
        }
    }
}
