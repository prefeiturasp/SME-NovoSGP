import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Graficos, Loader, SelectComponent } from '~/componentes';
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
    const dadosMapeados = [
      {
        label: 'Expirados',
        Expirados: dados.totalExpirados,
      },
      {
        label: 'Vigentes',
        Vigentes: dados.totalVigentes,
      },
    ];
    setDadosGraficoTotalComunicadosEnviados(dadosMapeados);
  };

  const obterDadosGraficoTotalComunicadosEnviados = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardEscolaAqui.obterDadosGraficoTotalComunicadosEnviados(
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
    const dadosMapeados = dados.map(item => {
      return {
        nomeDre: item.nomeDre,
        comunicadosvigentes: item.comunicadosvigentes,
        comunicadosExpirados: item.comunicadosExpirados,
        Vigentes: item.comunicadosvigentes,
        Expirados: item.comunicadosExpirados,
      };
    });
    setDadosTotalComunicadosPorDRE(dadosMapeados);
  }, []);

  const obterDadosGraficoTotalComunicadosPorDRE = useCallback(async () => {
    if (codigoDre === '-99' && codigoUe === '-99') {
      setExibirLoader(true);
      const retorno = await ServicoDashboardEscolaAqui.obterDadosGraficoTotalComunicadosPorDRE(
        codigoDre === '-99' ? '' : codigoDre,
        codigoUe === '-99' ? '' : codigoUe,
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
      obterDadosGraficoTotalComunicadosEnviados();
      obterDadosGraficoTotalComunicadosPorDRE();
    } else {
      limparDadosGraficos();
    }
  }, [
    anoLetivo,
    codigoDre,
    codigoUe,
    obterDadosGraficoTotalComunicadosEnviados,
    obterDadosGraficoTotalComunicadosPorDRE,
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
              indice="nomeDre"
              chaves={chavesGraficos}
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
        {dadosTotalComunicadosPorDRE?.length &&
        dadosTotalComunicadosPorDRE?.length
          ? ''
          : 'Sem dados'}
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
