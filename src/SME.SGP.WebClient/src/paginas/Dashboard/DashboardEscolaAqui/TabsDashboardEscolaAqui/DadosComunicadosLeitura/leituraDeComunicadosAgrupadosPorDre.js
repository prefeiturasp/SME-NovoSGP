import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { CoresGraficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import {
  mapearParaDtoDadosComunicadosGraficoBarras,
  obterDadosComunicadoSelecionado,
} from '../../../ComponentesDashboard/graficosDashboardUtils';
import GraficoBarraDashboard from '../../../ComponentesDashboard/graficoBarraDashboard';

const LeituraDeComunicadosAgrupadosPorDre = props => {
  const {
    chavesGrafico,
    modoVisualizacao,
    comunicado,
    listaComunicado,
  } = props;

  const [exibirLoader, setExibirLoader] = useState(false);

  const [
    dadosDeLeituraDeComunicadosAgrupadosPorDre,
    setDadosDeLeituraDeComunicadosAgrupadosPorDre,
  ] = useState([]);

  const [dadosLegendaGrafico, setDadosLegendaGrafico] = useState([]);

  const obterDadosDeLeituraDeComunicadosAgrupadosPorDre = useCallback(
    async dadosComunicado => {
      if (dadosComunicado?.id) {
        setExibirLoader(true);
        const resposta = await ServicoDashboardEscolaAqui.obterDadosDeLeituraDeComunicadosAgrupadosPorDre(
          dadosComunicado.id,
          modoVisualizacao
        )
          .catch(e => erros(e))
          .finally(() => setExibirLoader(false));

        if (resposta?.data?.length) {
          const retornoDados = mapearParaDtoDadosComunicadosGraficoBarras(
            resposta.data,
            'nomeAbreviadoDre',
            chavesGrafico
          );
          if (retornoDados?.dadosLegendaGrafico?.length) {
            setDadosLegendaGrafico(retornoDados.dadosLegendaGrafico);
          }
          if (retornoDados?.dadosComunicadosGraficoBarras?.length) {
            setDadosDeLeituraDeComunicadosAgrupadosPorDre(
              retornoDados.dadosComunicadosGraficoBarras
            );
          }
        } else {
          setDadosDeLeituraDeComunicadosAgrupadosPorDre([]);
        }
      }
    },
    [modoVisualizacao, chavesGrafico]
  );

  useEffect(() => {
    return () => {
      setDadosDeLeituraDeComunicadosAgrupadosPorDre([]);
    };
  }, []);

  useEffect(() => {
    setDadosDeLeituraDeComunicadosAgrupadosPorDre([]);
    if (comunicado && listaComunicado?.length) {
      const dadosComunicado = obterDadosComunicadoSelecionado(
        comunicado,
        listaComunicado
      );
      if (
        dadosComunicado?.id &&
        !dadosComunicado?.codigoDre &&
        !dadosComunicado?.codigoUe
      ) {
        obterDadosDeLeituraDeComunicadosAgrupadosPorDre(dadosComunicado);
      }
    }
  }, [comunicado, modoVisualizacao, listaComunicado]);

  return (
    <div className="col-md-12">
      <Loader
        loading={exibirLoader}
        tip="Carregando Total de Comunicados por DRE"
      >
        {dadosDeLeituraDeComunicadosAgrupadosPorDre?.length ? (
          <GraficoBarraDashboard
            titulo="Total de Comunicados por DRE"
            dadosGrafico={dadosDeLeituraDeComunicadosAgrupadosPorDre}
            chavesGrafico={chavesGrafico}
            indice="nomeAbreviadoDre"
            dadosLegendaCustomizada={dadosLegendaGrafico}
            removeLegends
            customPropsColors={item => {
              if (item.id === chavesGrafico[0]) {
                return CoresGraficos[0];
              }
              if (item.id === chavesGrafico[1]) {
                return CoresGraficos[1];
              }
              return CoresGraficos[2];
            }}
          />
        ) : (
          ''
        )}
      </Loader>
    </div>
  );
};

LeituraDeComunicadosAgrupadosPorDre.propTypes = {
  modoVisualizacao: PropTypes.oneOfType([PropTypes.string]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  comunicado: PropTypes.oneOfType([PropTypes.string]),
  listaComunicado: PropTypes.oneOfType([PropTypes.array]),
};

LeituraDeComunicadosAgrupadosPorDre.defaultProps = {
  modoVisualizacao: '',
  chavesGrafico: [],
  comunicado: '',
  listaComunicado: [],
};

export default LeituraDeComunicadosAgrupadosPorDre;
