import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader } from '~/componentes';
import {
  adicionarCoresNosGraficos,
  formataMilhar,
} from '~/paginas/Dashboard/ComponentesDashboard/graficosDashboardUtils';
import GraficoBarraDashboard from '~/paginas/Dashboard/ComponentesDashboard/graficoBarraDashboard';
import { erros } from '~/servicos';
import ServicoDashboardAEE from '~/servicos/Paginas/Dashboard/ServicoDashboardAEE';

const GraficoQuantidadeEncaminhamentosSituacao = props => {
  const { anoLetivo, codigoDre, codigoUe } = props;

  const [chavesGrafico, setChavesGrafico] = useState([]);
  const [dadosGrafico, setDadosGrafico] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);

  const montarDadosGrafico = (item, nomeCampo, dadosMapeados) => {
    if (item[nomeCampo]) {
      const novosDadosMap = {
        descricaoSituacao: item.descricaoSituacao,
      };
      novosDadosMap[nomeCampo] = item[nomeCampo];
      novosDadosMap[item.descricaoSituacao] = formataMilhar(item[nomeCampo]);
      dadosMapeados.push(novosDadosMap);
    }
  };

  const mapearDadosGraficos = useCallback(dados => {
    const chaves = [];
    const dadosMapeados = [];

    dados.forEach(item => {
      chaves.push(item.descricaoSituacao);
      montarDadosGrafico(item, 'quantidade', dadosMapeados);
    });

    setChavesGrafico(chaves);

    setDadosGrafico(adicionarCoresNosGraficos(dadosMapeados));
  }, []);

  const obterDadosGrafico = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardAEE.obterQuantidadeEncaminhamentosPorSituacao(
      anoLetivo,
      codigoDre,
      codigoUe
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno?.data?.length) {
      mapearDadosGraficos(retorno.data);
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, codigoDre, codigoUe, mapearDadosGraficos]);

  useEffect(() => {
    if (anoLetivo && codigoDre && codigoUe) {
      obterDadosGrafico();
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, codigoDre, codigoUe, obterDadosGrafico]);

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
      {dadosGrafico?.length ? graficoBarras(dadosGrafico, ' ') : 'Sem dados'}
    </Loader>
  );
};

GraficoQuantidadeEncaminhamentosSituacao.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  codigoDre: PropTypes.string,
  codigoUe: PropTypes.string,
};

GraficoQuantidadeEncaminhamentosSituacao.defaultProps = {
  anoLetivo: null,
  codigoDre: '',
  codigoUe: '',
};

export default GraficoQuantidadeEncaminhamentosSituacao;
