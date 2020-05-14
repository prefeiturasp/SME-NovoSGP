using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosAulaPrevista : IComandosAulaPrevista
    {
        private readonly IRepositorioAulaPrevista repositorio;
        private readonly IRepositorioAulaPrevistaBimestre repositorioBimestre;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosAulaPrevista(IRepositorioAulaPrevista repositorio,
                                    IRepositorioAulaPrevistaBimestre repositorioBimestre,
                                    IRepositorioTurma repositorioTurma,
                                     IRepositorioTipoCalendario repositorioTipoCalendario,
                                    IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioBimestre = repositorioBimestre ?? throw new ArgumentNullException(nameof(repositorioBimestre));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<string> Alterar(AulaPrevistaDto aulaPrevistaDto, long id)
        {
            IEnumerable<AulaPrevistaBimestre> aulasPrevistasBimestre = await repositorioBimestre.ObterBimestresAulasPrevistasPorId(id);

            unitOfWork.IniciarTransacao();

            foreach (var bimestre in aulaPrevistaDto.BimestresQuantidade)
            {
                AulaPrevistaBimestre aulaPrevistaBimestre = aulasPrevistasBimestre.Where(b => b.Bimestre == bimestre.Bimestre).FirstOrDefault();
                aulaPrevistaBimestre = MapearParaDominio(id, bimestre, aulaPrevistaBimestre);
                repositorioBimestre.Salvar(aulaPrevistaBimestre);
            }

            unitOfWork.PersistirTransacao();

            return "Alteração realizada com sucesso";
        }

        public async Task<long> Inserir(AulaPrevistaDto aulaPrevistaDto)
        {
            var turma = ObterTurma(aulaPrevistaDto.TurmaId);

            var tipoCalendario = ObterTipoCalendarioPorTurmaAnoLetivo(turma.AnoLetivo, turma.ModalidadeCodigo);

            long id;

            AulaPrevista aulaPrevista = null;
            aulaPrevista = MapearParaDominio(aulaPrevistaDto, aulaPrevista, tipoCalendario.Id);

            unitOfWork.IniciarTransacao();

            id = await Inserir(aulaPrevistaDto, aulaPrevista);

            unitOfWork.PersistirTransacao();

            return id;
        }

        private async Task<long> Inserir(AulaPrevistaDto aulaPrevistaDto, AulaPrevista aulaPrevista)
        {
            aulaPrevistaDto.Id = repositorio.Salvar(aulaPrevista);

            if (aulaPrevistaDto.BimestresQuantidade != null)
                foreach (var bimestreQuantidadeDto in aulaPrevistaDto.BimestresQuantidade)
                    await repositorioBimestre.SalvarAsync(new AulaPrevistaBimestre()
                    {
                        AulaPrevistaId = aulaPrevistaDto.Id,
                        Bimestre = bimestreQuantidadeDto.Bimestre,
                        Previstas = bimestreQuantidadeDto.Quantidade
                    });

            return aulaPrevistaDto.Id;
        }

        private Turma ObterTurma(string turmaId)
        {
            var turma = repositorioTurma.ObterPorCodigo(turmaId);

            if (turma == null)
                throw new NegocioException("Turma não encontrada!");

            return turma;
        }

        private TipoCalendario ObterTipoCalendarioPorTurmaAnoLetivo(int anoLetivo, Modalidade turmaModalidade)
        {
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, ModalidadeParaModalidadeTipoCalendario(turmaModalidade));

            if (tipoCalendario == null)
                throw new NegocioException("Tipo calendário não encontrado!");

            return tipoCalendario;
        }

        private AulaPrevista MapearParaDominio(AulaPrevistaDto aulaPrevistaDto, AulaPrevista aulaPrevista, long tipoCalendarioId)
        {
            if (aulaPrevista == null)
            {
                aulaPrevista = new AulaPrevista();
            }
            aulaPrevista.DisciplinaId = aulaPrevistaDto.DisciplinaId;
            aulaPrevista.TipoCalendarioId = tipoCalendarioId;
            aulaPrevista.TurmaId = aulaPrevistaDto.TurmaId;
            return aulaPrevista;
        }

        private AulaPrevistaBimestre MapearParaDominio(long aulaPrevistaId,
                                               AulaPrevistaBimestreQuantidadeDto bimestreQuantidadeDto,
                                               AulaPrevistaBimestre aulaPrevistaBimestre)
        {
            if (aulaPrevistaBimestre == null)
            {
                aulaPrevistaBimestre = new AulaPrevistaBimestre();
            }
            aulaPrevistaBimestre.AulaPrevistaId = aulaPrevistaId;
            aulaPrevistaBimestre.Bimestre = bimestreQuantidadeDto.Bimestre;
            aulaPrevistaBimestre.Previstas = bimestreQuantidadeDto.Quantidade;
            return aulaPrevistaBimestre;
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
    }
}
