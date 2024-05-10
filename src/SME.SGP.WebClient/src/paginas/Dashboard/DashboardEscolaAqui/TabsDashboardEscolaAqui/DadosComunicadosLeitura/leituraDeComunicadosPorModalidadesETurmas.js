import _ from 'lodash';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { CoresGraficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import {
  mapearParaDtoDadosComunicadosGraficoBarras,
  obterDadosComunicadoSelecionado,
} from '../../../ComponentesDashboard/graficosDashboardUtils';
import GraficoBarraDashboard from '../../../ComponentesDashboard/graficoBarraDashboard';

const LeituraDeComunicadosPorModalidadesETurmas = props => {
  const {
    chavesGrafico,
    modoVisualizacao,
    comunicado,
    listaComunicado,
  } = props;

  const [exibirLoader, setExibirLoader] = useState(false);

  const [
    dadosDeLeituraDeComunicadosPorModalidadesETurmas,
    setDadosDeLeituraDeComunicadosPorModalidadesETurmas,
  ] = useState([]);

  const dadosDeLeituraDeComunicadosAgrupadosPorModalidade = useSelector(
    state =>
      state.dashboardEscolaAqui
        .dadosDeLeituraDeComunicadosAgrupadosPorModalidade
  );

  const obterDadosDeLeituraDeComunicadosPorModalidadesETurmas = useCallback(
    async dadosComunicado => {
      const modalidadesCodigos = dadosDeLeituraDeComunicadosAgrupadosPorModalidade.map(
        item => item.modalidadeCodigo
      );
      if (modalidadesCodigos?.length && dadosComunicado?.id) {
        setExibirLoader(true);
        const resposta = await ServicoDashboardEscolaAqui.obterDadosDeLeituraDeComunicadosPorModalidadeETurmas(
          dadosComunicado.codigoDre,
          dadosComunicado.codigoUe,
          dadosComunicado.id,
          modoVisualizacao,
          modalidadesCodigos
        )
          .catch(e => erros(e))
          .finally(() => setExibirLoader(false));

        if (resposta?.data?.length) {
          const dadosAgrupados = _(resposta.data)
            .groupBy('modalidadeCodigo')
            .values()
            .value();

          const dados = dadosAgrupados.map(item => {
            const retornoDados = mapearParaDtoDadosComunicadosGraficoBarras(
              item,
              'turma',
              chavesGrafico
            );
            return retornoDados;
          });
          setDadosDeLeituraDeComunicadosPorModalidadesETurmas(dados);
        } else {
          setDadosDeLeituraDeComunicadosPorModalidadesETurmas([]);
        }
      }
    },
    [
      modoVisualizacao,
      chavesGrafico,
      dadosDeLeituraDeComunicadosAgrupadosPorModalidade,
    ]
  );

  useEffect(() => {
    return () => {
      setDadosDeLeituraDeComunicadosPorModalidadesETurmas([]);
    };
  }, []);

  useEffect(() => {
    setDadosDeLeituraDeComunicadosPorModalidadesETurmas([]);
    if (
      dadosDeLeituraDeComunicadosAgrupadosPorModalidade?.length &&
      comunicado &&
      listaComunicado?.length
    ) {
      const dadosComunicado = obterDadosComunicadoSelecionado(
        comunicado,
        listaComunicado
      );
      if (
        dadosComunicado?.id &&
        dadosComunicado?.codigoDre &&
        dadosComunicado?.codigoUe &&
        !dadosComunicado?.turmasCodigo.length
      ) {
        obterDadosDeLeituraDeComunicadosPorModalidadesETurmas(dadosComunicado);
      }
    }
  }, [
    modoVisualizacao,
    comunicado,
    listaComunicado,
    dadosDeLeituraDeComunicadosAgrupadosPorModalidade,
  ]);

  return dadosDeLeituraDeComunicadosAgrupadosPorModalidade?.length &&
    dadosDeLeituraDeComunicadosPorModalidadesETurmas?.length ? (
    <>
      <div className="col-md-12">
        <Loader
          loading={exibirLoader}
          tip="Carregando Total de leituras por Turma"
        >
          {dadosDeLeituraDeComunicadosPorModalidadesETurmas?.length
            ? dadosDeLeituraDeComunicadosPorModalidadesETurmas.map(
                dadosGrafico => {
                  return (
                    <GraficoBarraDashboard
                      titulo={`Total de leituras por Turma ${dadosGrafico.dadosComunicadosGraficoBarras[0].modalidade}`}
                      dadosGrafico={dadosGrafico.dadosComunicadosGraficoBarras}
                      chavesGrafico={chavesGrafico}
                      indice="turma"
                      dadosLegendaCustomizada={dadosGrafico.dadosLegendaGrafico}
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
                  );
                }
              )
            : ''}
        </Loader>
      </div>
    </>
  ) : (
    ''
  );
};

LeituraDeComunicadosPorModalidadesETurmas.propTypes = {
  modoVisualizacao: PropTypes.oneOfType([PropTypes.string]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  comunicado: PropTypes.oneOfType([PropTypes.string]),
  listaComunicado: PropTypes.oneOfType([PropTypes.array]),
};

LeituraDeComunicadosPorModalidadesETurmas.defaultProps = {
  modoVisualizacao: '',
  chavesGrafico: [],
  comunicado: '',
  listaComunicado: [],
};

export default LeituraDeComunicadosPorModalidadesETurmas;
