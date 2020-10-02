import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { setTabAtualComponenteCurricular } from '~/redux/modulos/anual/actions';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import DescricaoPlanejamento from '../DescricaoPlanejamento/descricaoPlanejamento';
import ListaObjetivos from '../ListaObjetivos/listaObjetivos';
import { ContainerTabsComponentesCorriculares } from './tabsComponentesCorriculares.css';

const { TabPane } = Tabs;

const TabsComponentesCorriculares = props => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { dadosBimestre } = props;

  const listaComponentesCurricularesPlanejamento = useSelector(
    store => store.planoAnual.listaComponentesCurricularesPlanejamento
  );

  const tabAtualComponenteCurricular = useSelector(
    store =>
      store.planoAnual.tabAtualComponenteCurricular[dadosBimestre.bimestre]
  );

  const componenteCurricular = useSelector(
    store => store.planoAnual.componenteCurricular
  );

  const clicouNoBimestre = useSelector(
    store => store.planoAnual.clicouNoBimestre[dadosBimestre.bimestre]
  );

  const montarDados = () => {
    return (
      <div className="col-md-12">
        {componenteCurricular.possuiObjetivos ? (
          <ListaObjetivos
            dadosBimestre={dadosBimestre}
            tabAtualComponenteCurricular={tabAtualComponenteCurricular}
          />
        ) : (
          ''
        )}
        <DescricaoPlanejamento
          dadosBimestre={dadosBimestre}
          tabAtualComponenteCurricular={tabAtualComponenteCurricular}
        />
      </div>
    );
  };

  const onChangeTab = useCallback(
    codigoComponente => {
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

      ServicoPlanoAnual.carregarDadosPlanoAnualPorComponenteCurricular(
        turmaSelecionada.id,
        codigoComponente,
        dadosBimestre.id,
        dadosBimestre.bimestre
      );
    },
    [
      dispatch,
      dadosBimestre,
      listaComponentesCurricularesPlanejamento,
      turmaSelecionada,
    ]
  );

  // Quando tiver somente uma tab(componente curricular) já selecionar!
  useEffect(() => {
    if (
      clicouNoBimestre &&
      listaComponentesCurricularesPlanejamento.length === 1
    ) {
      onChangeTab(
        listaComponentesCurricularesPlanejamento[0].codigoComponenteCurricular
      );
    }
  }, [onChangeTab, clicouNoBimestre, listaComponentesCurricularesPlanejamento]);

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

TabsComponentesCorriculares.propTypes = {
  dadosBimestre: PropTypes.oneOfType([PropTypes.object]),
};

TabsComponentesCorriculares.defaultProps = {
  dadosBimestre: '',
};

export default TabsComponentesCorriculares;
