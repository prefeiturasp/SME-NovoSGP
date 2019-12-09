using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task Inserir(AulaPrevistaDto dto)
        {
            var aulaPrevista = await ObterAulaPrevistaPorFiltro(0, dto.TipoCalendarioId, dto.TurmaId, dto.DisciplinaId);

            unitOfWork.IniciarTransacao();

            foreach (var bimestre in dto.BimestresQuantidade)
            {
                AulaPrevista aula = aulaPrevista.Where(ap => ap.Bimestre == bimestre.Bimestre).FirstOrDefault();
                aula = MapearParaDominio(dto, bimestre, aula);
                repositorio.Salvar(aula);
            }

            unitOfWork.PersistirTransacao();
        }

        private async Task<IEnumerable<AulaPrevista>> ObterAulaPrevistaPorFiltro(int bimestre, long tipoCalendarioId, string turmaId, string disciplinaId)
        {
            return await repositorio.ObterAulasPrevistasPorFiltro(bimestre, tipoCalendarioId, turmaId, disciplinaId);
        }

        private AulaPrevista MapearParaDominio(AulaPrevistaDto aulaPrevistaDto, AulaPrevistaBimestreQuantidadeDto aulaPrevistaBimestreQuantidadeDto, AulaPrevista aulaPrevista)
        {
            if (aulaPrevista == null)
            {
                aulaPrevista = new AulaPrevista();
            }
            aulaPrevista.Bimestre = aulaPrevistaBimestreQuantidadeDto.Bimestre;
            aulaPrevista.DisciplinaId = aulaPrevistaDto.DisciplinaId;
            aulaPrevista.Quantidade = aulaPrevistaBimestreQuantidadeDto.Quantidade;
            aulaPrevista.TipoCalendarioId = aulaPrevistaDto.TipoCalendarioId;
            aulaPrevista.TurmaId = aulaPrevistaDto.TurmaId;
            return aulaPrevista;
        }
    }
}
