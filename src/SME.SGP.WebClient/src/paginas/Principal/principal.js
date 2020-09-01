import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CardsDashboard from '~/componentes-sgp/cardsDashboard/cardsDashboard';
import {
  setCarregandoDadosCardsDashboard,
  setDadosCardsDashboard,
} from '~/redux/modulos/dashboard/actions';
import ServicoDashboard from '~/servicos/Paginas/Dashboard/ServicoDashboard';
import Alert from '../../componentes/alert';
import Card from '../../componentes/card';
import Grid from '../../componentes/grid';
import Row from '../../componentes/row';
import ListaNotificacoes from './listaNotificacoes';
import { Container, Label } from './principal.css';

const Principal = () => {
  const [turmaSelecionada, setTurmaSelecionada] = useState(false);

  const usuario = useSelector(state => state.usuario);
  const perfil = useSelector(state => state.perfil.perfilSelecionado);
  const modalidades = useSelector(state => state.filtro.modalidades);

  const dispatch = useDispatch();

  const validarFiltro = useCallback(() => {
    if (!usuario.turmaSelecionada) {
      setTurmaSelecionada(false);
      return;
    }

    const temTurma = !!usuario.turmaSelecionada.turma;

    setTurmaSelecionada(temTurma);
  }, [usuario.turmaSelecionada]);

  useEffect(() => {
    validarFiltro();
  }, [usuario, validarFiltro]);

  const obterDadosDashboard = useCallback(async () => {
    dispatch(setDadosCardsDashboard([]));
    dispatch(setCarregandoDadosCardsDashboard(true));
    const retorno = await ServicoDashboard.obterDadosDashboard();

    if (retorno && retorno.data && retorno.data.length) {
      dispatch(setDadosCardsDashboard(retorno.data));
    }
    dispatch(setCarregandoDadosCardsDashboard(false));
  }, [dispatch]);

  useEffect(() => {
    obterDadosDashboard();
  }, []);

  return (
    <div className="col-md-12">
      {modalidades &&
      !modalidades.length &&
      !usuario.ehProfessorCj &&
      !usuario.ehProfessor &&
      !usuario.ehProfessorInfantil &&
      !usuario.ehProfessorCjInfantil &&
      !usuario.ehProfessorPoa &&
      perfil &&
      perfil.nomePerfil === 'Supervisor' ? (
        <Row className="mb-0 pb-0">
          <Grid cols={12} className="mb-0 pb-0">
            <Container>
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'AlertaPrincipal',
                  mensagem: `Não foi possível obter as escolas atribuídas ao supervisor ${usuario.rf}`,
                  estiloTitulo: { fontSize: '18px' },
                }}
              />
            </Container>
          </Grid>
        </Row>
      ) : null}
      {!turmaSelecionada ? (
        <Row className="mb-0 pb-0">
          <Grid cols={12} className="mb-0 pb-0">
            <Container>
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'AlertaPrincipal',
                  mensagem: 'Você precisa escolher uma turma',
                  estiloTitulo: { fontSize: '18px' },
                }}
              />
            </Container>
          </Grid>
        </Row>
      ) : null}
      <Card className="rounded mb-4">
        <div className="col-xs-12 col-sm-12 col-md-12 col-lg-12">
          <Label>
            <span className="fas fa-thumbtack mr-2 mb-0" />
            Notificações
          </Label>
          <div className="row pt-3 pb-4">
            <div className="col-xs-12 col-sm-12 col-md-12 col-lg-12">
              <ListaNotificacoes />
            </div>
          </div>
        </div>
      </Card>
      <CardsDashboard />
    </div>
  );
};

export default Principal;
