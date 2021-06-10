import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Loader, SelectComponent } from '~/componentes';
import DataUltimaAtualizacao from '~/componentes-sgp/DataUltimaAtualizacao/dataUltimaAtualizacao';
import GraficoBarras from '~/componentes-sgp/Graficos/graficoBarras';
import { erros } from '~/servicos';
import ServicoDashboardFrequencia from '~/servicos/Paginas/Dashboard/ServicoDashboardFrequencia';

const GraficoFrequenciaGlobalPorDRE = props => {
  const { anoLetivo, modalidade, semestre } = props;

  const dataUltimaConsolidacao = useSelector(
    store =>
      store.dashboardFrequencia?.dadosDashboardFrequencia
        ?.dataUltimaConsolidacao
  );

  const listaAnosEscolares = useSelector(
    store =>
      store.dashboardFrequencia?.dadosDashboardFrequencia?.listaAnosEscolares
  );

  const [dadosGrafico, setDadosGrafico] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);
  const [anoEscolar, setAnoEscolar] = useState();

  const OPCAO_TODOS = '-99';

  const obterDadosGrafico = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardFrequencia.obterFrequenciaGlobalPorDRE(
      anoLetivo,
      modalidade,
      anoEscolar === OPCAO_TODOS ? '' : anoEscolar,
      semestre
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno?.data?.length) {
      setDadosGrafico(retorno.data);
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, modalidade, anoEscolar, semestre]);

  useEffect(() => {
    if (anoLetivo && modalidade && anoEscolar && modalidade) {
      obterDadosGrafico();
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, modalidade, anoEscolar, semestre, obterDadosGrafico]);

  useEffect(() => {
    if (listaAnosEscolares?.length === 1) {
      setAnoEscolar(listaAnosEscolares[0].ano);
    }
    if (listaAnosEscolares?.length > 1) {
      setAnoEscolar(OPCAO_TODOS);
    }
  }, [listaAnosEscolares]);

  const onChangeAnoEscolar = valor => {
    setAnoEscolar(valor);
  };

  return (
    <>
      <div className="row">
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
          <SelectComponent
            lista={listaAnosEscolares || []}
            valueOption="ano"
            valueText="modalidadeAno"
            disabled={listaAnosEscolares?.length === 1}
            valueSelect={anoEscolar}
            onChange={onChangeAnoEscolar}
            placeholder="Selecione o ano"
            allowClear={false}
          />
        </div>
        {dataUltimaConsolidacao && (
          <div className="col-sm-12 col-md-6 col-lg-9 col-xl-9 mb-2">
            <DataUltimaAtualizacao
              dataFormatada={
                dataUltimaConsolidacao
                  ? moment(dataUltimaConsolidacao).format('DD/MM/YYYY HH:mm:ss')
                  : ''
              }
            />
          </div>
        )}
      </div>
      <Loader
        loading={exibirLoader}
        className={exibirLoader ? 'text-center' : ''}
      >
        {dadosGrafico?.length ? (
          <GraficoBarras
            data={dadosGrafico}
            xField="dre"
            xAxisVisible
            isGroup
            colors={['#0288D1', '#F57C00']}
          />
        ) : !exibirLoader ? (
          <div className="text-center">Sem dados</div>
        ) : (
          ''
        )}
      </Loader>
    </>
  );
};

GraficoFrequenciaGlobalPorDRE.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  modalidade: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  semestre: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
};

GraficoFrequenciaGlobalPorDRE.defaultProps = {
  anoLetivo: null,
  modalidade: null,
  semestre: null,
};

export default GraficoFrequenciaGlobalPorDRE;
