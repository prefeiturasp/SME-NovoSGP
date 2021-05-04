import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Loader, SelectComponent } from '~/componentes';
import GraficoBarras from '~/componentes-sgp/Graficos/graficoBarras';
import { erros } from '~/servicos';
import ServicoDashboardFrequencia from '~/servicos/Paginas/Dashboard/ServicoDashboardFrequencia';

const GraficoFrequenciaGlobalPorDRE = props => {
  const {
    anoLetivo,
    dreId,
    ueId,
    modalidade,
    semestre,
    listaAnosEscolares,
  } = props;

  const [dadosGrafico, setDadosGrafico] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);
  const [anoEscolarSelecionado, setAnoEscolarSelecionado] = useState();

  const OPCAO_TODOS = '-99';

  const obterDadosGrafico = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardFrequencia.obterFrequenciaGlobalPorDRE(
      anoLetivo,
      dreId === OPCAO_TODOS ? '' : dreId,
      ueId === OPCAO_TODOS ? '' : ueId,
      modalidade,
      semestre,
      anoEscolarSelecionado
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno?.data?.length) {
      setDadosGrafico(retorno.data);
    } else {
      setDadosGrafico([]);
    }
  }, [anoEscolarSelecionado, anoLetivo, dreId, ueId, modalidade, semestre]);

  useEffect(() => {
    if (anoLetivo && dreId && ueId) {
      obterDadosGrafico();
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, dreId, ueId, obterDadosGrafico]);

  useEffect(() => {
    if (listaAnosEscolares?.length) {
      if (listaAnosEscolares?.length === 1) {
        setAnoEscolarSelecionado(listaAnosEscolares[0].valor);
      } else {
        const temTodos = listaAnosEscolares.find(
          item => item.valor === OPCAO_TODOS
        );
        if (temTodos) {
          setAnoEscolarSelecionado(OPCAO_TODOS);
        }
      }
    }
  }, [listaAnosEscolares]);

  const onChangeAnoEscolar = valor => {
    setAnoEscolarSelecionado(valor);
  };

  return (
    <>
      <div className="row">
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
          <SelectComponent
            lista={listaAnosEscolares}
            valueOption="valor"
            valueText="descricao"
            disabled={listaAnosEscolares?.length === 1}
            valueSelect={anoEscolarSelecionado}
            onChange={onChangeAnoEscolar}
            placeholder="Selecione o ano"
          />
        </div>
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
          'Sem dados'
        ) : (
          ''
        )}
      </Loader>
    </>
  );
};

GraficoFrequenciaGlobalPorDRE.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  modalidade: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  semestre: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  listaAnosEscolares: PropTypes.oneOfType(PropTypes.array),
};

GraficoFrequenciaGlobalPorDRE.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  modalidade: null,
  semestre: null,
  listaAnosEscolares: [],
};

export default GraficoFrequenciaGlobalPorDRE;
