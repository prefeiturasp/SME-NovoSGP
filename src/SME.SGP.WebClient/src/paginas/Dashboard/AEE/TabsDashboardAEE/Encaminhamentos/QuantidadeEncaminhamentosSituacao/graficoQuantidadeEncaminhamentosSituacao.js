import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader } from '~/componentes';
import {
  adicionarCoresNosGraficos,
  montarDadosGrafico,
} from '~/paginas/Dashboard/ComponentesDashboard/graficosDashboardUtils';
import GraficoBarraDashboard from '~/paginas/Dashboard/ComponentesDashboard/graficoBarraDashboard';
import { erros } from '~/servicos';
import ServicoDashboardAEE from '~/servicos/Paginas/Dashboard/ServicoDashboardAEE';

const GraficoQuantidadeEncaminhamentosSituacao = props => {
  const { anoLetivo, dreId, ueId } = props;

  const [chavesGrafico, setChavesGrafico] = useState([]);
  const [dadosGrafico, setDadosGrafico] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);

  const OPCAO_TODOS = '-99';

  const mapearDadosGraficos = useCallback(dados => {
    const chaves = [];
    const dadosMapeados = [];

    dados.forEach(item => {
      chaves.push(item.descricaoSituacao);
      montarDadosGrafico(
        item,
        'quantidade',
        dadosMapeados,
        'descricaoSituacao'
      );
    });

    setChavesGrafico(chaves);

    setDadosGrafico(adicionarCoresNosGraficos(dadosMapeados));
  }, []);

  const obterDadosGrafico = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardAEE.obterQuantidadeEncaminhamentosPorSituacao(
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
        indice="descricaoSituacao"
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

GraficoQuantidadeEncaminhamentosSituacao.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
};

GraficoQuantidadeEncaminhamentosSituacao.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
};

export default GraficoQuantidadeEncaminhamentosSituacao;
