import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import Card from '../../componentes/card';
import Grid from '../../componentes/grid';
import CardLink from '../../componentes/cardlink';
import Row from '../../componentes/row';
import Alert from '../../componentes/alert';
import {
  URL_PLANO_ANUAL,
  URL_PLANO_CICLO,
  URL_FREQ_PLANO_AULA,
} from '../../constantes/url';
import { salvarRf } from '../../redux/modulos/usuario/actions';
import { store } from '../../redux';

const Principal = props => {
  const FREQUENCIA_TYPE = 'frequencia';
  const CICLOS_TYPE = 'ciclos';
  const ANUAL_TYPE = 'anual';
  const [escolaSelecionada, setEscolaSelecionada] = useState(false);
  const [turmaSelecionada, setTurmaSelecionada] = useState(false);

  const FiltroStore = useSelector(store => store.usuario);

  useEffect(() => {
    if (props.match.params.rf) {
      const { rf } = props.match.params;
      store.dispatch(salvarRf(rf));
    }
    validarFiltro();
  }, []);

  useEffect(() => {
    validarFiltro();
  }, [FiltroStore]);

  const validarFiltro = () => {
    if (!FiltroStore.turmaSelecionada) {
      setTurmaSelecionada(false);
      setEscolaSelecionada(false);
      return;
    }

    const temTurma = FiltroStore.turmaSelecionada.length > 0;
    const temEscola =
      temTurma &&
      (FiltroStore.turmaSelecionada[0].ue !== '' &&
        typeof FiltroStore.turmaSelecionada[0].ue !== 'undefined');

    setTurmaSelecionada(temTurma);
    setEscolaSelecionada(temEscola);
  };

  const ehDisabled = tipo => {
    if (!escolaSelecionada) return true;

    if (tipo === CICLOS_TYPE) return !cicloLiberado();

    return !turmaSelecionada;
  };

  const cicloLiberado = () => {
    return escolaSelecionada;
  };

  const Container = styled.div`
    margin-left: 8px !important;
    margin-right: 8px !important;
    margin-top: 20px !important;
  `;

  const Img = styled.img`
    width: 100% !important;
  `;

  const Label = styled.h5`
    font-size: 16px !important;
  `;

  return (
    <div className="col-md-12">
      {!turmaSelecionada ? (
        <Row className="mb-0 pb-0">
          <Grid cols={12} className="mb-0 pb-0">
            <Container>
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'AlertaPrincipal',
                  mensagem: 'Você precisa escolher uma turma.',
                  estiloTitulo: { fontSize: '18px' },
                }}
                className="mb-0"
              />
            </Container>
          </Grid>
        </Row>
      ) : null}
      <Card>
        <Grid cols={12}>
          <Label>
            <span className="fas fa-thumbtack m-r-10 m-b-10" />
            Notificações
          </Label>
          <Row>
            <Grid cols={12}>
              <Img src="https://i.imgur.com/UOrwcA9.png" />
            </Grid>
          </Row>
        </Grid>
      </Card>
      <Row>
        <Grid cols={12} className="form-inline">
          <CardLink
            cols={[4, 4, 4, 12]}
            iconSize="90px"
            url={URL_FREQ_PLANO_AULA}
            disabled={(e => ehDisabled(FREQUENCIA_TYPE))()}
            icone="fa-columns"
            pack="fas"
            label="Frequência/ Plano de Aula"
          />
          <CardLink
            cols={[4, 4, 4, 12]}
            classHidden="hidden-xs-down"
            iconSize="90px"
            url={URL_PLANO_CICLO}
            disabled={(e => ehDisabled(CICLOS_TYPE))()}
            icone="fa-calendar-minus"
            pack="far"
            label="Plano de Ciclo"
          />
          <CardLink
            cols={[4, 4, 4, 12]}
            classHidden="hidden-xs-down"
            iconSize="90px"
            url={URL_PLANO_ANUAL}
            disabled={(e => ehDisabled(ANUAL_TYPE))()}
            icone="fa-calendar-alt"
            pack="far"
            label="Plano Anual"
          />
        </Grid>
      </Row>
    </div>
  );
};

export default Principal;
