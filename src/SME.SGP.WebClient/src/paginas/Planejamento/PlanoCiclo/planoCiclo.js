import * as moment from 'moment';
import React, { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';

import Alert from '../../../componentes/alert';
import Button from '../../../componentes/button';
import Card from '../../../componentes/card';
import { Colors } from '../../../componentes/colors';
import ModalConfirmacao from '../../../componentes/modalConfirmacao';
import SelectComponent from '../../../componentes/select';
import TextEditor from '../../../componentes/textEditor';
import { erro, sucesso } from '../../../servicos/alertas';
import api from '../../../servicos/api';
import history from '../../../servicos/history';
import {
  Badge,
  BtnLink,
  InseridoAlterado,
  ListaItens,
  Planejamento,
  Titulo,
  TituloAno,
} from './planoCiclo.css';

// import ControleEstado from '../../../componentes/controleEstado';
export default function PlanoCiclo() {
  const urlPrefeitura = 'https://curriculo.sme.prefeitura.sp.gov.br';
  const urlMatrizSaberes = `${urlPrefeitura}/matriz-de-saberes`;
  const urlODS = `${urlPrefeitura}/ods`;
  const textEditorRef = useRef(null);

  const [listaMatriz, setListaMatriz] = useState([]);
  const [listaODS, setListaODS] = useState([]);
  const [listaCiclos, setListaCiclos] = useState([]);
  const [cicloSelecionado, setCicloSelecionado] = useState('');
  const [descricaoCiclo, setDescricaoCiclo] = useState('');
  const [modoEdicao, setModoEdicao] = useState(false);
  const [pronto, setPronto] = useState(false);
  const [exibirConfirmacaoVoltar, setExibirConfirmacaoVoltar] = useState(false);
  const [
    exibirConfirmacaoTrocaCiclo,
    setExibirConfirmacaoTrocaCiclo,
  ] = useState(false);
  const [eventoTrocarCiclo, setEventoTrocarCiclo] = useState(false);
  const [registroMigrado, setRegistroMigrado] = useState(false);
  const [cicloParaTrocar, setCicloParaTrocar] = useState('');
  const [exibirConfirmacaoCancelar, setExibirConfirmacaoCancelar] = useState(
    false
  );
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

  const usuario = useSelector(store => store.usuario);

  useEffect(() => {
    async function carregarListas() {
      const matrizes = await api.get('v1/matrizes-saber');
      setListaMatriz(matrizes.data);

      const ods = await api.get('v1/objetivos-desenvolvimento-sustentavel');
      setListaODS(ods.data);
    }

    carregarListas();
  }, []);

  useEffect(() => {
    let anosTurmasUsuario = usuario.turmasUsuario.map(item => item.ano);
    anosTurmasUsuario = anosTurmasUsuario.filter(
      (elem, pos) => anosTurmasUsuario.indexOf(elem) == pos
    );
    setAnosTurmasUsuario(anosTurmasUsuario);
  }, [usuario.turmasUsuario]);

  useEffect(() => {
    async function carregarCiclos() {
      if (
        usuario &&
        usuario.turmaSelecionada &&
        usuario.turmaSelecionada.length
      ) {
        let anoSelecionado = '';
        let codModalidade = null;
        if (usuario.turmaSelecionada && usuario.turmaSelecionada.length) {
          anoSelecionado = String(usuario.turmaSelecionada[0].ano);
          codModalidade = usuario.turmaSelecionada[0].codModalidade;
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
          params['anos'] = anos;
        } else {
          params['anos'] = anosTurmasUsuario;
        }

        const ciclos = await api.post('v1/ciclos/filtro', params);

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

        const anoLetivo = String(usuario.turmaSelecionada[0].anoLetivo);
        const codEscola = String(usuario.turmaSelecionada[0].codEscola);

        if (usuario.turmaSelecionada.codModalidade == 3) {
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
    }

    carregarCiclos();
  }, [usuario.turmaSelecionada]);

  async function obterCicloExistente(ano, escolaId, cicloId) {
    resetListas();
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
        document.getElementById(`matriz-${id}`).click();
      });
    }
    if (
      ciclo.data.idsObjetivosDesenvolvimentoSustentavel &&
      ciclo.data.idsObjetivosDesenvolvimentoSustentavel.length
    ) {
      ciclo.data.idsObjetivosDesenvolvimentoSustentavel.forEach(id => {
        document.getElementById(`ods-${id}`).click();
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

  function validaTrocaCiclo(value) {
    if (modoEdicao) {
      setCicloParaTrocar(value);
      setExibirConfirmacaoTrocaCiclo(true);
      setEventoTrocarCiclo(true);
    } else {
      trocaCiclo(value);
    }
  }

  function trocaCiclo(value) {
    const anoLetivo = String(usuario.turmaSelecionada[0].anoLetivo);
    const codEscola = String(usuario.turmaSelecionada[0].codEscola);
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

  function onClickVoltar() {
    if (modoEdicao) {
      setExibirConfirmacaoVoltar(true);
    } else {
      history.push('/');
    }
  }

  function onClickCancelar() {
    setExibirConfirmacaoCancelar(true);
  }

  function confirmarCancelamento() {
    resetListas();
    setModoEdicao(false);
    let ciclo = '';
    if (eventoTrocarCiclo) {
      ciclo = cicloParaTrocar;
      setCicloSelecionado(ciclo);
    }
    const anoLetivo = String(usuario.turmaSelecionada[0].anoLetivo);
    const codEscola = String(usuario.turmaSelecionada[0].codEscola);
    obterCicloExistente(anoLetivo, codEscola, ciclo || cicloSelecionado);
  }

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

    const anoLetivo = String(usuario.turmaSelecionada[0].anoLetivo);
    const codEscola = String(usuario.turmaSelecionada[0].codEscola);
    const params = {
      ano: anoLetivo,
      cicloId: cicloSelecionado,
      descricao: textEditorRef.current.state.value,
      escolaId: codEscola,
      id: planoCicloId || 0,
      idsMatrizesSaber,
      idsObjetivosDesenvolvimento,
    };

    api.post('v1/planos/ciclo', params).then(
      () => {
        sucesso('Suas informações foram salvas com sucesso.');
        if (navegarParaPlanejamento) {
          history.push('/');
        } else {
          confirmarCancelamento();
        }
      },
      e => {
        erro(`Erro: ${e.response.data.mensagens[0]}`);
      }
    );
  }

  // TODO quanto tivermos a tela de login e a home, deverá ser movido todos os alertas para a home/container
  const notificacoes = useSelector(state => state.notificacoes);

  return (
    <>
      <div className="col-md-12">
        {notificacoes.alertas.map(alerta => (
          <Alert alerta={alerta} key={alerta.id} />
        ))}

        {usuario &&
        usuario.turmaSelecionada &&
        usuario.turmaSelecionada.length ? (
          ''
        ) : (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'plano-ciclo-selecione-turma',
              mensagem: 'Você precisa escolher uma turma.',
            }}
            className="mb-0"
          />
        )}
      </div>
      <div className="col-md-12">
        <Planejamento>PLANEJAMENTO</Planejamento>
        <Titulo>
          {modalidadeEja ? 'Plano de Etapa' : 'Plano de Ciclo'}
          <TituloAno>
            /2019 <i className="fas fa-retweet" />
          </TituloAno>
        </Titulo>
      </div>

      <Card>
        <div className="col-md-12 pb-3">
          {registroMigrado ? <span> REGISTRO MIGRADO </span> : ''}
        </div>
        <ModalConfirmacao
          id="modal-confirmacao-cancelar"
          visivel={exibirConfirmacaoCancelar}
          onConfirmacaoSecundaria={() => {
            confirmarCancelamento();
            setExibirConfirmacaoCancelar(false);
          }}
          onConfirmacaoPrincipal={() => setExibirConfirmacaoCancelar(false)}
          onClose={() => setExibirConfirmacaoCancelar(false)}
          conteudo="Você não salvou as informações preenchidas."
          perguntaDoConteudo="Deseja realmente cancelar as alterações?"
          labelPrincipal="Não"
          labelSecundaria="Sim"
          titulo="Atenção"
        />
        <ModalConfirmacao
          id="modal-confirmacao-voltar"
          visivel={exibirConfirmacaoVoltar}
          onConfirmacaoPrincipal={() => {
            salvarPlanoCiclo(true);
            setExibirConfirmacaoVoltar(false);
          }}
          onConfirmacaoSecundaria={() => {
            setExibirConfirmacaoVoltar(false);
            setModoEdicao(false);
            history.push('/');
          }}
          onClose={() => {
            setExibirConfirmacaoVoltar(false);
            setModoEdicao(false);
            history.push('/');
          }}
          labelPrincipal="Sim"
          labelSecundaria="Não"
          perguntaDoConteudo="Suas alterações não foram salvas, deseja salvar agora?"
          titulo="Atenção"
        />
        <ModalConfirmacao
          id="modal-confirmacao-troca-ciclo"
          visivel={exibirConfirmacaoTrocaCiclo}
          onConfirmacaoPrincipal={() => {
            salvarPlanoCiclo(false);
            setExibirConfirmacaoTrocaCiclo(false);
          }}
          onConfirmacaoSecundaria={() => {
            setExibirConfirmacaoTrocaCiclo(false);
            trocaCiclo(cicloParaTrocar);
          }}
          onClose={() => {
            setExibirConfirmacaoTrocaCiclo(false);
            trocaCiclo(cicloParaTrocar);
          }}
          labelPrincipal="Sim"
          labelSecundaria="Não"
          perguntaDoConteudo="Suas alterações não foram salvas, deseja salvar agora?"
          titulo="Atenção"
        />
        {/* <ControleEstado
        when={modoEdicao}
        confirmar={url => history.push(url)}
        cancelar={() => false}
        bloquearNavegacao={() => modoEdicao}
      /> */}
        <div className="col-md-12">
          <div className="row mb-3">
            <div className="col-md-6">
              <div className="row">
                <div className="col-md-6">
                  <SelectComponent
                    className="col-md-12"
                    name="tipo-ciclo"
                    id="tipo-ciclo"
                    lista={listaCiclos}
                    valueOption="id"
                    label="descricao"
                    onChange={validaTrocaCiclo}
                    valueSelect={cicloSelecionado}
                  />
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
                hidden={!modoEdicao}
              />
              <Button
                label="Salvar"
                color={Colors.Roxo}
                border
                bold
                onClick={() => salvarPlanoCiclo(false)}
                disabled={!modoEdicao}
              />
            </div>
          </div>

          <div className="row mb-3">
            <div className="col-md-6">
              Este é um espaço para construção coletiva. Considere os diversos
              ritmos de aprendizagem para planejar e traçar o percurso de cada
              ciclo.
            </div>
            <div className="col-md-6">
              Considerando as especificações de cada{' '}
              {modalidadeEja ? 'etapa' : 'ciclo'} desta unidade escolar e o
              currículo da cidade, <b>selecione</b> os itens da matriz do saber
              e dos objetivos de desenvolvimento e sustentabilidade que
              contemplam as propostas que planejaram:
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

                {inseridoAlterado.alteradoPor && inseridoAlterado.alteradoEm ? (
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
                  <BtnLink onClick={() => irParaLinkExterno(urlMatrizSaberes)}>
                    Matriz de saberes
                    <i className="fas fa-share" />
                  </BtnLink>
                </div>

                <div className="row">
                  <ListaItens
                    className={registroMigrado ? 'desabilitar-elemento' : ''}
                  >
                    <ul>
                      {listaMatriz.map(item => {
                        return (
                          <li key={item.id}>
                            {
                              <Badge
                                id={`matriz-${item.id}`}
                                className="btn-li-item btn-li-item-matriz"
                                opcao-selecionada={validaMatrizSelecionada}
                                onClick={e => addRemoverMatriz(e, item)}
                              >
                                {item.id}
                              </Badge>
                            }
                            {item.descricao}
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
                    className={registroMigrado ? 'desabilitar-elemento' : ''}
                  >
                    <ul>
                      {listaODS.map(item => {
                        return (
                          <li key={item.id}>
                            {
                              <Badge
                                id={`ods-${item.id}`}
                                className="btn-li-item btn-li-item-ods"
                                opcao-selecionada={validaODSSelecionado}
                                onClick={e => addRemoverODS(e, item)}
                              >
                                {item.id}
                              </Badge>
                            }
                            {item.descricao}
                          </li>
                        );
                      })}
                    </ul>
                  </ListaItens>
                </div>
              </div>
            </div>
          </div>
        </div>
      </Card>
    </>
  );
}
