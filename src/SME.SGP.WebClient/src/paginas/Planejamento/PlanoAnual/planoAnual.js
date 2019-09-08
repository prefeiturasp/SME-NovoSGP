import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  Salvar,
  PrePost,
  Post,
} from '../../../redux/modulos/planoAnual/action';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';
import { Colors, Base } from '../../../componentes/colors';
import _ from 'lodash';
import Card from '../../../componentes/card';
import Bimestre from './bimestre';
import Row from '../../../componentes/row';
import Service from '../../../servicos/Paginas/PlanoAnualServices';
import Alert from '../../../componentes/alert';
import ModalMultiLinhas from '../../../componentes/modalMultiLinhas';
import ModalConfirmacao from '../../../componentes/modalConfirmacao';
import history from '../../../servicos/history';
import { URL_PLANO_ANUAL, URL_HOME } from '../../../constantes/url';

export default function PlanoAnual() {
  const bimestres = useSelector(store => store.bimestres.bimestres);
  const notificacoes = useSelector(store => store.notificacoes);
  const bimestresErro = useSelector(store => store.bimestres.bimestresErro);
  const usuario = useSelector(store => store.usuario);

  const turmaSelecionada = usuario.turmaSelecionada;
  const emEdicao = bimestres.filter(x => x.ehEdicao).length > 0;
  const ehDisabled = usuario.turmaSelecionada.length === 0;
  const dispatch = useDispatch();
  const [modalConfirmacaoVisivel, setModalConfirmacaoVisivel] = useState(false);

  const ehEja =
    turmaSelecionada[0] && turmaSelecionada[0].codModalidade === 3
      ? true
      : false;

  const qtdBimestres = ehEja ? 2 : 4;

  const anoLetivo = turmaSelecionada[0] ? turmaSelecionada[0].anoLetivo : 0;
  const escolaId = turmaSelecionada[0] ? turmaSelecionada[0].codEscola : 0;
  const anoEscolar = turmaSelecionada[0] ? turmaSelecionada[0].ano : 0;
  const turmaId = turmaSelecionada[0] ? turmaSelecionada[0].codTurma : 0;

  useEffect(() => {
    window.onpopstate = onBackButtonEvent;
  }, []);

  useEffect(() => {
    if ((!bimestres || bimestres.length === 0) && !ehDisabled)
      ObtenhaBimestres();

    verificarSeEhEdicao();
  }, [usuario]);

  useEffect(() => {
    VerificarEnvio();

    console.log(bimestres);
  }, [bimestres]);

  const onF5Click = e => {
    if ((e.which || e.keyCode) == 116) {
      console.log(emEdicao);
      if (emEdicao) {
        e.preventDefault();
        setModalConfirmacaoVisivel(true);
      }
    }
  };

  const onBackButtonEvent = e => {
    console.log(e);
  };

  document.onkeydown = onF5Click;
  document.onkeypress = onF5Click;
  document.onkeyup = onF5Click;

  const VerificarEnvio = () => {
    const paraEnviar = bimestres.map(x => x.paraEnviar).filter(x => x);

    if (paraEnviar && paraEnviar.length > 0) dispatch(Post(bimestres));
  };

  const verificarSeEhEdicao = () => {
    if (!turmaSelecionada[0]) return;

    Service.obterBimestre({
      AnoLetivo: anoLetivo,
      Bimestre: 1,
      EscolaId: escolaId,
      TurmaId: turmaId,
    })
      .then(res => {
        const ehEdicao = res.status === 200;
        Service.getDisciplinasProfessor(usuario.rf, turmaId).then(res => {
          ObtenhaBimestres(_.cloneDeep(res), !ehEdicao);
        });
      })
      .catch(() => {});
  };

  const ObtenhaNomebimestre = index =>
    `${index}º ${ehEja ? 'Semestre' : 'Bimestre'}`;

  const confirmarCancelamento = () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = () => {
    verificarSeEhEdicao();
  };

  const cancelarModalConfirmacao = () => {
    setModalConfirmacaoVisivel(false);
  };

  const onClickSalvar = () => {
    dispatch(PrePost());
  };

  const ObtenhaBimestres = (disciplinas = [], ehEdicao) => {
    for (let i = 1; i <= qtdBimestres; i++) {
      const Nome = ObtenhaNomebimestre(i);

      const objetivo = '';

      const bimestre = {
        anoLetivo,
        anoEscolar,
        escolaId,
        turmaId,
        ehExpandido: false,
        indice: i,
        nome: Nome,
        materias: disciplinas,
        objetivo: objetivo,
        paraEnviar: false,
        ehEdicao,
        ehExpandido: ehEdicao,
        jaSincronizou: false,
      };

      dispatch(Salvar(i, bimestre));
    }
  };

  const voltarParaHome = () => {
    if (emEdicao) setModalConfirmacaoVisivel(true);
    else history.push(URL_HOME);
  };

  return (
    <>
      <div className="col-md-12">
        {!turmaSelecionada[0] ? (
          <Row className="mb-0 pb-0">
            <Grid cols={12} className="mb-0 pb-0">
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'AlertaPrincipal',
                  mensagem: 'Você precisa escolher uma turma.',
                }}
                className="mb-0"
              />
            </Grid>
          </Row>
        ) : null}
        {notificacoes.alertas.map(alerta => (
          <Alert alerta={alerta} key={alerta.id} />
        ))}
      </div>
      <ModalMultiLinhas
        key="errosBimestre"
        visivel={bimestresErro.visible}
        onClose={bimestresErro.onClose}
        type={bimestresErro.type}
        conteudo={bimestresErro.content}
        titulo={bimestresErro.title}
      />
      <ModalConfirmacao
        key="confirmacaoDeSaida"
        visivel={modalConfirmacaoVisivel}
        onConfirmacaoPrincipal={cancelarModalConfirmacao}
        onConfirmacaoSecundaria={confirmarCancelamento}
        onClose={cancelarModalConfirmacao}
        labelPrincipal="Não"
        labelSecundaria="Sim"
        titulo="Atenção"
        conteudo="Você não salvou as informações preenchidas"
        perguntaDoConteudo="Deseja realmente cancelar as alterações?"
      />
      <Card>
        <Grid cols={12}>
          {ehEja ? <h1>Plano Semestral</h1> : <h1>Plano Anual</h1>}
        </Grid>
        <Grid cols={6} className="d-flex justify-content-start mb-3">
          <Button
            label="Migrar Conteúdo"
            icon="share-square"
            color={Colors.Azul}
            border
            disabled
          />
        </Grid>
        <Grid cols={6} className="d-flex justify-content-end mb-3">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            onClick={voltarParaHome}
            border
            className="mr-3"
          />
          <Button
            label="Cancelar"
            color={Colors.Roxo}
            onClick={onClickCancelar}
            border
            bold
            className="mr-3"
          />
          <Button
            label="Salvar"
            color={Colors.Roxo}
            onClick={onClickSalvar}
            disabled={!emEdicao || ehDisabled}
            border
            bold
          />
        </Grid>
        <Grid cols={12}>
          {bimestres
            ? bimestres.map(bim => {
                return (
                  <Bimestre
                    disabled={ehDisabled}
                    key={bim.indice}
                    indice={bim.indice}
                  />
                );
              })
            : null}
        </Grid>
      </Card>
    </>
  );
}
