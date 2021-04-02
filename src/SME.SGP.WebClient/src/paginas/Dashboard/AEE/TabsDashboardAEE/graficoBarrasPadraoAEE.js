import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader } from '~/componentes';
import GraficoBarraDashboard from '~/paginas/Dashboard/ComponentesDashboard/graficoBarraDashboard';
import {
  adicionarCoresNosGraficos,
  montarDadosGrafico,
} from '~/paginas/Dashboard/ComponentesDashboard/graficosDashboardUtils';
import { erros } from '~/servicos';

const GraficoBarrasPadraoAEE = props => {
  const {
    anoLetivo,
    dreId,
    ueId,
    nomeIndiceDesc,
    nomeValor,
    ServicoObterValoresGrafico,
  } = props;

  const [chavesGrafico, setChavesGrafico] = useState([]);
  const [dadosGrafico, setDadosGrafico] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);

  const OPCAO_TODOS = '-99';

  const mapearDadosGraficos = useCallback(
    dados => {
      const chaves = [];
      const dadosMapeados = [];

      dados.forEach(item => {
        chaves.push(item[nomeIndiceDesc]);
        montarDadosGrafico(item, nomeValor, dadosMapeados, nomeIndiceDesc);
      });

      setChavesGrafico(chaves);

      setDadosGrafico(adicionarCoresNosGraficos(dadosMapeados));
    },
    [nomeIndiceDesc, nomeValor]
  );

  const obterDadosGrafico = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoObterValoresGrafico(
      anoLetivo,
      dreId === OPCAO_TODOS ? '' : dreId,
      ueId === OPCAO_TODOS ? '' : ueId
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno?.data?.length) {
      mapearDadosGraficos(retorno.data);
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, dreId, ueId, mapearDadosGraficos]);

  useEffect(() => {
    if (anoLetivo && dreId && ueId) {
      obterDadosGrafico();
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, dreId, ueId, obterDadosGrafico]);

  const graficoBarras = (dados, titulo) => {
    return (
      <GraficoBarraDashboard
        titulo={titulo}
        dadosGrafico={dados}
        chavesGrafico={chavesGrafico}
        indice={nomeIndiceDesc}
        groupMode="stacked"
        removeLegends
        margemPersonalizada={{
          top: 50,
          right: 0,
          bottom: 50,
          left: 90,
        }}
      />
    );
  };

  return (
    <Loader loading={exibirLoader} className="col-md-12 text-center">
      {dadosGrafico?.length
        ? graficoBarras(dadosGrafico, ' ')
        : !exibirLoader
        ? 'Sem dados'
        : ''}
    </Loader>
  );
};

GraficoBarrasPadraoAEE.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
  nomeIndiceDesc: PropTypes.string,
  nomeValor: PropTypes.string,
  ServicoObterValoresGrafico: PropTypes.func,
};

GraficoBarrasPadraoAEE.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
  nomeIndiceDesc: '',
  nomeValor: '',
  ServicoObterValoresGrafico: () => {},
};

export default GraficoBarrasPadraoAEE;
