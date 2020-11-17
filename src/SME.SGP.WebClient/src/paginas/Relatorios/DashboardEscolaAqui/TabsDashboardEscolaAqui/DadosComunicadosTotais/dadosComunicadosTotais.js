import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import {
  CoresGraficos,
  Graficos,
  Loader,
  SelectComponent,
} from '~/componentes';
import { AbrangenciaServico, erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';
import {
  ContainerGraficoBarras,
  TituloGrafico,
} from '../../dashboardEscolaAqui.css';

const DadosComunicadosTotais = props => {
  const { codigoDre, codigoUe } = props;

  const [exibirLoader, setExibirLoader] = useState(false);
  const [
    dadosGraficoTotalComunicadosEnviados,
    setDadosGraficoTotalComunicadosEnviados,
  ] = useState([]);
  const [
    dadosTotalComunicadosPorDRE,
    setDadosTotalComunicadosPorDRE,
  ] = useState([]);

  const chavesGraficos = ['Vigentes', 'Expirados'];

  const [listaAnosLetivo, setListaAnosLetivo] = useState([]);
  const [anoLetivo, setAnoLetivo] = useState();

  const limparDadosGraficos = () => {
    setDadosGraficoTotalComunicadosEnviados([]);
    setDadosTotalComunicadosPorDRE([]);
  };

  const mapearParaDtoGraficoTotalComunicadosEnviados = dados => {
    if (dados.totalComunicadosVigentes || dados.totalComunicadosExpirados) {
      const dadosMapeados = [
        {
          label: 'Expirados',
          Expirados: dados.totalComunicadosExpirados,
        },
        {
          label: 'Vigentes',
          Vigentes: dados.totalComunicadosVigentes,
        },
      ];
      setDadosGraficoTotalComunicadosEnviados(dadosMapeados);
    } else {
      setDadosGraficoTotalComunicadosEnviados([]);
    }
  };

  const obterComunicadosTotaisSme = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardEscolaAqui.obterComunicadosTotaisSme(
      codigoDre === '-99' ? '' : codigoDre,
      codigoUe === '-99' ? '' : codigoUe,
      anoLetivo
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno && retorno.data) {
      mapearParaDtoGraficoTotalComunicadosEnviados(retorno.data);
    } else {
      setDadosGraficoTotalComunicadosEnviados([]);
    }
  }, [codigoDre, codigoUe, anoLetivo]);

  const mapearParaDtoGraficoTotalComunicadosPorDRE = useCallback(dados => {
    const temDados = dados.filter(
      item => item.totalComunicadosVigentes || item.totalComunicadosExpirados
    );
    if (temDados?.length) {
      const dadosMapeados = dados.map(item => {
        const novo = {};
        if (item.totalComunicadosVigentes || item.totalComunicadosExpirados) {
          novo.nomeAbreviadoDre = item.nomeAbreviadoDre;
          if (item.totalComunicadosVigentes) {
            novo.totalComunicadosVigentes = item.totalComunicadosVigentes;
            novo.Vigentes = item.totalComunicadosVigentes;
          }
          if (item.totalComunicadosExpirados) {
            novo.totalComunicadosExpirados = item.totalComunicadosExpirados;
            novo.Expirados = item.totalComunicadosExpirados;
          }
        }
        return novo;
      });
      setDadosTotalComunicadosPorDRE(
        dadosMapeados.filter(item => item.nomeAbreviadoDre)
      );
    } else {
      setDadosTotalComunicadosPorDRE([]);
    }
  }, []);

  const obterComunicadosTotaisAgrupadosPorDre = useCallback(async () => {
    if (codigoDre === '-99' && codigoUe === '-99' && anoLetivo) {
      setExibirLoader(true);
      const retorno = await ServicoDashboardEscolaAqui.obterComunicadosTotaisAgrupadosPorDre(
        anoLetivo
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (retorno && retorno.data && retorno.data.length) {
        mapearParaDtoGraficoTotalComunicadosPorDRE(retorno.data);
      } else {
        setDadosTotalComunicadosPorDRE([]);
      }
    } else {
      setDadosTotalComunicadosPorDRE([]);
    }
  }, [
    codigoDre,
    codigoUe,
    anoLetivo,
    mapearParaDtoGraficoTotalComunicadosPorDRE,
  ]);

  useEffect(() => {
    if (codigoDre && codigoUe && anoLetivo) {
      obterComunicadosTotaisSme();
      obterComunicadosTotaisAgrupadosPorDre();
    } else {
      limparDadosGraficos();
    }
  }, [
    anoLetivo,
    codigoDre,
    codigoUe,
    obterComunicadosTotaisSme,
    obterComunicadosTotaisAgrupadosPorDre,
  ]);

  const obterAnosLetivos = useCallback(async () => {
    setExibirLoader(true);
    const anosLetivo = await AbrangenciaServico.buscarTodosAnosLetivos()
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (anosLetivo?.data?.length) {
      const a = [];
      anosLetivo.data.forEach(ano => {
        a.push({ desc: ano, valor: ano });
      });
      setAnoLetivo(a[0].valor);
      setListaAnosLetivo(a);
    } else {
      setListaAnosLetivo([]);
    }
  }, []);

  useEffect(() => {
    obterAnosLetivos();
  }, [obterAnosLetivos]);

  const tooltipCustomizado = item => {
    return (
      <div style={{ whiteSpace: 'pre', display: 'flex', alignItems: 'center' }}>
        <span
          style={{
            display: 'block',
            width: '12px',
            height: '12px',
            background: item.color,
            marginRight: '7px',
          }}
        />
        {item.id} - <strong>{item.value}</strong>
      </div>
    );
  };

  const formataMilhar = valor => {
    return valor.toLocaleString('pt-BR');
  };

  const graficoTotalComunicadosEnviados = () => {
    return dadosGraficoTotalComunicadosEnviados?.length ? (
      <div className="scrolling-chart">
        <div className="col-md-12">
          <TituloGrafico>Total de Comunicados Enviados</TituloGrafico>
          <ContainerGraficoBarras>
            <Graficos.Barras
              dados={dadosGraficoTotalComunicadosEnviados}
              indice="label"
              chaves={chavesGraficos}
              groupMode="stacked"
              removeLegends
              customProps={{
                colors: item => {
                  if (item.id === 'Vigentes') {
                    return CoresGraficos[0];
                  }
                  return CoresGraficos[1];
                },
                tooltip: item => {
                  return tooltipCustomizado(item);
                },
                labelFormat: valor => {
                  return formataMilhar(valor);
                },
              }}
            />
          </ContainerGraficoBarras>
        </div>
      </div>
    ) : (
      ''
    );
  };

  const graficoTotalComunicadosPorDRE = () => {
    return dadosTotalComunicadosPorDRE?.length ? (
      <div className="scrolling-chart">
        <div className="col-md-12">
          <TituloGrafico>Total de Comunicados por DRE</TituloGrafico>
          <ContainerGraficoBarras>
            <Graficos.Barras
              dados={dadosTotalComunicadosPorDRE}
              indice="nomeAbreviadoDre"
              chaves={chavesGraficos}
              customProps={{
                colors: item => {
                  if (item.id === 'Vigentes') {
                    return CoresGraficos[0];
                  }
                  return CoresGraficos[1];
                },
                tooltip: item => {
                  return tooltipCustomizado(item);
                },
                labelFormat: valor => {
                  return formataMilhar(valor);
                },
              }}
            />
          </ContainerGraficoBarras>
        </div>
      </div>
    ) : (
      ''
    );
  };

  return (
    <Loader loading={exibirLoader}>
      <div className="col-sm-12 col-md-6 col-lg-3 col-xl-2 mb-2">
        <SelectComponent
          label="Ano Letivo"
          lista={listaAnosLetivo}
          valueOption="valor"
          valueText="desc"
          disabled={listaAnosLetivo?.length === 1}
          onChange={setAnoLetivo}
          valueSelect={anoLetivo}
          placeholder="Selecione o ano"
        />
      </div>

      {graficoTotalComunicadosEnviados()}
      {graficoTotalComunicadosPorDRE()}

      <div className="col-md-12 text-center">
        {!dadosGraficoTotalComunicadosEnviados?.length &&
        !dadosTotalComunicadosPorDRE?.length
          ? 'Sem dados'
          : ''}
      </div>
    </Loader>
  );
};

DadosComunicadosTotais.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
};

DadosComunicadosTotais.defaultProps = {
  codigoDre: '',
  codigoUe: '',
};

export default DadosComunicadosTotais;
