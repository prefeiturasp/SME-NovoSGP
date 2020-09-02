import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import styled from 'styled-components';
import CardLink from '../../componentes/cardlink';
import Grid from '../../componentes/grid';
import Row from '../../componentes/row';
import LaderCardsDashboard from './laderCardsDashboard';

export const Dashboard = styled.div`
  .alinhar-itens-topo {
    align-items: initial !important;
  }
  .card {
    height: 100% !important;
  }
`;

const CardsDashboard = () => {
  const usuario = useSelector(state => state.usuario);

  const dadosCardsDashboard = useSelector(
    store => store.dashboard.dadosCardsDashboard
  );

  const temTurma = !!usuario.turmaSelecionada.turma;

  return (
    <LaderCardsDashboard>
      <Dashboard>
        <Row>
          <Grid cols={12} className="form-inline alinhar-itens-topo">
            {dadosCardsDashboard && dadosCardsDashboard.length
              ? dadosCardsDashboard.map(item => {
                  return (
                    <CardLink
                      key={shortid.generate()}
                      cols={[4, 4, 4, 12]}
                      iconSize="90px"
                      url={item.rota}
                      disabled={
                        !item.usuarioTemPermissao ||
                        (item.turmaObrigatoria && !temTurma)
                      }
                      icone={item.icone}
                      label={item.descricao}
                      minHeight="177px"
                    />
                  );
                })
              : ''}
          </Grid>
        </Row>
      </Dashboard>
    </LaderCardsDashboard>
  );
};

export default CardsDashboard;
