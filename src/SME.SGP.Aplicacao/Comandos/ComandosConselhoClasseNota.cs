using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosConselhoClasseNota : IComandosConselhoClasseNota
    {
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;

        public ComandosConselhoClasseNota(IRepositorioConselhoClasseNota repositorioConselhoClasseNota, IServicoConselhoClasseNota)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }

        public async Task Persistir(PosConselhosNotasPersistirDto posConselhosNotasPersistirDto)
        {
            var notaPosConselho = await repositorioConselhoClasseNota.ObterPorFiltrosAsync(posConselhosNotasPersistirDto.AlunoCodigo, posConselhosNotasPersistirDto.Bimestre,
                posConselhosNotasPersistirDto.ComponenteCurricularCodigo, posConselhosNotasPersistirDto.TurmaCodigo);

            if (notaPosConselho == null)
            {
                notaPosConselho = TransformaPosConselhosNotasPersistirDtoEmEntidade(posConselhosNotasPersistirDto);
            }
        }

        private ConselhoClasseNota TransformaPosConselhosNotasPersistirDtoEmEntidade(PosConselhosNotasPersistirDto posConselhosNotasPersistirDto)
        {
            var conselhoClasseNota = new ConselhoClasseNota();

            throw new NotImplementedException();
        }
    }
}