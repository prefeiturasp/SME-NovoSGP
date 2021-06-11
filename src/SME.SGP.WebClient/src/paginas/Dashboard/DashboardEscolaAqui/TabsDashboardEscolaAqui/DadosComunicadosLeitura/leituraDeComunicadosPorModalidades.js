import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { CoresGraficos, Loader } from '~/componentes';
import { setDadosDeLeituraDeComunicadosAgrupadosPorModalidade } from '~/redux/modulos/dashboardEscolaAqui/actions';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import {
  mapearParaDtoDadosComunicadosGraficoBarras,
  obterDadosComunicadoSelecionado,
} from '../../../ComponentesDashboard/graficosDashboardUtils';
import GraficoBarraDashboard from '../../../ComponentesDashboard/graficoBarraDashboard';

const LeituraDeComunicadosPorModalidades = props => {
  const {
    chavesGrafico,
    modoVisualizacao,
    comunicado,
    listaComunicado,
  } = props;

  const [exibirLoader, setExibirLoader] = useState(false);

  const dispatch = useDispatch();

  const dadosDeLeituraDeComunicadosAgrupadosPorModalidade = useSelector(
    state =>
      state.dashboardEscolaAqui
        .dadosDeLeituraDeComunicadosAgrupadosPorModalidade
  );

  const [dadosLegendaGrafico, setDadosLegendaGrafico] = useState([]);

  useEffect(() => {
    dispatch(setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]));
    return () => {
      dispatch(setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]));
    };
  }, [dispatch]);

  const obterDadosDeLeituraDeComunicadosAgrupadosPorModalidade = useCallback(
    async dadosComunicado => {
      if (dadosComunicado?.id) {
        setExibirLoader(true);
        const resposta = await ServicoDashboardEscolaAqui.obterDadosDeLeituraDeComunicadosPorModalidades(
          dadosComunicado.codigoDre || '',
          dadosComunicado.codigoUe || '',
          dadosComunicado.id,
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
            dispatch(
              setDadosDeLeituraDeComunicadosAgrupadosPorModalidade(
                retornoDados.dadosComunicadosGraficoBarras
              )
            );
          }
        } else {
          dispatch(setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]));
        }
      }
    },
    [modoVisualizacao, chavesGrafico, dispatch]
  );

  useEffect(() => {
    dispatch(setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]));
    if (comunicado && listaComunicado?.length) {
      const dadosComunicado = obterDadosComunicadoSelecionado(
        comunicado,
        listaComunicado
      );
      if (
        dadosComunicado?.id &&
        dadosComunicado?.codigoDre &&
        dadosComunicado?.codigoUe &&
        !dadosComunicado?.turmasCodigo?.length
      ) {
        obterDadosDeLeituraDeComunicadosAgrupadosPorModalidade(dadosComunicado);
      }
    }
  }, [comunicado, modoVisualizacao, listaComunicado]);

  return dadosDeLeituraDeComunicadosAgrupadosPorModalidade?.length ? (
    <div className="col-md-12">
      <Loader
        loading={exibirLoader}
        tip="Carregando Total de leituras por Modalidade"
      >
        {dadosDeLeituraDeComunicadosAgrupadosPorModalidade?.length ? (
          <GraficoBarraDashboard
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
  ) : (
    ''
  );
};

LeituraDeComunicadosPorModalidades.propTypes = {
  modoVisualizacao: PropTypes.oneOfType([PropTypes.string]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  comunicado: PropTypes.oneOfType([PropTypes.string]),
  listaComunicado: PropTypes.oneOfType([PropTypes.array]),
};

LeituraDeComunicadosPorModalidades.defaultProps = {
  modoVisualizacao: '',
  chavesGrafico: [],
  comunicado: '',
  listaComunicado: [],
};

export default LeituraDeComunicadosPorModalidades;
