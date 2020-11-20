import { Tabs } from 'antd';
import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { setTabAtualComponenteCurricular } from '~/redux/modulos/frequenciaPlanoAula/actions';
import ServicoPlanoAula from '~/servicos/Paginas/DiarioClasse/ServicoPlanoAula';
import ListaObjetivosPlanoAula from '../ListaObjetivos/listaObjetivosPlanoAula';
import {
  ContainerTabsComponentesCorriculares,
  DescricaoNomeTabComponenteCurricular,
} from './tabsComponentesCorriculares.css';

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
      <div className="col-md-12 mb-2">
        {componenteCurricular.possuiObjetivos ? (
          <ListaObjetivosPlanoAula
            tabAtualComponenteCurricular={tabAtualComponenteCurricular}
          />
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

      dispatch(setTabAtualComponenteCurricular(componente));
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

  const obterDescricaoNomeTabComponenteCurricular = (
    nome,
    codigoComponenteCurricular
  ) => {
    const temObjetivosSelecionados = ServicoPlanoAula.temObjetivosSelecionadosTabComponenteCurricular(
      codigoComponenteCurricular
    );

    if (temObjetivosSelecionados) {
      return (
        <DescricaoNomeTabComponenteCurricular
          title={nome}
          tabSelecionada={
            String(tabAtualComponenteCurricular?.codigoComponenteCurricular) ===
            String(codigoComponenteCurricular)
          }
        >
          <span className="desc-nome">{nome}</span>
          <i className="fas fa-check-circle ml-2" />
        </DescricaoNomeTabComponenteCurricular>
      );
    }
    return <span title={nome}>{nome}</span>;
  };

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
                  tab={obterDescricaoNomeTabComponenteCurricular(
                    item.nome,
                    item.codigoComponenteCurricular
                  )}
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
