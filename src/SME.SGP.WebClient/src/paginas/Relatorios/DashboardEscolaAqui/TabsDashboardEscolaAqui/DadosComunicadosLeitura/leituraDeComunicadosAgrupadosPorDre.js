import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { CoresGraficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';
import { adicionarCoresNosGraficos } from '../../dashboardEscolaAquiGraficosUtils';
import GraficoBarraDashboardEscolaAqui from '../ComponentesDashboardEscolaAqui/graficoBarraDashboardEscolaAqui';

const LeituraDeComunicadosAgrupadosPorDre = props => {
  const {
    codigoDre,
    codigoUe,
    chavesGrafico,
    modoVisualizacao,
    comunicado,
    listaComunicado,
    obterCominicadoId,
  } = props;

  const [exibirLoader, setExibirLoader] = useState(false);

  const [
    dadosDeLeituraDeComunicadosAgrupadosPorDre,
    setDadosDeLeituraDeComunicadosAgrupadosPorDre,
  ] = useState([]);

  const [dadosLegendaGrafico, setDadosLegendaGrafico] = useState([]);

  const OPCAO_TODOS = '-99';

  const mapearParaDtoGraficoComunicadosAgrupadosPorDre = useCallback(
    dados => {
      const temDados = dados.filter(
        item =>
          item.naoReceberamComunicado ||
          item.receberamENaoVisualizaram ||
          item.visualizaramComunicado
      );
      if (temDados?.length) {
        const dadosMapeados = dados.map(item => {
          const novo = {};
          if (
            item.naoReceberamComunicado ||
            item.receberamENaoVisualizaram ||
            item.visualizaramComunicado
          ) {
            novo.nomeAbreviadoDre = item.nomeAbreviadoDre;
            if (item.naoReceberamComunicado) {
              novo.naoReceberamComunicado = item.naoReceberamComunicado;
              novo[chavesGrafico[0]] = item.naoReceberamComunicado;
            }
            if (item.receberamENaoVisualizaram) {
              novo.receberamENaoVisualizaram = item.receberamENaoVisualizaram;
              novo[chavesGrafico[1]] = item.receberamENaoVisualizaram;
            }
            if (item.visualizaramComunicado) {
              novo.visualizaramComunicado = item.visualizaramComunicado;
              novo[chavesGrafico[2]] = item.visualizaramComunicado;
            }
          }
          return novo;
        });

        const dadosMapeadosComCores = adicionarCoresNosGraficos(
          dadosMapeados.filter(item => item.nomeAbreviadoDre)
        );

        const dadosParaMontarLegenda = [];
        if (dadosMapeadosComCores.find(item => !!item.naoReceberamComunicado)) {
          dadosParaMontarLegenda.push({
            label: chavesGrafico[0],
            color: CoresGraficos[0],
          });
        }
        if (
          dadosMapeadosComCores.find(item => !!item.receberamENaoVisualizaram)
        ) {
          dadosParaMontarLegenda.push({
            label: chavesGrafico[1],
            color: CoresGraficos[1],
          });
        }
        if (dadosMapeadosComCores.find(item => !!item.visualizaramComunicado)) {
          dadosParaMontarLegenda.push({
            label: chavesGrafico[2],
            color: CoresGraficos[2],
          });
        }

        setDadosLegendaGrafico(dadosParaMontarLegenda);
        setDadosDeLeituraDeComunicadosAgrupadosPorDre(dadosMapeadosComCores);
      } else {
        setDadosDeLeituraDeComunicadosAgrupadosPorDre([]);
      }
    },
    [chavesGrafico]
  );

  const obterDadosDeLeituraDeComunicadosAgrupadosPorDre = useCallback(async () => {
    const comunicadoId = obterCominicadoId(comunicado);

    if (comunicadoId) {
      setExibirLoader(true);
      const resposta = await ServicoDashboardEscolaAqui.obterDadosDeLeituraDeComunicadosAgrupadosPorDre(
        comunicadoId,
        modoVisualizacao
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (resposta?.data?.length) {
        mapearParaDtoGraficoComunicadosAgrupadosPorDre(resposta.data);
      } else {
        setDadosDeLeituraDeComunicadosAgrupadosPorDre([]);
      }
    }
  }, [
    modoVisualizacao,
    comunicado,
    obterCominicadoId,
    mapearParaDtoGraficoComunicadosAgrupadosPorDre,
  ]);

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
    <Loader loading={exibirLoader}>
      <div className="col-md-12">
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
      </div>
    </Loader>
  );
};

LeituraDeComunicadosAgrupadosPorDre.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
  modoVisualizacao: PropTypes.oneOfType([PropTypes.string]),
  chavesGrafico: PropTypes.oneOfType([PropTypes.array]),
  comunicado: PropTypes.oneOfType([PropTypes.string]),
  listaComunicado: PropTypes.oneOfType([PropTypes.array]),
  obterCominicadoId: PropTypes.oneOfType([PropTypes.func]),
};

LeituraDeComunicadosAgrupadosPorDre.defaultProps = {
  codigoDre: '',
  codigoUe: '',
  modoVisualizacao: '',
  chavesGrafico: [],
  comunicado: '',
  listaComunicado: [],
  obterCominicadoId: () => {},
};

export default LeituraDeComunicadosAgrupadosPorDre;
