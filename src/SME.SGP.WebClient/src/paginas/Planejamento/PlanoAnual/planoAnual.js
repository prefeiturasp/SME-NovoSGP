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
import { URL_HOME } from '../../../constantes/url';
import { erro } from '../../../servicos/alertas';
import { salvarRf } from '../../../redux/modulos/usuario/actions';
import ModalConteudoHtml from '../../../componentes/modalConteudoHtml';
import Select from '../../../componentes/select';

export default function PlanoAnual() {
  const diciplinasSemObjetivo = [1061];

  const bimestres = useSelector(store => store.bimestres.bimestres);
  const notificacoes = useSelector(store => store.notificacoes);
  const bimestresErro = useSelector(store => store.bimestres.bimestresErro);
  const usuario = useSelector(store => store.usuario);

  const turmaSelecionada = usuario.turmaSelecionada;
  const emEdicao = bimestres.filter(x => x.ehEdicao).length > 0;
  const ehDisabled = usuario.turmaSelecionada.length === 0;
  const dispatch = useDispatch();
  const [modalConfirmacaoVisivel, setModalConfirmacaoVisivel] = useState({
    modalVisivel: false,
    sairTela: false,
  });

  const ehEja =
    turmaSelecionada[0] && turmaSelecionada[0].codModalidade === 3
      ? true
      : false;

  const ehMedio =
    turmaSelecionada[0] && turmaSelecionada[0].codModalidade === 6
      ? true
      : false;

  const [disciplinaObjetivo, setDisciplinaObjetivo] = useState(false);
  const [modalCopiarConteudo, setModalCopiarConteudo] = useState({
    visivel: false,
  });

  const LayoutEspecial = ehEja || ehMedio || disciplinaObjetivo;

  const qtdBimestres = ehEja ? 2 : 4;

  const anoLetivo = turmaSelecionada[0] ? turmaSelecionada[0].anoLetivo : 0;
  const escolaId = turmaSelecionada[0] ? turmaSelecionada[0].codEscola : 0;
  const anoEscolar = turmaSelecionada[0] ? turmaSelecionada[0].ano : 0;
  const turmaId = turmaSelecionada[0] ? turmaSelecionada[0].codTurma : 0;

  useEffect(() => {}, []);

  useEffect(() => {
    console.log(modalCopiarConteudo);
  }, [modalCopiarConteudo]);

  useEffect(() => {
    if ((!bimestres || bimestres.length === 0) && !ehDisabled)
      ObtenhaBimestres();

    verificarSeEhEdicao();
  }, [usuario]);

  useEffect(() => {
    VerificarEnvio();
  }, [bimestres]);

  const onF5Click = e => {
    if (e.code === 'F5') {
      if (emEdicao) {
        e.preventDefault();
        setModalConfirmacaoVisivel({
          modalVisivel: true,
          sairTela: false,
        });
      }
    }
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
        Service.getDisciplinasProfessor(usuario.rf, turmaId)
          .then(res => {
            ObtenhaBimestres(_.cloneDeep(res), !ehEdicao);
          })
          .catch(() => {
            erro(`Não foi possivel obter as disciplinas do professor`);
          });
      })
      .catch(() => {
        erro(
          `Não foi possivel obter os dados do ${
            ehEja ? 'plano semestral' : 'plano anual'
          }`
        );
      });
  };

  const ObtenhaNomebimestre = index =>
    `${index}º ${ehEja ? 'Semestre' : 'Bimestre'}`;

  const confirmarCancelamento = () => {
    if (modalConfirmacaoVisivel.sairTela) {
      history.push(URL_HOME);
    } else {
      cancelarModalConfirmacao();
      verificarSeEhEdicao();
    }
  };

  const onClickCancelar = () => {
    verificarSeEhEdicao();
  };

  const cancelarModalConfirmacao = () => {
    setModalConfirmacaoVisivel({
      modalVisivel: false,
      sairTela: false,
    });
  };

  const onClickSalvar = () => {
    dispatch(PrePost());
  };

  const onCopiarConteudoClick = () => {
    Service.obterBimestre({
      AnoLetivo: anoLetivo,
      Bimestre: 1,
      EscolaId: escolaId,
      TurmaId: turmaId,
    })
      .then(res => {
        if (res.status === 200) {
          modalCopiarConteudo.visivel = true;
          //setModalCopiarConteudo({ ...modalCopiarConteudo });
          ObtenhaTurmasCopiarConteudo();
        } else {
          erro('Este plano ainda não foi salvo na base de dados');
        }
      })
      .catch(() => {
        erro('Não foi possivel obter os dados do plano');
      });
  };

  const ObtenhaBimestres = (disciplinas = [], ehEdicao) => {
    console.log(disciplinas);

    let semObjetivo = false;

    if (disciplinas.length === 1) {
      const arraySemObjetivo = diciplinasSemObjetivo.filter(
        x => x === disciplinas[0].codigo
      );

      if (arraySemObjetivo.length > 0) semObjetivo = true;
    }

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
        LayoutEspecial: LayoutEspecial || semObjetivo,
        ehExpandido: ehEdicao,
        jaSincronizou: false,
      };

      dispatch(Salvar(i, bimestre));
    }

    setDisciplinaObjetivo(semObjetivo);
  };

  const ObtenhaTurmasCopiarConteudo = () => {
    const anoEscolar = turmaSelecionada[0].ano;

    console.log(usuario);
  };

  const voltarParaHome = () => {
    if (emEdicao)
      setModalConfirmacaoVisivel({
        modalVisivel: true,
        sairTela: true,
      });
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
        visivel={modalConfirmacaoVisivel.modalVisivel}
        onConfirmacaoPrincipal={cancelarModalConfirmacao}
        onConfirmacaoSecundaria={confirmarCancelamento}
        onClose={cancelarModalConfirmacao}
        labelPrincipal="Não"
        labelSecundaria="Sim"
        titulo="Atenção"
        conteudo="Você não salvou as informações preenchidas"
        perguntaDoConteudo="Deseja realmente cancelar as alterações?"
      />
      <ModalConteudoHtml
        key={'copiarConteudo'}
        visivel={modalCopiarConteudo.visivel}
        onConfirmacaoPrincipal={() => {}}
        onConfirmacaoSecundaria={() => {}}
        onClose={() => {}}
        labelBotaoPrincipal="Copiar"
        labelBotaoSecundario="Cancelar"
        titulo="Copiar Conteúdo"
      ></ModalConteudoHtml>
      <Card>
        <Grid cols={12}>
          {ehEja ? <h1>Plano Semestral</h1> : <h1>Plano Anual</h1>}
        </Grid>
        <Grid cols={6} className="d-flex justify-content-start mb-3">
          <Button
            label="Copiar Conteúdo"
            icon="share-square"
            color={Colors.Azul}
            onClick={onCopiarConteudoClick}
            border
            disabled={turmaSelecionada[0] ? false : true}
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
            border={!emEdicao || ehDisabled}
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
                    LayoutEspecial={LayoutEspecial}
                  />
                );
              })
            : null}
        </Grid>
      </Card>
    </>
  );
}
