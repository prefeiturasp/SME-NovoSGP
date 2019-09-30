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
import ListaNotificacoes from './listaNotificacoes';
import modalidade from '~/dtos/modalidade';

const Principal = props => {
  const FREQUENCIA_TYPE = 'frequencia';
  const CICLOS_TYPE = 'ciclos';
  const ANUAL_TYPE = 'anual';
  const [escolaSelecionada, setEscolaSelecionada] = useState(false);
  const [turmaSelecionada, setTurmaSelecionada] = useState(false);
  const [modalidadeEja, setModalidadeEja] = useState(false);

  const filtroStore = useSelector(state => state.usuario);

  useEffect(() => {
    if (props.match.params.rf) {
      const { rf } = props.match.params;
      store.dispatch(salvarRf(rf));
    }

    validarFiltro();
  }, []);

  useEffect(() => {
    validarFiltro();
  }, [filtroStore]);

  useEffect(() => {
    if (
      filtroStore &&
      filtroStore.turmaSelecionada &&
      filtroStore.turmaSelecionada.length &&
      filtroStore.turmaSelecionada[0].codModalidade == modalidade.EJA
    ) {
      setModalidadeEja(true);
    } else {
      setModalidadeEja(false);
    }
  }, [filtroStore.turmaSelecionada]);

  const validarFiltro = () => {
    if (!filtroStore.turmaSelecionada) {
      setTurmaSelecionada(false);
      setEscolaSelecionada(false);
      return;
    }

    const temTurma = filtroStore.turmaSelecionada.length > 0;
    const temEscola =
      temTurma &&
      (filtroStore.turmaSelecionada[0].ue !== '' &&
        typeof filtroStore.turmaSelecionada[0].ue !== 'undefined');

    setTurmaSelecionada(temTurma);
    setEscolaSelecionada(temEscola);
  };

  useEffect(() => {
    if (props.match.params && props.match.params.rf) {
      const { rf } = props.match.params;
      store.dispatch(salvarRf(rf));
    }
  }, []);

  useEffect(() => {
    validarFiltro();
  }, [filtroStore]);

  const cicloLiberado = () => {
    return escolaSelecionada;
  };

  const isDesabilitado = tipo => {
    if (!escolaSelecionada) return true;
    if (tipo === CICLOS_TYPE) return !cicloLiberado();
    return !turmaSelecionada;
  };

  const Container = styled.div`
    margin-left: 8px !important;
    margin-right: 8px !important;
    margin-top: 20px !important;
  `;

  const Label = styled.h5`
    font-size: 16px !important;
  `;

  const Dashboard = styled.div`
    .alinhar-itens-topo{
      align-items: initial !important;
    }
    .card{
      height: 100% !important;
    }
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
              />
            </Container>
          </Grid>
        </Row>
      ) : null}
      <Card className="rounded mb-4">
        <Grid cols={12}>
          <Label>
            <span className="fas fa-thumbtack mr-2 mb-0" />
            Notificações
          </Label>
          <Row className="pt-3 pb-4">
            <Grid cols={12}>
              <ListaNotificacoes />
            </Grid>
          </Row>
        </Grid>
      </Card>
      <Dashboard>
        <Row>
          <Grid cols={12} className="form-inline alinhar-itens-topo">
            <CardLink
              cols={[4, 4, 4, 12]}
              iconSize="90px"
              url={URL_FREQ_PLANO_AULA}
              disabled={isDesabilitado(FREQUENCIA_TYPE)}
              icone="fa-columns"
              pack="fas"
              label="Frequência/ Plano de Aula"
              minHeight="177px"
            />
            <CardLink
              cols={[4, 4, 4, 12]}
              classHidden="hidden-xs-down"
              iconSize="90px"
              url={URL_PLANO_CICLO}
              disabled={isDesabilitado(CICLOS_TYPE)}
              icone="fa-calendar-minus"
              pack="far"
              label={modalidadeEja ? 'Plano de Etapa' : 'Plano de Ciclo'}
              minHeight="177px"
            />
            <CardLink
              cols={[4, 4, 4, 12]}
              classHidden="hidden-xs-down"
              iconSize="90px"
              url={URL_PLANO_ANUAL}
              disabled={isDesabilitado(ANUAL_TYPE)}
              icone="fa-calendar-alt"
              pack="far"
              label={modalidadeEja ? 'Plano Semestral' : 'Plano Anual'}
              minHeight="177px"
            />
          </Grid>
        </Row>
      </Dashboard>
    </div>
  );
};

export default Principal;
