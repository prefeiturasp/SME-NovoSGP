import React, { useCallback, useEffect, useState } from 'react';
import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { Loader } from '~/componentes';
import ListaAlunos from './ListaAlunos/listaAlunos';

const { TabPane } = Tabs;

const ListaBimestres = props => {
  const { bimestres, componenteCurricularIdSelecionado } = props;

  const [bimestreCorrente, setBimestreCorrente] = useState(1);
  const [carregandoListaBimestres, setCarregandoListaBimestres] = useState(
    false
  );

  const obterDadosBimestres = useCallback(
    async numeroBimestre => {
      if (componenteCurricularIdSelecionado > 0) {
        setCarregandoListaBimestres(true);
        setBimestreCorrente(numeroBimestre);

        setCarregandoListaBimestres(false);
      }
    },
    [componenteCurricularIdSelecionado]
  );

  useEffect(() => {
    obterDadosBimestres(1);
  }, [componenteCurricularIdSelecionado, obterDadosBimestres]);

  const onChangeTab = async numeroBimestre => {
    if (componenteCurricularIdSelecionado) {
      await obterDadosBimestres(numeroBimestre);
    }
  };

  return (
    <>
      <Loader loading={carregandoListaBimestres} />
      <ContainerTabsCard
        type="card"
        onChange={onChangeTab}
        activeKey={String(bimestreCorrente)}
      >
        {bimestres.map(bimestre => {
          return (
            <TabPane tab={bimestre.descricao} key={bimestre.id}>
              <ListaAlunos
                bimestreSelecionado={bimestre.id}
                componenteCurricularId={componenteCurricularIdSelecionado}
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
