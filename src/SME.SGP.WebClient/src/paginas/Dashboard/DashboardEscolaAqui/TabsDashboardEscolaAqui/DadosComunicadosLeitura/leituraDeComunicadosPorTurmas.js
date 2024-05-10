import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { CoresGraficos, Loader } from '~/componentes';
import { setDadosDeLeituraDeComunicadosPorTurmas } from '~/redux/modulos/dashboardEscolaAqui/actions';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import {
  mapearParaDtoDadosComunicadosGraficoBarras,
  obterDadosComunicadoSelecionado,
} from '../../../ComponentesDashboard/graficosDashboardUtils';
import GraficoBarraDashboard from '../../../ComponentesDashboard/graficoBarraDashboard';

const LeituraDeComunicadosPorTurmas = props => {
  const {
    chavesGrafico,
    modoVisualizacao,
    comunicado,
    listaComunicado,
  } = props;

  const [exibirLoader, setExibirLoader] = useState(false);

  const dispatch = useDispatch();

  const dadosDeLeituraDeComunicadosPorTurmas = useSelector(
    state => state.dashboardEscolaAqui.dadosDeLeituraDeComunicadosPorTurmas
  );

  const [dadosLegendaGrafico, setDadosLegendaGrafico] = useState([]);

  useEffect(() => {
    dispatch(setDadosDeLeituraDeComunicadosPorTurmas([]));
    return () => {
      dispatch(setDadosDeLeituraDeComunicadosPorTurmas([]));
    };
  }, [dispatch]);

  const obterDadosDeLeituraDeComunicadosPorTurmas = useCallback(
    async (dadosComunicado, turmasCodigo) => {
      setExibirLoader(true);
      const resposta = await ServicoDashboardEscolaAqui.obterDadosDeLeituraDeComunicadosPorModalidadeETurmas(
        dadosComunicado.codigoDre,
        dadosComunicado.codigoUe,
        dadosComunicado.id,
        modoVisualizacao,
        '',
        turmasCodigo
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data?.length) {
        const retornoDados = mapearParaDtoDadosComunicadosGraficoBarras(
          resposta.data,
          'turma',
          chavesGrafico
        );
        if (retornoDados?.dadosLegendaGrafico?.length) {
          setDadosLegendaGrafico(retornoDados.dadosLegendaGrafico);
        }
        if (retornoDados?.dadosComunicadosGraficoBarras?.length) {
          dispatch(
            setDadosDeLeituraDeComunicadosPorTurmas(
              retornoDados.dadosComunicadosGraficoBarras
            )
          );
        }
      } else {
        dispatch(setDadosDeLeituraDeComunicadosPorTurmas([]));
      }
    },
    [modoVisualizacao, chavesGrafico, dispatch]
  );

  useEffect(() => {
    dispatch(setDadosDeLeituraDeComunicadosPorTurmas([]));
    if (comunicado && listaComunicado?.length) {
      const dadosComunicado = obterDadosComunicadoSelecionado(
        comunicado,
        listaComunicado
      );

      const turmasCodigo = dadosComunicado?.turmasCodigo?.length
        ? dadosComunicado.turmasCodigo.map(item => item.codigoTurma)
        : '';

      if (
        dadosComunicado?.id &&
        dadosComunicado?.codigoDre &&
        dadosComunicado?.codigoUe &&
        turmasCodigo?.length
      ) {
        obterDadosDeLeituraDeComunicadosPorTurmas(
          dadosComunicado,
          turmasCodigo
        );
      }
    }
  }, [comunicado, modoVisualizacao, listaComunicado, dispatch]);

  return dadosDeLeituraDeComunicadosPorTurmas.length ? (
    <div className="col-md-12">
      <Loader
        loading={exibirLoader}
        tip="Carregando Total de leituras por Turma"
      >
        <GraficoBarraDashboard
          titulo="Total de leituras por Turma"
          dadosGrafico={dadosDeLeituraDeComunicadosPorTurmas}
          chavesGrafico={chavesGrafico}
          indice="turma"
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
      </Loader>
    </div>
  ) : (
    ''
  );
};

LeituraDeComunicadosPorTurmas.propTypes = {
  modoVisualizacao: PropTypes.oneOfType([PropTypes.string]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  comunicado: PropTypes.oneOfType([PropTypes.string]),
  listaComunicado: PropTypes.oneOfType([PropTypes.array]),
};

LeituraDeComunicadosPorTurmas.defaultProps = {
  modoVisualizacao: '',
  chavesGrafico: [],
  comunicado: '',
  listaComunicado: [],
};

export default LeituraDeComunicadosPorTurmas;
