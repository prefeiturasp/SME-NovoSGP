import { Tabs } from 'antd';
import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { setTabAtualComponenteCurricular } from '~/redux/modulos/frequenciaPlanoAula/actions';
import { ContainerTabsComponentesCorriculares } from './tabsComponentesCorriculares.css';

const { TabPane } = Tabs;

const TabsComponentesCorriculares = () => {
  const dispatch = useDispatch();
  const listaComponentesCurricularesPlanejamento = useSelector(
    store => store.frequenciaPlanoAula.listaComponentesCurricularesPlanejamento
  );

  const tabAtualComponenteCurricular = useSelector(
    store => store.frequenciaPlanoAula.tabAtualComponenteCurricular
  );

  const componenteCurricular = useSelector(
    store => store.frequenciaPlanoAula.componenteCurricular
  );

  const montarDados = () => {
    return (
      <div className="col-md-12">
        {componenteCurricular.possuiObjetivos ? (
          // <ListaObjetivos
          //   dadosBimestre={dadosBimestre}
          //   tabAtualComponenteCurricular={tabAtualComponenteCurricular}
          // />
          <div>opa </div>
        ) : (
          ''
        )}
      </div>
    );
  };

  const onChangeTab = useCallback(
    codigoComponente => {
      const componente = listaComponentesCurricularesPlanejamento.find(
        item =>
          String(item.codigoComponenteCurricular) === String(codigoComponente)
      );

      // NO REDUX REFATORAR PARA NAO CONSIDERAR O BIMESTRE!
      dispatch(setTabAtualComponenteCurricular(componente));

      // TODO AQUI CARREGAR OS DADOS JA SALVOS DO PLANO AULA!
    },
    [dispatch, listaComponentesCurricularesPlanejamento]
  );

  // Quando tiver somente uma tab(componente curricular) já selecionar!
  useEffect(() => {
    if (listaComponentesCurricularesPlanejamento.length === 1) {
      onChangeTab(
        listaComponentesCurricularesPlanejamento[0].codigoComponenteCurricular
      );
    }
  }, [onChangeTab, listaComponentesCurricularesPlanejamento]);

  return (
    <>
      {listaComponentesCurricularesPlanejamento &&
      listaComponentesCurricularesPlanejamento.length ? (
        <ContainerTabsComponentesCorriculares
          widthAntTabsNav={
            listaComponentesCurricularesPlanejamento.length > 4
              ? `${100 / listaComponentesCurricularesPlanejamento.length}%`
              : '25%'
          }
        >
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
                  tab={<span title={item.nome}>{item.nome}</span>}
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
        </ContainerTabsComponentesCorriculares>
      ) : (
        ''
      )}
    </>
  );
};

export default TabsComponentesCorriculares;
