using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IReceberRelatorioProntoUseCase
    {
        void Executar(DadosRelatorioDto dadosRelatorioDto);
    }
}
