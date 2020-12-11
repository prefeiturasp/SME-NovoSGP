import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { CoresGraficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';
import {
  mapearParaDtoDadosComunicadosGraficoBarras,
  obterDadosComunicadoSelecionado,
} from '../../dashboardEscolaAquiGraficosUtils';
import GraficoBarraDashboardEscolaAqui from '../ComponentesDashboardEscolaAqui/graficoBarraDashboardEscolaAqui';

const LeituraDeComunicadosPorModalidades = props => {
  const {
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
            setDadosDeLeituraDeComunicadosAgrupadosPorModalidade(
              retornoDados.dadosComunicadosGraficoBarras
            );
          }
        } else {
          setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]);
        }
      }
    },
    [modoVisualizacao, chavesGrafico]
  );

  useEffect(() => {
    if (comunicado && listaComunicado?.length) {
      const dadosComunicado = obterDadosComunicadoSelecionado(
        comunicado,
        listaComunicado
      );
      if (
        dadosComunicado?.id &&
        dadosComunicado.codigoDre &&
        dadosComunicado.codigoUe
      ) {
        obterDadosDeLeituraDeComunicadosAgrupadosPorModalidade(dadosComunicado);
      }
    } else {
      setDadosDeLeituraDeComunicadosAgrupadosPorModalidade([]);
    }
  }, [comunicado, listaComunicado]);

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
