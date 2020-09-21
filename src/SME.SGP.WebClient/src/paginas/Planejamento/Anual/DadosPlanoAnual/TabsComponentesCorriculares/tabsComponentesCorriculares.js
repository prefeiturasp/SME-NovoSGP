import PropTypes from 'prop-types';
import { Tabs } from 'antd';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { setTabAtualComponenteCurricular } from '~/redux/modulos/anual/actions';
import ListaObjetivos from '../ListaObjetivos/listaObjetivos';

const { TabPane } = Tabs;

const TabsComponentesCorriculares = props => {
  const dispatch = useDispatch();

  const { dadosBimestre } = props;

  const listaComponentesCurricularesPlanejamento = useSelector(
    store => store.planoAnual.listaComponentesCurricularesPlanejamento
  );

  const tabAtualComponenteCurricular = useSelector(
    store =>
      store.planoAnual.tabAtualComponenteCurricular[dadosBimestre.bimestre]
  );

  const montarDados = () => {
    return <ListaObjetivos />;
  };

  const onChangeTab = codigoComponente => {
    const componente = listaComponentesCurricularesPlanejamento.find(
      item =>
        String(item.codigoComponenteCurricular) === String(codigoComponente)
    );

    dispatch(
      setTabAtualComponenteCurricular({
        bimestre: dadosBimestre.bimestre,
        componente,
      })
    );
  };

  return (
    <>
      {listaComponentesCurricularesPlanejamento &&
      listaComponentesCurricularesPlanejamento.length ? (
        <ContainerTabsCard
          type="card"
          onChange={onChangeTab}
          activeKey={String(
            tabAtualComponenteCurricular?.codigoComponenteCurricular
          )}
        >
          {listaComponentesCurricularesPlanejamento.map(item => {
            return (
              <TabPane
                tab={item.nome}
                key={String(item.codigoComponenteCurricular)}
              >
                {String(
                  tabAtualComponenteCurricular?.codigoComponenteCurricular
                ) === String(item.codigoComponenteCurricular)
                  ? montarDados()
                  : ''}
              </TabPane>
            );
          })}
        </ContainerTabsCard>
      ) : (
        ''
      )}
    </>
  );
};

TabsComponentesCorriculares.propTypes = {
  dadosBimestre: PropTypes.oneOfType([PropTypes.object]),
};

TabsComponentesCorriculares.defaultProps = {
  dadosBimestre: '',
};

export default TabsComponentesCorriculares;
