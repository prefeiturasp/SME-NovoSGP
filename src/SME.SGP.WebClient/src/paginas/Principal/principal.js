import React, { useState, useEffect, useCallback } from 'react';
import { useSelector } from 'react-redux';
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
import ListaNotificacoes from './listaNotificacoes';
import modalidade from '~/dtos/modalidade';
import { Container, Label, Dashboard } from './principal.css';
import { obterDescricaoNomeMenu } from '~/servicos/servico-navegacao';
import RotasDto from '~/dtos/rotasDto';

const Principal = () => {
  const FREQUENCIA_TYPE = 'frequencia';
  const CICLOS_TYPE = 'ciclos';
  const ANUAL_TYPE = 'anual';
  const [escolaSelecionada, setEscolaSelecionada] = useState(false);
  const [turmaSelecionada, setTurmaSelecionada] = useState(false);

  const usuario = useSelector(state => state.usuario);
  const perfil = useSelector(state => state.perfil.perfilSelecionado);
  const modalidades = useSelector(state => state.filtro.modalidades);

  const validarFiltro = useCallback(() => {
    if (!usuario.turmaSelecionada) {
      setTurmaSelecionada(false);
      setEscolaSelecionada(false);
      return;
    }

    const temTurma = !!usuario.turmaSelecionada.turma;
    const temEscola = !!usuario.turmaSelecionada.unidadeEscolar;

    setTurmaSelecionada(temTurma);
    setEscolaSelecionada(temEscola);
  }, [usuario.turmaSelecionada]);

  useEffect(() => {
    validarFiltro();
  }, [usuario, validarFiltro]);

  const cicloLiberado = () => {
    return escolaSelecionada;
  };

  const isDesabilitado = tipo => {
    if (!escolaSelecionada) return true;
    if (tipo === CICLOS_TYPE) return !cicloLiberado();
    return !turmaSelecionada;
  };

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
              label={obterDescricaoNomeMenu(
                RotasDto.FREQUENCIA_PLANO_AULA,
                modalidades,
                usuario.turmaSelecionada
              )}
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
              label={obterDescricaoNomeMenu(
                RotasDto.PLANO_CICLO,
                modalidades,
                usuario.turmaSelecionada
              )}
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
              label={obterDescricaoNomeMenu(
                RotasDto.PLANO_ANUAL,
                modalidades,
                usuario.turmaSelecionada
              )}
              minHeight="177px"
            />
          </Grid>
        </Row>
      </Dashboard>
    </div>
  );
};

export default Principal;
