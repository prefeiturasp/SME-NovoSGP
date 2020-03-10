import * as moment from 'moment';
import React, { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';

import Alert from '../../../componentes/alert';
import Button from '../../../componentes/button';
import Card from '../../../componentes/card';
import { Colors } from '../../../componentes/colors';
import SelectComponent from '../../../componentes/select';
import TextEditor from '../../../componentes/textEditor';
import { erro, sucesso, confirmar } from '../../../servicos/alertas';
import api from '../../../servicos/api';
import history from '../../../servicos/history';
import {
  Badge,
  BtnLink,
  InseridoAlterado,
  ListaItens,
  Titulo,
  TituloAno,
} from './planoCiclo.css';
import modalidade from '~/dtos/modalidade';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import tipoPermissao from '~/dtos/tipoPermissao';
import { Loader } from '~/componentes';
import { RegistroMigrado } from '~/componentes-sgp/registro-migrado';

export default function PlanoCiclo() {
  const urlPrefeitura = 'https://curriculo.sme.prefeitura.sp.gov.br';
  const urlMatrizSaberes = `${urlPrefeitura}/matriz-de-saberes`;
  const urlODS = `${urlPrefeitura}/ods`;
  const textEditorRef = useRef(null);

  const [listaMatriz, setListaMatriz] = useState([]);
  const [listaODS, setListaODS] = useState([]);
  const [carregandoCiclos, setCarregandoCiclos] = useState(true);
  const [listaCiclos, setListaCiclos] = useState([]);
  const [cicloSelecionado, setCicloSelecionado] = useState('');
  const [descricaoCiclo, setDescricaoCiclo] = useState('');
  const [modoEdicao, setModoEdicao] = useState(false);
  const [pronto, setPronto] = useState(false);
  const [eventoTrocarCiclo, setEventoTrocarCiclo] = useState(false);
  const [registroMigrado, setRegistroMigrado] = useState(false);
  const [cicloParaTrocar, setCicloParaTrocar] = useState('');
  const [inseridoAlterado, setInseridoAlterado] = useState({
    alteradoEm: '',
    alteradoPor: '',
    criadoEm: '',
    criadoPor: '',
  });
  let [listaMatrizSelecionda, setListaMatrizSelecionda] = useState([]);
  let [listaODSSelecionado, setListaODSSelecionado] = useState([]);
  const [anosTurmasUsuario, setAnosTurmasUsuario] = useState([]);
  const [planoCicloId, setPlanoCicloId] = useState(0);
  const [modalidadeEja, setModalidadeEja] = useState(false);
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [campoCicloDesabilitado, setCampoCicloDesabilitado] = useState(false);

  const usuario = useSelector(store => store.usuario);
  const turmaSelecionada = useSelector(store => store.usuario.turmaSelecionada);
  const permissoesTela = usuario.permissoes[RotasDto.PLANO_CICLO];

  useEffect(() => {
    async function carregarListas() {
      const matrizes = await api.get('v1/matrizes-saber');
      setListaMatriz(matrizes.data);

      const ods = await api.get('v1/objetivos-desenvolvimento-sustentavel');
      setListaODS(ods.data);
    }
    carregarListas();
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, []);

  useEffect(() => {
    let anosTurmasUsuario = usuario.turmasUsuario.map(item => item.ano);
    anosTurmasUsuario = anosTurmasUsuario.filter(
      (elem, pos) => anosTurmasUsuario.indexOf(elem) == pos
    );
    setAnosTurmasUsuario(anosTurmasUsuario);
  }, [usuario.turmasUsuario]);

  const [carregando, setCarregando] = useState(false);
  const [carregandoSalvar, setCarregandoSalvar] = useState(false);

  useEffect(() => {
    setCarregando(true);
    carregarCiclos();

    if (!Object.entries(turmaSelecionada).length) setCicloSelecionado();
  }, [turmaSelecionada]);

  const carregarCiclos = async () => {
    if (usuario && turmaSelecionada.turma) {
      let anoSelecionado = '';
      let codModalidade = null;

      if (turmaSelecionada.turma) {
        anoSelecionado = String(turmaSelecionada.ano);
        codModalidade = turmaSelecionada.modalidade;
      }

      const params = {
        anoSelecionado,
        modalidade: codModalidade,
      };

      let anos = [];
      if (
        usuario.turmasUsuario &&
        usuario.turmasUsuario.length &&
        anosTurmasUsuario.length < 1
      ) {
        anos = usuario.turmasUsuario.map(item => item.ano);
        anos = anos.filter((elem, pos) => anos.indexOf(elem) == pos);
      }

      if (anosTurmasUsuario.length < 1 && anos.length > 0) {
        setAnosTurmasUsuario(anos);
        params.anos = anos;
      } else {
        params.anos = anosTurmasUsuario;
      }

      const ciclos = await api.post('v1/ciclos/filtro', params);

      setSomenteConsulta(ciclos.status === 204);

      if (ciclos.status === 204) {
        setCarregandoCiclos(false);
        setCarregando(false);
        setListaCiclos([]);
        setPlanoCicloId('');
        return;
      }

      let sugestaoCiclo = ciclos.data.find(item => item.selecionado);
      if (sugestaoCiclo && sugestaoCiclo.id) {
        sugestaoCiclo = sugestaoCiclo.id;
      }
      const listaCiclosAtual = ciclos.data.filter(item => !item.selecionado);

      setListaCiclos(listaCiclosAtual);

      if (sugestaoCiclo) {
        setCicloSelecionado(String(sugestaoCiclo));
      } else {
        setCicloSelecionado(String(listaCiclosAtual[0]));
      }

      setCarregandoCiclos(false);

      const anoLetivo = String(turmaSelecionada.anoLetivo);
      const codEscola = String(turmaSelecionada.unidadeEscolar);

      if (turmaSelecionada.modalidade == modalidade.EJA) {
        setModalidadeEja(true);
      } else {
        setModalidadeEja(false);
      }
      obterCicloExistente(
        anoLetivo,
        codEscola,
        String(sugestaoCiclo) || String(listaCiclosAtual[0])
      );
    }
    setCarregando(false);
  };

  function resetListas() {
    listaMatriz.forEach(item => {
      const target = document.getElementById(`matriz-${item.id}`);
      const estaSelecionado =
        target.getAttribute('opcao-selecionada') === 'true';
      if (estaSelecionado) {
        target.setAttribute('opcao-selecionada', 'false');
      }
    });
    listaODS.forEach(item => {
      const target = document.getElementById(`ods-${item.id}`);
      const estaSelecionado =
        target.getAttribute('opcao-selecionada') === 'true';
      if (estaSelecionado) {
        target.setAttribute('opcao-selecionada', 'false');
      }
    });
    setListaMatrizSelecionda([]);
    setListaODSSelecionado([]);
    setDescricaoCiclo('');
    setPronto(false);
  }

  async function obterCicloExistente(ano, escolaId, cicloId) {
    resetListas();
    setCampoCicloDesabilitado(false);

    const ciclo = await api.get(
      `v1/planos/ciclo/${ano}/${cicloId}/${escolaId}`
    );

    setPlanoCicloId(ciclo.data.id);

    if (ciclo && ciclo.data) {
      const alteradoEm = moment(ciclo.data.alteradoEm).format(
        'DD/MM/YYYY HH:mm:ss'
      );
      const criadoEm = moment(ciclo.data.criadoEm).format(
        'DD/MM/YYYY HH:mm:ss'
      );
      setInseridoAlterado({
        alteradoEm,
        alteradoPor: ciclo.data.alteradoPor,
        criadoEm,
        criadoPor: ciclo.data.criadoPor,
      });
      configuraValoresPlanoCiclo(ciclo);
      setPronto(true);
    } else {
      setPronto(true);
    }
  }

  function configuraValoresPlanoCiclo(ciclo) {
    if (ciclo.data.idsMatrizesSaber && ciclo.data.idsMatrizesSaber.length) {
      ciclo.data.idsMatrizesSaber.forEach(id => {
        const matriz = document.querySelector(
          `#matriz-${id}:not([opcao-selecionada='true'])`
        );
        if (matriz) matriz.click();
      });
    }
    if (
      ciclo.data.idsObjetivosDesenvolvimentoSustentavel &&
      ciclo.data.idsObjetivosDesenvolvimentoSustentavel.length
    ) {
      ciclo.data.idsObjetivosDesenvolvimentoSustentavel.forEach(id => {
        const objetivo = document.querySelector(
          `#ods-${id}:not([opcao-selecionada='true'])`
        );
        if (objetivo) objetivo.click();
      });
    }
    setDescricaoCiclo(ciclo.data.descricao);
    setCicloSelecionado(String(ciclo.data.cicloId));
    if (ciclo.data.migrado) {
      setRegistroMigrado(ciclo.data.migrado);
    }
  }

  function addRemoverMatriz(event, matrizSelecionada) {
    const estaSelecionado =
      event.target.getAttribute('opcao-selecionada') === 'true';
    event.target.setAttribute(
      'opcao-selecionada',
      estaSelecionado ? 'false' : 'true'
    );

    if (estaSelecionado) {
      listaMatrizSelecionda = listaMatrizSelecionda.filter(
        item => item.id !== matrizSelecionada.id
      );
    } else {
      listaMatrizSelecionda.push(matrizSelecionada);
    }
    setListaMatrizSelecionda(listaMatrizSelecionda);
    if (pronto) {
      setModoEdicao(true);
    }
  }

  function addRemoverODS(event, odsSelecionado) {
    const estaSelecionado =
      event.target.getAttribute('opcao-selecionada') === 'true';
    event.target.setAttribute(
      'opcao-selecionada',
      estaSelecionado ? 'false' : 'true'
    );

    if (estaSelecionado) {
      listaODSSelecionado = listaODSSelecionado.filter(
        item => item.id !== odsSelecionado.id
      );
    } else {
      listaODSSelecionado.push(odsSelecionado);
    }
    setListaODSSelecionado(listaODSSelecionado);
    if (pronto) {
      setModoEdicao(true);
    }
  }

  function trocaCiclo(value) {
    const anoLetivo = String(turmaSelecionada.anoLetivo);
    const codEscola = String(turmaSelecionada.unidadeEscolar);
    obterCicloExistente(anoLetivo, codEscola, value);
    setCicloSelecionado(value);
    setModoEdicao(false);
    setInseridoAlterado({});
  }

  const onChangeTextEditor = value => {
    setDescricaoCiclo(value);

    if (pronto) {
      setModoEdicao(true);
    }
  };

  function irParaLinkExterno(link) {
    window.open(link, '_blank');
  }

  function validaMatrizSelecionada() {
    listaMatriz.forEach(item => {
      const jaSelecionado = listaMatrizSelecionda.find(
        matriz => matriz.id === item.id
      );
      if (jaSelecionado) {
        return true;
      }
      return false;
    });
  }

  function validaODSSelecionado() {
    listaODS.forEach(item => {
      const jaSelecionado = listaODSSelecionado.find(
        matriz => matriz.id === item.id
      );
      if (jaSelecionado) {
        return true;
      }
      return false;
    });
  }

  function confirmarCancelamento() {
    resetListas();
    setModoEdicao(false);
    let ciclo = '';
    if (eventoTrocarCiclo) {
      ciclo = cicloParaTrocar;
      setCicloSelecionado(ciclo);
    }
    const anoLetivo = String(turmaSelecionada.anoLetivo);
    const codEscola = String(turmaSelecionada.unidadeEscolar);
    obterCicloExistente(anoLetivo, codEscola, ciclo || cicloSelecionado);
  }

  function salvarPlanoCiclo(navegarParaPlanejamento) {
    let idsMatrizesSaber = [];
    let idsObjetivosDesenvolvimento = [];

    if (!registroMigrado) {
      if (!listaMatrizSelecionda.length) {
        erro('Selecione uma opção ou mais em Matriz de saberes');
        return;
      }

      if (!listaODSSelecionado.length) {
        erro(
          'Selecione uma opção ou mais em Objetivos de Desenvolvimento Sustentável'
        );
        return;
      }

      idsMatrizesSaber = listaMatrizSelecionda.map(matriz => matriz.id);
      idsObjetivosDesenvolvimento = listaODSSelecionado.map(ods => ods.id);
    }

    const anoLetivo = String(turmaSelecionada.anoLetivo);
    const codEscola = String(turmaSelecionada.unidadeEscolar);

    const textoReal = textEditorRef.current.state.value
      .replace(/<[^>]*>/g, '')
      .trim();

    if (!textoReal) {
      erro('A descrição deve ser informada');
      return;
    }
    const params = {
      ano: anoLetivo,
      cicloId: cicloSelecionado,
      descricao: textEditorRef.current.state.value,
      escolaId: codEscola,
      id: planoCicloId || 0,
      idsMatrizesSaber,
      idsObjetivosDesenvolvimento,
    };

    setCarregandoSalvar(true);

    api.post('v1/planos/ciclo', params).then(
      () => {
        setCarregandoSalvar(false);
        sucesso('Suas informações foram salvas com sucesso.');
        if (navegarParaPlanejamento) {
          history.push('/');
        } else {
          confirmarCancelamento();
        }
      },
      e => {
        setCarregandoSalvar(false);
        erro(`Erro: ${e.response.data.mensagens[0]}`);
      }
    );
  }

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmado) {
        salvarPlanoCiclo(true);
      } else {
        setModoEdicao(false);
        history.push('/');
      }
    } else {
      history.push('/');
    }
  };

  async function onClickCancelar() {
    const confirmou = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas.',
      'Deseja realmente cancelar as alterações?'
    );
    if (confirmou) {
      confirmarCancelamento();
    }
  }

  async function validaTrocaCiclo(value) {
    if (modoEdicao) {
      setCicloParaTrocar(value);
      const confirmou = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmou) {
        salvarPlanoCiclo(false);
      } else {
        trocaCiclo(value);
      }
      setEventoTrocarCiclo(true);
    } else {
      trocaCiclo(value);
    }
  }

  const podeAlterar = () => {
    return permissoesTela[tipoPermissao.podeAlterar];
  };

  const desabilitaCamposEdicao = () => {
    if (podeAlterar()) return !modoEdicao;
    return true;
  };

  const anoAtual = window.moment().format('YYYY');

  return (
    <>
      <div className="col-md-12">
        {usuario && turmaSelecionada.turma ? (
          ''
        ) : (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'plano-ciclo-selecione-turma',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-0"
          />
        )}
      </div>
      <div className="col-md-12 mt-1">
        <Titulo>
          {modalidadeEja ? 'Plano de Etapa' : 'Plano de Ciclo'}
          <TituloAno>
            {` / ${anoAtual} `}
            <i className="fas fa-retweet" />
          </TituloAno>
          {registroMigrado ? (
            <RegistroMigrado className="float-right">
              Registro Migrado
            </RegistroMigrado>
          ) : (
            ''
          )}
        </Titulo>
      </div>
      <Card>
        <div className="col-md-12">
          <div className="row mb-3">
            <div className="col-md-6">
              <div className="row">
                <div className="col-md-6">
                  <Loader
                    loading={turmaSelecionada.turma && carregandoCiclos}
                    tip=""
                  >
                    <SelectComponent
                      className="col-md-12"
                      name="tipo-ciclo"
                      id="tipo-ciclo"
                      placeHolder="Selecione um tipo de ciclo"
                      lista={listaCiclos}
                      disabled={
                        campoCicloDesabilitado ||
                        somenteConsulta ||
                        !podeAlterar()
                          ? true
                          : listaCiclos.length === 1
                      }
                      valueOption="id"
                      valueText="descricao"
                      onChange={validaTrocaCiclo}
                      valueSelect={cicloSelecionado}
                    />
                  </Loader>
                </div>
              </div>
            </div>
            <div className="col-md-6 d-flex justify-content-end">
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-3"
                onClick={onClickVoltar}
              />
              <Button
                label="Cancelar"
                color={Colors.Roxo}
                border
                bold
                className="mr-3"
                onClick={onClickCancelar}
                hidden={desabilitaCamposEdicao()}
              />
              <Loader loading={carregandoSalvar} tip="">
                <Button
                  label="Salvar"
                  color={Colors.Roxo}
                  border
                  bold
                  onClick={() => salvarPlanoCiclo(false)}
                  disabled={desabilitaCamposEdicao()}
                />
              </Loader>
            </div>
          </div>
          {usuario && turmaSelecionada.turma && (
            <Loader loading={carregando}>
              <div className="row mb-3">
                <div className="col-md-6">
                  Este é um espaço para construção coletiva. Considere os
                  diversos ritmos de aprendizagem para planejar e traçar o
                  percurso de cada
                  {modalidadeEja ? ' etapa' : ' ciclo'}.
                </div>
                <div className="col-md-6">
                  Considerando as especificações de cada
                  {modalidadeEja ? ' etapa ' : ' ciclo '} desta unidade escolar
                  e o currículo da cidade, <b>selecione</b> os itens da matriz
                  do saber e dos objetivos de desenvolvimento e sustentabilidade
                  que contemplam as propostas que planejaram:
                </div>
              </div>
              <div className="row mb-3">
                <div className="col-md-6">
                  <TextEditor
                    ref={textEditorRef}
                    id="textEditor"
                    height="500px"
                    maxHeight="calc(100vh)"
                    onBlur={onChangeTextEditor}
                    value={descricaoCiclo}
                    disabled={somenteConsulta}
                  />
                  <InseridoAlterado>
                    {inseridoAlterado.criadoPor && inseridoAlterado.criadoEm ? (
                      <p className="pt-2">
                        INSERIDO por {inseridoAlterado.criadoPor} em{' '}
                        {inseridoAlterado.criadoEm}
                      </p>
                    ) : (
                      ''
                    )}

                    {inseridoAlterado.alteradoPor &&
                    inseridoAlterado.alteradoEm ? (
                      <p>
                        ALTERADO por {inseridoAlterado.alteradoPor} em{' '}
                        {inseridoAlterado.alteradoEm}
                      </p>
                    ) : (
                      ''
                    )}
                  </InseridoAlterado>
                </div>
                <div className="col-md-6 btn-link-plano-ciclo">
                  <div className="col-md-12">
                    <div className="row mb-3">
                      <BtnLink
                        onClick={() => irParaLinkExterno(urlMatrizSaberes)}
                      >
                        Matriz de saberes
                        <i className="fas fa-share" />
                      </BtnLink>
                    </div>

                    <div className="row">
                      <ListaItens
                        className={
                          registroMigrado || somenteConsulta
                            ? 'desabilitar-elemento'
                            : ''
                        }
                      >
                        <ul>
                          {listaMatriz.map(item => {
                            return (
                              <li key={item.id} className="row">
                                <div className="col-md-12">
                                  <div className="row aling-center">
                                    <div className="col-md-2">
                                      <Badge
                                        id={`matriz-${item.id}`}
                                        className="btn-li-item btn-li-item-matriz"
                                        opcao-selecionada={
                                          validaMatrizSelecionada
                                        }
                                        onClick={e => addRemoverMatriz(e, item)}
                                      >
                                        {item.id}
                                      </Badge>
                                    </div>

                                    <div className="col-md-10 pl-3">
                                      {item.descricao}
                                    </div>
                                  </div>
                                </div>
                              </li>
                            );
                          })}
                        </ul>
                      </ListaItens>
                    </div>

                    <hr className="row mb-3 mt-3" />

                    <div className="row mb-3">
                      <BtnLink onClick={() => irParaLinkExterno(urlODS)}>
                        Objetivos de Desenvolvimento Sustentável
                        <i className="fas fa-share" />
                      </BtnLink>
                    </div>
                    <div className="row">
                      <ListaItens
                        className={
                          registroMigrado || somenteConsulta
                            ? 'desabilitar-elemento'
                            : ''
                        }
                      >
                        <ul>
                          {listaODS.map(item => {
                            return (
                              <li key={item.id} className="row">
                                <div className="col-md-12">
                                  <div className="row aling-center">
                                    <div className="col-md-2">
                                      <Badge
                                        id={`ods-${item.id}`}
                                        className="btn-li-item btn-li-item-ods"
                                        opcao-selecionada={validaODSSelecionado}
                                        onClick={e => addRemoverODS(e, item)}
                                      >
                                        {item.id}
                                      </Badge>
                                    </div>

                                    <div className="col-md-10 pl-3">
                                      {item.descricao}
                                    </div>
                                  </div>
                                </div>
                              </li>
                            );
                          })}
                        </ul>
                      </ListaItens>
                    </div>
                  </div>
                </div>
              </div>
            </Loader>
          )}
        </div>
      </Card>
    </>
  );
}
