import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  SalvarDisciplinasPlanoAnual,
  PrePost,
  Post,
  setBimestresErro,
  setLimpartBimestresErro,
  LimparDisciplinaPlanoAnual,
  SelecionarDisciplinaPlanoAnual,
  LimparBimestres,
  SalvarBimestres,
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
import { erro, sucesso, confirmar } from '../../../servicos/alertas';
import ModalConteudoHtml from '../../../componentes/modalConteudoHtml';
import PlanoAnualHelper from './planoAnualHelper';
import FiltroPlanoAnualDto from '~/dtos/filtroPlanoAnualDto';
import {
  Titulo,
  TituloAno,
  Planejamento,
  RegistroMigrado, Select,
} from './planoAnual.css.js';
import modalidade from '~/dtos/modalidade';
import SelectComponent from '~/componentes/select';

export default function PlanoAnual() {
  const bimestres = useSelector(store => store.bimestres.bimestres);

  const disciplinasPlanoAnual = useSelector(
    store => store.bimestres.disciplinasPlanoAnual
  );

  const disciplinaSelecionada = useSelector(
    store => store.bimestres.disciplinasPlanoAnual && store.bimestres.disciplinasPlanoAnual.find(x => x.selecionada)
  );

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
    turmaSelecionada[0] && turmaSelecionada[0].codModalidade === modalidade.EJA
      ? true
      : false;

  const ehMedio =
    turmaSelecionada[0] &&
      turmaSelecionada[0].codModalidade === modalidade.ENSINO_MEDIO
      ? true
      : false;

  const [disciplinaSemObjetivo, setDisciplinaSemObjetivo] = useState(false);
  const [modalCopiarConteudo, setModalCopiarConteudo] = useState({
    visivel: false,
    listSelect: [],
    turmasSelecionadas: [],
    turmasComPlanoAnual: [],
    loader: false,
  });

  const recarregarPlanoAnual =
    bimestres.filter(bimestre => bimestre.recarregarPlanoAnual).length > 0;

  const LayoutEspecial = () => ehEja || ehMedio || disciplinaSemObjetivo;

  const anoLetivo = turmaSelecionada[0] ? turmaSelecionada[0].anoLetivo : 0;
  const escolaId = turmaSelecionada[0] ? turmaSelecionada[0].codEscola : 0;
  const anoEscolar = turmaSelecionada[0] ? turmaSelecionada[0].ano : 0;
  const turmaId = turmaSelecionada[0] ? turmaSelecionada[0].codTurma : 0;

  useEffect(() => {
    VerificarEnvio();

    if (recarregarPlanoAnual) verificarSeEhEdicao();
  }, [bimestres]);

  useEffect(() => {
    if (!ehDisabled) obterDisciplinasPlanoAnual();
  }, [turmaSelecionada]);

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

  useEffect(() => {
    verificarSeEhEdicao();
  }, [disciplinaSelecionada]);

  document.onkeydown = onF5Click;
  document.onkeypress = onF5Click;
  document.onkeyup = onF5Click;

  const VerificarEnvio = () => {
    const BimestresParaEnviar = bimestres.filter(x => x.paraEnviar);

    if (BimestresParaEnviar && BimestresParaEnviar.length > 0) {
      dispatch(Post(BimestresParaEnviar, disciplinaSelecionada.codigo));
    }
  };

  const obterDisciplinasPlanoAnual = async () => {
    const disciplinas = await PlanoAnualHelper.ObterDisciplinasPlano(
      usuario.rf,
      turmaId
    );

    dispatch(SalvarDisciplinasPlanoAnual(disciplinas));

    if (disciplinas.length === 1)
      dispatch(SelecionarDisciplinaPlanoAnual(disciplinas[0].codigo));
  };

  const verificarSeEhEdicao = async () => {
    dispatch(LimparBimestres());

    if (!turmaSelecionada[0]) return;

    if (!disciplinaSelecionada) return;

    const filtro = new FiltroPlanoAnualDto(
      anoLetivo,
      1,
      escolaId,
      turmaId,
      disciplinaSelecionada.codigo
    );

    const ehEdicao = await PlanoAnualHelper.verificarSeExiste(filtro, ehEja);

    const disciplinas = await PlanoAnualHelper.ObterDiscplinasObjetivos(
      turmaId,
      disciplinaSelecionada
    );

    const semObjetivos =
      disciplinas && disciplinas.filter(x => !x.possuiObjetivos).length > 0;

    setDisciplinaSemObjetivo(semObjetivos);

    const bimestres = PlanoAnualHelper.ObtenhaBimestres(
      disciplinas,
      ehEdicao,
      filtro,
      anoEscolar,
      LayoutEspecial() || semObjetivos,
      ehEja
    );

    dispatch(SalvarBimestres(bimestres));
  };

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

  const AoMudarDisciplinaPlanoAnual = async e => {
    if (!emEdicao) return alterarValorDisciplina(e);

    const confirmarPerderDados = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas',
      'Deseja realmente cancelar as alterações?',
      'Sim',
      'Não'
    );

    if (confirmarPerderDados) alterarValorDisciplina(e);
  };

  const alterarValorDisciplina = e => {

    const valor = e.target.value * 1;

    if (valor === 0 || !valor) {
      dispatch(LimparDisciplinaPlanoAnual());
      return;
    }

    dispatch(SelecionarDisciplinaPlanoAnual(valor));
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

  const onCopiarConteudoClick = async () => {

    const turmasCopiarConteudo = await PlanoAnualHelper.ObtenhaTurmasCopiarConteudo(anoLetivo, escolaId, turmaId, disciplinaSelecionada, ehEja, usuario, turmaSelecionada);

    if (!turmasCopiarConteudo)
      return;

    modalCopiarConteudo.listSelect = turmasCopiarConteudo;
    modalCopiarConteudo.visivel = true;
    modalCopiarConteudo.turmasComPlanoAnual = turmasCopiarConteudo
      .filter(x => x.temPlano)
      .map(x => x.codigo);

    setModalCopiarConteudo({ ...modalCopiarConteudo });
  };

  const modalCopiarConteudoAlertaVisivel = () => {
    return modalCopiarConteudo.turmasSelecionadas.some(selecionada =>
      modalCopiarConteudo.turmasComPlanoAnual.includes(selecionada * 1)
    );
  };

  const modalCopiarConteudoAtencaoTexto = () => {
    const turmasReportar = usuario.turmasUsuario
      ? usuario.turmasUsuario
        .filter(
          turma =>
            modalCopiarConteudo.turmasSelecionadas.includes(
              `${turma.codigo}`
            ) &&
            modalCopiarConteudo.turmasComPlanoAnual.includes(turma.codigo)
        )
        .map(turma => turma.turma)
      : [];

    return turmasReportar.length > 1
      ? `As turmas ${turmasReportar.join(
        ', '
      )} já possuem plano anual que serão sobrescritos ao realizar a cópia. Deseja continuar?`
      : `A turma ${
      turmasReportar[0]
      } já possui plano anual que será sobrescrito ao realizar a cópia. Deseja continuar?`;
  };

  const onChangeCopiarConteudo = selecionadas => {
    modalCopiarConteudo.turmasSelecionadas = selecionadas;
    setModalCopiarConteudo({ ...modalCopiarConteudo });
  };

  const onCloseCopiarConteudo = () => {
    setModalCopiarConteudo({
      ...modalCopiarConteudo,
      visivel: false,
      listSelect: [],
      turmasSelecionadas: [],
      loader: false,
      turmasComPlanoAnual: [],
    });
  };

  const onConfirmarCopiarConteudo = () => {
    setModalCopiarConteudo({
      ...modalCopiarConteudo,
      loader: true,
    });

    CopiarConteudo().finally(() => onCloseCopiarConteudo());
  };

  const CopiarConteudo = async () => {

    const qtdBimestres = ehEja ? 2 : 4;

    const bimestresCopiar = await PlanoAnualHelper.ObtenhaBimestresCopiarConteudo(anoLetivo, escolaId, turmaId, qtdBimestres, disciplinaSelecionada);

    if (!bimestresCopiar)
      return erro('Não foi possivel copiar o conteudo');

    const planoAnualEnviar = PlanoAnualHelper.TratarBimestresCopiarConteudo(bimestresCopiar, disciplinaSelecionada);

    const retornoCopia = await PlanoAnualHelper.CopiarConteudo(planoAnualEnviar, usuario.rf, modalCopiarConteudo.turmasSelecionadas);

    if (retornoCopia.sucesso)
      return sucesso('Plano copiado com sucesso');

    dispatch(
      setBimestresErro({
        type: 'erro',
        content: retornoCopia.erro.error,
        title: 'Ocorreu uma falha',
        onClose: () => dispatch(setLimpartBimestresErro()),
        visible: true,
      })
    );

  };

  const onCancelarCopiarConteudo = () => {
    onCloseCopiarConteudo();
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
        onConfirmacaoPrincipal={onConfirmarCopiarConteudo}
        onConfirmacaoSecundaria={onCancelarCopiarConteudo}
        onClose={onCloseCopiarConteudo}
        labelBotaoPrincipal="Copiar"
        tituloAtencao={modalCopiarConteudoAlertaVisivel() ? 'Atenção' : null}
        perguntaAtencao={
          modalCopiarConteudoAlertaVisivel()
            ? modalCopiarConteudoAtencaoTexto()
            : null
        }
        labelBotaoSecundario="Cancelar"
        titulo="Copiar Conteúdo"
        closable={false}
        loader={modalCopiarConteudo.loader}
        desabilitarBotaoPrincipal={
          modalCopiarConteudo.turmasSelecionadas &&
          modalCopiarConteudo.turmasSelecionadas.length < 1
        }
      >
        <label
          htmlFor="SelecaoTurma"
          alt="Selecione uma ou mais turmas de destino"
        >
          Copiar para a(s) turma(s)
        </label>
        <SelectComponent
          id="SelecaoTurma"
          lista={modalCopiarConteudo.listSelect}
          valueOption="codigo"
          valueText="turma"
          onChange={onChangeCopiarConteudo}
          valueSelect={modalCopiarConteudo.turmasSelecionadas}
          multiple
        />
      </ModalConteudoHtml>
      <Grid cols={12} className="p-0">
        <Planejamento>PLANEJAMENTO</Planejamento>
        <Titulo>
          {ehEja ? 'Plano Semestral' : 'Plano Anual'}
          <TituloAno>
            {` / ${anoLetivo ? anoLetivo : new Date().getFullYear()}`}
          </TituloAno>
          {bimestres.filter(bimestre => bimestre.migrado).length > 0 && (
            <RegistroMigrado className="float-right">
              Registro Migrado
            </RegistroMigrado>
          )}
        </Titulo>
      </Grid>
      <Card className="col-md-12 p-0" mx="mx-0">
        <Grid cols={8} className="d-flex justify-content-start mb-3">
          <Select
            placeholder="Selecione uma disciplina"
            onChange={AoMudarDisciplinaPlanoAnual}
            disabled={turmaSelecionada[0] ? false : true}
            className="col-md-6 form-control p-r-10">
            <option value={0} selected={!disciplinaSelecionada}>Selecione uma disciplina</option>
            {disciplinasPlanoAnual && disciplinasPlanoAnual.map(disciplina => {
              const selected = disciplinaSelecionada && disciplinaSelecionada.codigo == disciplina.codigo
              return (<option value={disciplina.codigo} selected={selected}>{disciplina.nome}</option>);
            })}
          </Select>
          <Button
            label="Copiar Conteúdo"
            icon="share-square"
            className="ml-3"
            color={Colors.Azul}
            onClick={onCopiarConteudoClick}
            border
            disabled={turmaSelecionada[0] ? false : true}
          />
        </Grid>
        <Grid cols={4} className="d-flex justify-content-end mb-3">
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
          {bimestres && disciplinaSelecionada
            ? bimestres.map(bim => {
              return (
                <Bimestre
                  disabled={ehDisabled}
                  key={bim.indice}
                  indice={bim.indice}
                  modalidadeEja={ehEja}
                  disciplinaSelecionada={
                    disciplinaSelecionada && disciplinaSelecionada.codigo
                  }
                />
              );
            })
            : null}
        </Grid>
      </Card>
    </>
  );
}
