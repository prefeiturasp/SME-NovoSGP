import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { CoresGraficos, Loader, SelectComponent } from '~/componentes';
import { AbrangenciaServico, erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import GraficoBarraDashboard from '../../../ComponentesDashboard/graficoBarraDashboard';

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

  const corVientes = CoresGraficos[0];
  const corExpirados = CoresGraficos[1];

  const mapearParaDtoGraficoTotalComunicadosEnviados = dados => {
    if (dados.totalComunicadosVigentes || dados.totalComunicadosExpirados) {
      const dadosMapeados = [
        {
          label: 'Expirados',
          Expirados: dados.totalComunicadosExpirados,
          color: corExpirados,
        },
        {
          label: 'Vigentes',
          Vigentes: dados.totalComunicadosVigentes,
          color: corVientes,
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
            novo.color = corVientes;
          }
          if (item.totalComunicadosExpirados) {
            novo.totalComunicadosExpirados = item.totalComunicadosExpirados;
            novo.Expirados = item.totalComunicadosExpirados;
            novo.color = corExpirados;
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

  const graficoTotalComunicadosEnviados = () => {
    return dadosGraficoTotalComunicadosEnviados?.length ? (
      <GraficoBarraDashboard
        titulo="Total de Comunicados Enviados"
        dadosGrafico={dadosGraficoTotalComunicadosEnviados}
        chavesGrafico={chavesGraficos}
        indice="label"
        groupMode="stacked"
        removeLegends
      />
    ) : (
      ''
    );
  };

  const graficoTotalComunicadosPorDRE = () => {
    return dadosTotalComunicadosPorDRE?.length ? (
      <GraficoBarraDashboard
        titulo="Total de Comunicados por DRE"
        dadosGrafico={dadosTotalComunicadosPorDRE}
        chavesGrafico={chavesGraficos}
        indice="nomeAbreviadoDre"
        customPropsColors={item => {
          if (item.id === 'Vigentes') {
            return corVientes;
          }
          return corExpirados;
        }}
      />
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
