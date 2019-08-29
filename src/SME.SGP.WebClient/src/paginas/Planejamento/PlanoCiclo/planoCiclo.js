import * as moment from 'moment';
import React, { useEffect, useState, useRef } from 'react';
import { useSelector } from 'react-redux';
import { BtnLink, ListaItens, Badge, Container, InseridoAlterado, } from './planoCiclo.css';
import TextEditor from '../../../componentes/textEditor';

import Alert from '../../../componentes/alert';
import Button from '../../../componentes/button';
import { Colors } from '../../../componentes/colors';
import SelectComponent from '../../../componentes/select';
import { erro, sucesso, confirmacao } from '../../../servicos/alertas';
// import ControleEstado from '../../../componentes/controleEstado';
import api from '../../../servicos/api';
import history from '../../../servicos/history';
import ModalConfirmacao from '../../../componentes/modalConfirmacao';

export default function PlanoCiclo(props) {
  const { match } = props;

  const urlPrefeitura = 'https://curriculo.prefeitura.sp.gov.br';
  const urlMatrizSaberes = `${urlPrefeitura}/matriz-de-saberes`;
  const urlODS = `${urlPrefeitura}/ods`;
  const textEditorRef = useRef(null);

  const [listaMatriz, setListaMatriz] = useState([]);
  const [listaODS, setListaODS] = useState([]);
  const [listaCiclos, setListaCiclos] = useState([]);
  const [cicloSelecionado, setCicloSelecionado] = useState('');
  const [descricaoCiclo, setDescricaoCiclo] = useState('');
  const [parametrosConsulta, setParametrosConsulta] = useState({ id: 0 });
  const [modoEdicao, setModoEdicao] = useState(false);
  const [pronto, setPronto] = useState(false);
  const [exibirConfirmacaoVoltar, setExibirConfirmacaoVoltar] = useState(false);
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

  useEffect(() => {
    function obterSugestaoCiclo() {
      // TODO - Setar o ciclo quando tiver uma sugestão!
      // setCicloSelecionado('2');
    }

    async function carregarListas() {
      const matrizes = await api.get('v1/matrizes-saber');
      setListaMatriz(matrizes.data);

      const ods = await api.get('v1/objetivos-desenvolvimento-sustentavel');
      setListaODS(ods.data);

      const ciclos = await api.get('v1/ciclos');
      setListaCiclos(ciclos.data);

      if (match.params && match.params.ano && match.params.escolaId) {
        obterCicloExistente(
          match.params.ano,
          match.params.escolaId,
          cicloSelecionado || '1'
        );
      }
    }

    carregarListas();
  }, []);

  async function obterCicloExistente(ano, escolaId, cicloId) {
    const ciclo = await api.get(
      `v1/planos-ciclo/${ano}/${cicloId}/${escolaId}`
    );
    setParametrosConsulta({
      id: ciclo.data.id,
      ano,
      cicloId,
      escolaId,
    });
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

  function setCiclo(value) {
    obterCicloExistente(
      parametrosConsulta.ano,
      parametrosConsulta.escolaId,
      value
    );
    resetListas();
    setCicloSelecionado(value);
    setModoEdicao(false);
    setInseridoAlterado({});
  }

  const onChangeTextEditor = (value) => {

    setDescricaoCiclo(value);
    
    if (pronto) {
      setModoEdicao(true);
    }
  }

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
    obterCicloExistente(
      parametrosConsulta.ano,
      parametrosConsulta.escolaId,
      cicloSelecionado
    );
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

    const params = {
      ano: parametrosConsulta.ano,
      cicloId: cicloSelecionado,
      descricao: textEditorRef.current.state.value,
      escolaId: parametrosConsulta.escolaId,
      id: parametrosConsulta.id || 0,
      idsMatrizesSaber: listaMatrizSelecionda.map(matriz => matriz.id),
      idsObjetivosDesenvolvimento: listaODSSelecionado.map(ods => ods.id),
    };

    api.post('v1/planos-ciclo', params).then(
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
    <Container>
      <ModalConfirmacao
        id="modal-confirmacao-cancelar"
        visivel={exibirConfirmacaoCancelar}
        onConfirmacaoSim={() => {
          confirmarCancelamento();
          setExibirConfirmacaoCancelar(false);
        }}
        onConfirmacaoNao={() => setExibirConfirmacaoCancelar(false)}
        conteudo="Você não salvou as informações preenchidas."
        perguntaDoConteudo="Deseja realmente cancelar as alterações?"
        titulo="Atenção"
      />
      <ModalConfirmacao
        id="modal-confirmacao-voltar"
        visivel={exibirConfirmacaoVoltar}
        onConfirmacaoSim={() => {
          salvarPlanoCiclo(true);
          setExibirConfirmacaoVoltar(false);
        }}
        onConfirmacaoNao={() => {
          setExibirConfirmacaoVoltar(false);
          setModoEdicao(false);
          history.push('/');
        }}
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
        {notificacoes.alertas.map(alerta => (
          <Alert alerta={alerta} key={alerta.id} />
        ))}
      </div>
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
                  onChange={setCiclo}
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
            Considerando as especificações de cada ciclo desta unidade escolar e
            o currículo da cidade, <b>selecione</b> os itens da matriz do saber
            e dos objetivos de desenvolvimento e sustentabilidade que contemplam
            as propostas que planejaram:
          </div>
        </div>

        <div className="row mb-3">
          <div className="col-md-6">
            <TextEditor ref={textEditorRef} id="textEditor" height="300px" maxHeight="calc(100vh)" onBlur={onChangeTextEditor} value={descricaoCiclo} />
            <InseridoAlterado>
              {inseridoAlterado.criadoPor && inseridoAlterado.criadoEm ? (
                <p>
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
                <ListaItens>
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
                <ListaItens>
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
    </Container>
  );
}
