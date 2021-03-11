import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { setBimestreSelecionado } from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { setExpandirLinhaAusenciaEstudante } from '~/redux/modulos/listaFrequenciaPorBimestre/actions';
import ListaAlunos from './ListaAlunos/listaAlunos';

const { TabPane } = Tabs;

const ListaBimestres = props => {
  const dispatch = useDispatch();

  const { bimestres, componenteCurricularIdSelecionado } = props;

  const { bimestreSelecionado } = useSelector(
    store => store.acompanhamentoFrequencia
  );

  const onChangeTab = numeroBimestre => {
    if (componenteCurricularIdSelecionado) {
      dispatch(setExpandirLinhaAusenciaEstudante([]));
      dispatch(setBimestreSelecionado(numeroBimestre));
    }
  };

  return (
    <>
      <ContainerTabsCard
        type="card"
        width="20%"
        onChange={onChangeTab}
        activeKey={String(bimestreSelecionado)}
      >
        {bimestres.map(bimestre => {
          return (
            <TabPane tab={bimestre.descricao} key={bimestre.id}>
              <ListaAlunos
                componenteCurricularId={componenteCurricularIdSelecionado}
                bimestreLista={bimestre.id}
              />
            </TabPane>
          );
        })}
      </ContainerTabsCard>
    </>
  );
};

ListaBimestres.propTypes = {
  componenteCurricularIdSelecionado: PropTypes.string,
  bimestres: PropTypes.oneOfType([PropTypes.array]),
};

ListaBimestres.defaultProps = {
  componenteCurricularIdSelecionado: PropTypes.string,
  bimestres: PropTypes.oneOfType([PropTypes.array]),
};

export default ListaBimestres;
