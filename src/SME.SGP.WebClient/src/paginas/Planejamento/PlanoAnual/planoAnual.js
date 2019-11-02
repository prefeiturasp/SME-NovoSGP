import React, { useEffect, useState, useRef } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  SalvarDisciplinasPlanoAnual,
  PrePost,
  RemoverFocado,
  Post,
  Salvar,
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
  RegistroMigrado,
  Select,
} from './planoAnual.css.js';
import modalidade from '~/dtos/modalidade';
import SelectComponent from '~/componentes/select';
import { store } from '~/redux';
import FiltroPlanoAnualExpandidoDto from '~/dtos/filtroPlanoAnualExpandidoDto';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

export default function PlanoAnual() {
  const bimestres = useSelector(state => state.bimestres.bimestres);

  const bimestreFocado = useSelector(state =>
    state.bimestres.bimestres.find(x => x && x.focado)
  );

  const disciplinasPlanoAnual = useSelector(
    store => store.bimestres.disciplinasPlanoAnual
  );

  const disciplinaSelecionada = useSelector(
    store =>
      store.bimestres.disciplinasPlanoAnual &&
      store.bimestres.disciplinasPlanoAnual.find(x => x.selecionada)
  );

  const bimestresErro = useSelector(store => store.bimestres.bimestresErro);
  const usuario = useSelector(store => store.usuario);

  const permissoesTela = usuario.permissoes[RotasDto.PLANO_ANUAL];
  const [somenteConsulta, setSomenteConsulta] = useState(verificaSomenteConsulta(permissoesTela));

  const turmaSelecionada = usuario.turmaSelecionada;
  const emEdicao = bimestres.filter(x => x.ehEdicao).length > 0;
  const ehDisabled = somenteConsulta || !permissoesTela.podeAlterar? true : !usuario.turmaSelecionada.turma;
  const dispatch = useDispatch();
  const [modalConfirmacaoVisivel, setModalConfirmacaoVisivel] = useState({
    modalVisivel: false,
    sairTela: false,
  });

  const refFocado = useRef(null);

  const ehEja =
    turmaSelecionada && turmaSelecionada.codModalidade === modalidade.EJA
      ? true
      : false;

  const ehMedio =
    turmaSelecionada &&
    turmaSelecionada.codModalidade === modalidade.ENSINO_MEDIO
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

  const anoLetivo = turmaSelecionada ? turmaSelecionada.anoLetivo : 0;
  const escolaId = turmaSelecionada ? turmaSelecionada.unidadeEscolar : 0;
  const anoEscolar = turmaSelecionada ? turmaSelecionada.ano : 0;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  useEffect(() => {
    VerificarEnvio();

    if (recarregarPlanoAnual) verificarSeEhEdicao();
  }, [bimestres]);

  useEffect(() => {
    document.addEventListener('keydown', onF5Click, true);
    document.addEventListener('keyup', onF5Click, true);

    return () => {
      document.removeEventListener('keydown', null);
      document.removeEventListener('keyup', null);

      if (bimestres) dispatch(LimparBimestres());

      if (disciplinasPlanoAnual) dispatch(LimparDisciplinaPlanoAnual());
    };
  }, []);

  useEffect(() => {
    if (!ehDisabled) obterDisciplinasPlanoAnual();
  }, [turmaSelecionada]);

  function onF5Click(e) {
    const bimestres = store.getState().bimestres.bimestres;
    const emEdicao = bimestres.filter(x => x.ehEdicao).length > 0;

    if (e.code === 'F5') {
      if (emEdicao) {
        e.preventDefault();
        setModalConfirmacaoVisivel({
          modalVisivel: true,
          sairTela: false,
        });
      }
    }
  }

  useEffect(() => {
    if (bimestreFocado && refFocado.current) {
      refFocado.current.scrollIntoViewIfNeeded(refFocado.current);

      dispatch(RemoverFocado());
    }
  }, [bimestreFocado]);

  useEffect(() => {
    verificarSeEhEdicao();
  }, [disciplinaSelecionada]);

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

    if (disciplinas && disciplinas.length === 1)
      dispatch(SelecionarDisciplinaPlanoAnual(disciplinas[0].codigo));
  };

  const verificarSeEhEdicao = async () => {
    dispatch(LimparBimestres());

    if (!turmaSelecionada) return;

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

    if (ehEdicao) {
      const filtro = new FiltroPlanoAnualExpandidoDto(
        anoLetivo,
        disciplinaSelecionada.codigo,
        escolaId,
        turmaSelecionada.modalidade,
        turmaId
      );

      const retornoBimestre = await PlanoAnualHelper.ObterBimestreExpandido(
        filtro
      );

      if (!retornoBimestre.sucesso) return;

      const bimestreExpandido = retornoBimestre.bimestre;

      if (!bimestreExpandido) return;

      dispatch(
        Salvar(bimestreExpandido.bimestre, {
          ...bimestres[bimestreExpandido.bimestre],
          objetivo: bimestreExpandido.descricao,
          ehExpandido: true,
          id: bimestreExpandido.id,
          alteradoPor: bimestreExpandido.alteradoPor,
          alteradoEm: bimestreExpandido.alteradoEm,
          alteradoRF: bimestreExpandido.alteradoRF,
          criadoRF: bimestreExpandido.criadoRF,
          criadoEm: bimestreExpandido.criadoEm,
          focado: true,
          LayoutEspecial:
            bimestreExpandido.migrado ||
            (bimestres[bimestreExpandido.bimestre] &&
              bimestres[bimestreExpandido.bimestre].LayoutEspecial),
          migrado: bimestreExpandido.migrado,
          criadoPor: bimestreExpandido.criadoPor,
          objetivosAprendizagem: bimestreExpandido.objetivosAprendizagem
            ? bimestreExpandido.objetivosAprendizagem.map(obj => {
                obj.selected = true;
                return obj;
              })
            : [],
        })
      );
    }
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
    const valor = e.target && e.target.value * 1;

    if (!emEdicao) return alterarValorDisciplina(valor);

    const confirmarPerderDados = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas',
      'Deseja realmente cancelar as alterações?',
      'Sim',
      'Não'
    );

    if (confirmarPerderDados) alterarValorDisciplina(valor);
  };

  const alterarValorDisciplina = valor => {
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
    const turmasCopiarConteudo = await PlanoAnualHelper.ObtenhaTurmasCopiarConteudo(
      anoLetivo,
      escolaId,
      turmaId,
      disciplinaSelecionada,
      ehEja,
      usuario,
      turmaSelecionada
    );

    if (!turmasCopiarConteudo) return;

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

    const bimestresCopiar = await PlanoAnualHelper.ObtenhaBimestresCopiarConteudo(
      anoLetivo,
      escolaId,
      turmaId,
      qtdBimestres,
      disciplinaSelecionada
    );

    if (!bimestresCopiar) return erro('Não foi possivel copiar o conteudo');

    const planoAnualEnviar = PlanoAnualHelper.TratarBimestresCopiarConteudo(
      bimestresCopiar,
      disciplinaSelecionada
    );

    const retornoCopia = await PlanoAnualHelper.CopiarConteudo(
      planoAnualEnviar,
      usuario.rf,
      modalCopiarConteudo.turmasSelecionadas
    );

    if (retornoCopia.sucesso) return sucesso('Plano copiado com sucesso');

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
        {' '}
        {!turmaSelecionada.turma ? (
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
            </Grid>{' '}
          </Row>
        ) : null}{' '}
      </div>{' '}
      <ModalMultiLinhas
        key="errosBimestre"
        visivel={bimestresErro.visible}
        onClose={bimestresErro.onClose}
        type={bimestresErro.type}
        conteudo={bimestresErro.content}
        titulo={bimestresErro.title}
      />{' '}
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
          Copiar para a(s) turma(s){' '}
        </label>{' '}
        <SelectComponent
          id="SelecaoTurma"
          lista={modalCopiarConteudo.listSelect}
          valueOption="codigo"
          valueText="turma"
          onChange={onChangeCopiarConteudo}
          valueSelect={modalCopiarConteudo.turmasSelecionadas}
          multiple
        />
      </ModalConteudoHtml>{' '}
      <Grid cols={12} className="p-0">
        <Planejamento> PLANEJAMENTO </Planejamento>{' '}
        <Titulo>
          {' '}
          {ehEja ? 'Plano Semestral' : 'Plano Anual'}{' '}
          <TituloAno>
            {' '}
            {` / ${anoLetivo ? anoLetivo : new Date().getFullYear()}`}{' '}
          </TituloAno>{' '}
          {bimestres.filter(bimestre => bimestre.migrado).length > 0 && (
            <RegistroMigrado className="float-right">
              Registro Migrado{' '}
            </RegistroMigrado>
          )}{' '}
        </Titulo>{' '}
      </Grid>{' '}
      <Card className="col-md-12 p-0" mx="mx-0">
        <Grid cols={8} className="d-flex justify-content-start mb-3">
          <Select
            placeholder="Selecione uma disciplina"
            onChange={AoMudarDisciplinaPlanoAnual}
            disabled={
              ehDisabled ||
              (disciplinasPlanoAnual && disciplinasPlanoAnual.length === 1)
            }
            className="col-md-6 form-control p-r-10"
            value={disciplinaSelecionada ? disciplinaSelecionada.codigo : 0}
          >
            <option value={0}> Selecione uma disciplina </option>{' '}
            {disciplinasPlanoAnual &&
              disciplinasPlanoAnual.map(disciplina => {
                return (
                  <option
                    key={disciplina.codigo + disciplina.nome}
                    value={disciplina.codigo}
                  >
                    {' '}
                    {disciplina.nome}{' '}
                  </option>
                );
              })}{' '}
          </Select>{' '}
          <Button
            label="Copiar Conteúdo"
            icon="share-square"
            className="ml-3"
            color={Colors.Azul}
            onClick={onCopiarConteudoClick}
            border
            disabled={
              ehDisabled || (turmaSelecionada && !emEdicao ? false : true)
            }
          />{' '}
        </Grid>{' '}
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
            disabled={somenteConsulta}
          />
          <Button
            label="Salvar"
            color={Colors.Roxo}
            onClick={onClickSalvar}
            disabled={!emEdicao || ehDisabled}
            border={!emEdicao || ehDisabled}
            bold
          />
        </Grid>{' '}
        <Grid cols={12}>
          {' '}
          {bimestres && disciplinaSelecionada
            ? bimestres.map(bim => {
                console.log(bim.focado);
                return (
                  <Bimestre
                    ref={bim.focado ? refFocado : null}
                    disabled={ehDisabled}
                    key={bim.indice}
                    indice={bim.indice}
                    focado={bim.focado}
                    modalidadeEja={ehEja}
                    disciplinaSelecionada={
                      disciplinaSelecionada && disciplinaSelecionada.codigo
                    }
                  />
                );
              })
            : null}{' '}
        </Grid>{' '}
      </Card>{' '}
    </>
  );
}
