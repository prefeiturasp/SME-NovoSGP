import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { CoresGraficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';
import {
  mapearParaDtoDadosComunicadosGraficoBarras,
  obterComunicadoId,
} from '../../dashboardEscolaAquiGraficosUtils';
import GraficoBarraDashboardEscolaAqui from '../ComponentesDashboardEscolaAqui/graficoBarraDashboardEscolaAqui';

const LeituraDeComunicadosAgrupadosPorDre = props => {
  const {
    codigoDre,
    codigoUe,
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

  const OPCAO_TODOS = '-99';

  const obterDadosDeLeituraDeComunicadosAgrupadosPorDre = useCallback(async () => {
    const comunicadoId = obterComunicadoId(comunicado, listaComunicado);

    if (comunicadoId) {
      setExibirLoader(true);
      const resposta = await ServicoDashboardEscolaAqui.obterDadosDeLeituraDeComunicadosAgrupadosPorDre(
        comunicadoId,
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
  }, [modoVisualizacao, comunicado, chavesGrafico, listaComunicado]);

  useEffect(() => {
    if (
      comunicado &&
      codigoDre === OPCAO_TODOS &&
      codigoUe === OPCAO_TODOS &&
      listaComunicado?.length
    ) {
      obterDadosDeLeituraDeComunicadosAgrupadosPorDre();
    } else {
      setDadosDeLeituraDeComunicadosAgrupadosPorDre([]);
    }
  }, [
    codigoDre,
    codigoUe,
    comunicado,
    listaComunicado,
    obterDadosDeLeituraDeComunicadosAgrupadosPorDre,
  ]);

  return (
    <div className="col-md-12">
      <Loader
        loading={exibirLoader}
        tip="Carregando Total de Comunicados por DRE"
      >
        {dadosDeLeituraDeComunicadosAgrupadosPorDre?.length ? (
          <GraficoBarraDashboardEscolaAqui
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
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
  modoVisualizacao: PropTypes.oneOfType([PropTypes.string]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  comunicado: PropTypes.oneOfType([PropTypes.string]),
  listaComunicado: PropTypes.oneOfType([PropTypes.array]),
};

LeituraDeComunicadosAgrupadosPorDre.defaultProps = {
  codigoDre: '',
  codigoUe: '',
  modoVisualizacao: '',
  chavesGrafico: [],
  comunicado: '',
  listaComunicado: [],
};

export default LeituraDeComunicadosAgrupadosPorDre;
