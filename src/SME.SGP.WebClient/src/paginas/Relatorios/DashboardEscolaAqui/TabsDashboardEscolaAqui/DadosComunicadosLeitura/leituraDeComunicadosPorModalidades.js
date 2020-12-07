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

const LeituraDeComunicadosPorModalidades = props => {
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
    dadosDeLeituraDeComunicadosAgrupadosPorModalidade,
    setDadosDeLeituraDeComunicadosAgrupadosPorModalidade,
  ] = useState([]);

  const [dadosLegendaGrafico, setDadosLegendaGrafico] = useState([]);

  const OPCAO_TODOS = '-99';

  const obterDadosDeLeituraDeComunicadosAgrupadosPorModalidade = useCallback(async () => {
    const comunicadoId = obterComunicadoId(comunicado, listaComunicado);

    if (comunicadoId) {
      setExibirLoader(true);
      const resposta = await ServicoDashboardEscolaAqui.obterDadosDeLeituraDeComunicadosPorModalidades(
        codigoDre === OPCAO_TODOS ? codigoDre || '' : '',
        codigoUe === OPCAO_TODOS ? codigoUe || '' : '',
        comunicadoId,
        modoVisualizacao
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data?.length) {
        const retornoDados = mapearParaDtoDadosComunicadosGraficoBarras(
          resposta.data,
          'modalidade',
          chavesGrafico
        );
        if (retornoDados?.dadosLegendaGrafico?.length) {
          setDadosLegendaGrafico(retornoDados.dadosLegendaGrafico);
        }
        if (retornoDados?.dadosComunicadosGraficoBarras?.length) {
          setDadosDeLeituraDeComunicadosAgrupadosPorModalidade(
            retornoDados.dadosComunicadosGraficoBarras
          );
        }
      } else {
        setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]);
      }
    }
  }, [
    codigoDre,
    codigoUe,
    modoVisualizacao,
    comunicado,
    chavesGrafico,
    listaComunicado,
  ]);

  useEffect(() => {
    if (comunicado && codigoDre && codigoUe && listaComunicado?.length) {
      obterDadosDeLeituraDeComunicadosAgrupadosPorModalidade();
    } else {
      setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]);
    }
  }, [codigoDre, codigoUe, comunicado, listaComunicado]);

  return (
    <div className="col-md-12">
      <Loader
        loading={exibirLoader}
        tip="Carregando Total de leituras por Modalidade"
      >
        {dadosDeLeituraDeComunicadosAgrupadosPorModalidade?.length ? (
          <GraficoBarraDashboardEscolaAqui
            titulo="Total de leituras por Modalidade"
            dadosGrafico={dadosDeLeituraDeComunicadosAgrupadosPorModalidade}
            chavesGrafico={chavesGrafico}
            indice="modalidade"
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

LeituraDeComunicadosPorModalidades.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
  modoVisualizacao: PropTypes.oneOfType([PropTypes.string]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  comunicado: PropTypes.oneOfType([PropTypes.string]),
  listaComunicado: PropTypes.oneOfType([PropTypes.array]),
};

LeituraDeComunicadosPorModalidades.defaultProps = {
  codigoDre: '',
  codigoUe: '',
  modoVisualizacao: '',
  chavesGrafico: [],
  comunicado: '',
  listaComunicado: [],
};

export default LeituraDeComunicadosPorModalidades;
