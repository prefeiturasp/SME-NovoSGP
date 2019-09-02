import React, { useState } from 'react';
import Card from '../../componentes/card';
import Grid from '../../componentes/grid';
import CardLink from '../../componentes/cardlink';
import Row from '../../componentes/row';
import Alert from '../../componentes/alert';
import styled from 'styled-components';

const Principal = (props) => {

  const FREQUENCIA_TYPE = "frequencia";
  const CICLOS_TYPE = "ciclos";
  const ANUAL_TYPE = "anual";

  const [escolaSelecionada, setEscolaSelecionada] = useState(false);

  const ehDisabled = (tipo) => {

    if (!escolaSelecionada)
      return true;

    if (tipo === CICLOS_TYPE)
      return !cicloLiberado();

    return false;
  }

  const cicloLiberado = () => {
    return false;
  }

  const Container = styled.div`
    margin-left: 8px !important;
    margin-right: 8px !important;
    margin-top: 20px !important;
  `;

  const Img = styled.img`
    width: 100% !important;
  `;

  const Label = styled.h5`
    font-size: 16px !important
  `;

  return (
    <div className="col-md-12">
      {
        !escolaSelecionada ?
          <Row className="mb-0 pb-0">
            <Grid cols={12} className="mb-0 pb-0">
              <Container>
                <Alert alerta={{ tipo: "warning", id: "AlertaPrincipal", mensagem: "Você precisa escolher uma turma." }} className="mb-0" />
              </Container>
            </Grid>
          </Row>
          : null
      }
      <Card>
        <Grid cols={12}>
          <Label><span className="fas fa-thumbtack m-r-10 m-b-10"></span>Notificações</Label>
          <Row>
            <Grid cols={12}>
              <Img src="https://i.imgur.com/UOrwcA9.png" />
            </Grid>
          </Row>
        </Grid>
      </Card>
      <Row>
        <Grid cols={12} className="form-inline">
          <CardLink cols={[4, 4, 4, 12]} iconSize="90px" url="/" disabled={(e => ehDisabled(FREQUENCIA_TYPE))()} icone="fa-columns" pack="fas" label="Frequência/ Plano de Aula" />
          <CardLink cols={[4, 4, 4, 12]} classHidden="hidden-xs-down" iconSize="90px" url="/" disabled={(e => ehDisabled(CICLOS_TYPE))()} icone="fa-calendar-minus" pack="far" label="Plano de Ciclo" />
          <CardLink cols={[4, 4, 4, 12]} classHidden="hidden-xs-down" iconSize="90px" url="/planejamento/plano-anual" disabled={(e => ehDisabled(ANUAL_TYPE))()} icone="fa-calendar-alt" pack="far" label="Plano Anual" />
        </Grid>
      </Row>
    </div>
  );
}

export default Principal;
