import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import {
  adicionarCoresNosGraficos,
  montarDadosGrafico,
  mapearParaDtoGraficoPizzaComValorEPercentual,
} from '../../../ComponentesDashboard/graficosDashboardUtils';
import DataUltimaAtualizacaoDashboardEscolaAqui from '../ComponentesDashboardEscolaAqui/dataUltimaAtualizacaoDashboardEscolaAqui';
import GraficoBarraDashboard from '~/paginas/Dashboard/ComponentesDashboard/graficoBarraDashboard';
import GraficoPizzaDashboard from '~/paginas/Dashboard/ComponentesDashboard/graficoPizzaDashboard';

const DadosAdesao = props => {
  const { codigoDre, codigoUe } = props;

  const [dadosGraficoAdesao, setDadosGraficoAdesao] = useState([]);
  const [chavesGrafico, setChavesGrafico] = useState([]);

  const [
    dadosGraficoTotalUsuariosSemAppInstalado,
    setDadosGraficoTotalUsuariosSemAppInstalado,
  ] = useState([]);

  const [
    dadosGraficoTotalUsuariosComCpfInvalidos,
    setDadosGraficoTotalUsuariosComCpfInvalidos,
  ] = useState([]);

  const [
    dadosGraficoTotalUsuariosPrimeiroAcessoIncompleto,
    setDadosGraficoTotalUsuariosPrimeiroAcessoIncompleto,
  ] = useState([]);

  const [
    dadosGraficoTotalUsuariosValidos,
    setDadosGraficoTotalUsuariosValidos,
  ] = useState([]);

  const [exibirLoader, setExibirLoader] = useState(false);

  const OPCAO_TODOS = '-99';

  const mapearParaDtoGraficoPizza = dados => {
    const dadosParaMapear = [];

    if (dados.totalUsuariosComCpfInvalidos) {
      const totalUsuariosComCpfInvalidos = {
        label: 'Responsáveis sem CPF ou com CPF inválido no EOL',
        value: dados.totalUsuariosComCpfInvalidos || 0,
      };
      dadosParaMapear.push(totalUsuariosComCpfInvalidos);
    }

    if (dados.totalUsuariosSemAppInstalado) {
      const totalUsuariosSemAppInstalado = {
        label: 'Usuários que não realizaram a instalação',
        value: dados.totalUsuariosSemAppInstalado || 0,
      };
      dadosParaMapear.push(totalUsuariosSemAppInstalado);
    }

    if (dados.totalUsuariosPrimeiroAcessoIncompleto) {
      const totalUsuariosPrimeiroAcessoIncompleto = {
        label: 'Usuários com primeiro acesso incompleto',
        value: dados.totalUsuariosPrimeiroAcessoIncompleto || 0,
      };
      dadosParaMapear.push(totalUsuariosPrimeiroAcessoIncompleto);
    }

    if (dados.totalUsuariosValidos) {
      const totalUsuariosValidos = {
        label: 'Usuários válidos',
        value: dados.totalUsuariosValidos || 0,
      };
      dadosParaMapear.push(totalUsuariosValidos);
    }

    const dadosMapeadosComPorcentagem = mapearParaDtoGraficoPizzaComValorEPercentual(
      dadosParaMapear
    );
    setDadosGraficoAdesao(dadosMapeadosComPorcentagem);
  };

  const obterDadosGraficoAdesao = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardEscolaAqui.obterDadosGraficoAdesao(
      codigoDre === OPCAO_TODOS ? '' : codigoDre,
      codigoUe === OPCAO_TODOS ? '' : codigoUe
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno && retorno.data && retorno.data.length) {
      mapearParaDtoGraficoPizza(retorno.data[0]);
    } else {
      setDadosGraficoAdesao([]);
    }
  }, [codigoDre, codigoUe]);

  const mapearDadosGraficos = useCallback(dados => {
    const chaves = [];
    const dadosTotalUsuariosComCpfInvalidos = [];
    const dadosTotalUsuariosSemAppInstalado = [];
    const dadosTotalUsuariosPrimeiroAcessoIncompleto = [];
    const dadosTotalUsuariosValidos = [];

    dados.forEach(item => {
      chaves.push(item.nomeCompletoDre);

      montarDadosGrafico(
        item,
        'totalUsuariosComCpfInvalidos',
        dadosTotalUsuariosComCpfInvalidos,
        'nomeCompletoDre'
      );

      montarDadosGrafico(
        item,
        'totalUsuariosSemAppInstalado',
        dadosTotalUsuariosSemAppInstalado,
        'nomeCompletoDre'
      );

      montarDadosGrafico(
        item,
        'totalUsuariosPrimeiroAcessoIncompleto',
        dadosTotalUsuariosPrimeiroAcessoIncompleto,
        'nomeCompletoDre'
      );

      montarDadosGrafico(
        item,
        'totalUsuariosValidos',
        dadosTotalUsuariosValidos,
        'nomeCompletoDre'
      );
    });

    setChavesGrafico(chaves);
    setDadosGraficoTotalUsuariosComCpfInvalidos(
      adicionarCoresNosGraficos(dadosTotalUsuariosComCpfInvalidos)
    );
    setDadosGraficoTotalUsuariosSemAppInstalado(
      adicionarCoresNosGraficos(dadosTotalUsuariosSemAppInstalado)
    );
    setDadosGraficoTotalUsuariosPrimeiroAcessoIncompleto(
      adicionarCoresNosGraficos(dadosTotalUsuariosPrimeiroAcessoIncompleto)
    );
    setDadosGraficoTotalUsuariosValidos(
      adicionarCoresNosGraficos(dadosTotalUsuariosValidos)
    );
  }, []);

  const limparGraficosTotais = () => {
    setDadosGraficoTotalUsuariosComCpfInvalidos([]);
    setDadosGraficoTotalUsuariosSemAppInstalado([]);
    setDadosGraficoTotalUsuariosPrimeiroAcessoIncompleto([]);
    setDadosGraficoTotalUsuariosValidos([]);
  };

  const obterDadosGraficoAdesaoAgrupados = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardEscolaAqui.obterDadosGraficoAdesaoAgrupados()
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno && retorno.data && retorno.data.length) {
      mapearDadosGraficos(retorno.data);
    } else {
      limparGraficosTotais();
    }
  }, [mapearDadosGraficos]);

  useEffect(() => {
    if (codigoDre && codigoUe) {
      if (codigoDre === OPCAO_TODOS && codigoUe === OPCAO_TODOS) {
        obterDadosGraficoAdesaoAgrupados();
      } else {
        limparGraficosTotais();
      }
      obterDadosGraficoAdesao();
    } else {
      setDadosGraficoAdesao([]);
    }
  }, [
    codigoDre,
    codigoUe,
    obterDadosGraficoAdesao,
    obterDadosGraficoAdesaoAgrupados,
  ]);

  const graficoBarras = (dados, titulo) => {
    return (
      <GraficoBarraDashboard
        titulo={titulo}
        dadosGrafico={dados}
        chavesGrafico={chavesGrafico}
        indice="nomeCompletoDre"
        groupMode="stacked"
      />
    );
  };

  return (
    <Loader loading={exibirLoader} className="text-center">
      <div className="col-md-12 mt-2">
        <DataUltimaAtualizacaoDashboardEscolaAqui nomeConsulta="ConsolidarAdesaoEOL" />
      </div>

      {dadosGraficoAdesao && dadosGraficoAdesao.length ? (
        <>
          <GraficoPizzaDashboard
            titulo="Total de Usuários"
            dadosGrafico={dadosGraficoAdesao}
          />

          {dadosGraficoTotalUsuariosComCpfInvalidos?.length
            ? graficoBarras(
                dadosGraficoTotalUsuariosComCpfInvalidos,
                'Responsáveis sem CPF ou com CPF inválido no EOL'
              )
            : ''}

          {dadosGraficoTotalUsuariosSemAppInstalado?.length
            ? graficoBarras(
                dadosGraficoTotalUsuariosSemAppInstalado,
                'Responsáveis que não realizaram a instalação'
              )
            : ''}

          {dadosGraficoTotalUsuariosPrimeiroAcessoIncompleto?.length
            ? graficoBarras(
                dadosGraficoTotalUsuariosPrimeiroAcessoIncompleto,
                ' Responsáveis com primeiro acesso incompleto'
              )
            : ''}

          {dadosGraficoTotalUsuariosValidos?.length
            ? graficoBarras(
                dadosGraficoTotalUsuariosValidos,
                ' Responsáveis válidos'
              )
            : ''}
        </>
      ) : (
        'Sem dados'
      )}
    </Loader>
  );
};

DadosAdesao.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
};

DadosAdesao.defaultProps = {
  codigoDre: '',
  codigoUe: '',
};

export default DadosAdesao;
