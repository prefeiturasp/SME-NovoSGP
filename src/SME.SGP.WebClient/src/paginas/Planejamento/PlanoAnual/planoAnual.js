import React, { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import { Collapse } from 'antd';
import Row from '~/componentes/row';

import {
  Grid,
  Card,
  SelectComponent,
  Button,
  Colors,
  Loader,
} from '~/componentes';
import CopiarConteudo from './copiarConteudo';
import Alert from '~/componentes/alert';
import modalidade from '~/dtos/modalidade';
import {
  Titulo,
  TituloAno,
  Planejamento,
  ContainerBimestres,
} from './planoAnual.css';
import { RegistroMigrado } from '~/componentes-sgp/registro-migrado';
import servicoDisciplinas from '~/servicos/Paginas/ServicoDisciplina';
import { erros, sucesso, confirmar } from '~/servicos/alertas';
import servicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import Bimestre from './bimestre';
import history from '~/servicos/history';
import ModalErros from './componentes/ModalErros';

const { Panel } = Collapse;

const PlanoAnual = () => {
  const turmaSelecionada = useSelector(c => c.usuario.turmaSelecionada);
  const [possuiTurmaSelecionada, setPossuiTurmaSelecionada] = useState(false);
  const [ehEja, setEhEja] = useState(false);
  const [planoAnual, setPlanoAnual] = useState([]);
  const [registroMigrado, setRegistroMigrado] = useState(false);
  const [
    possuiTurmasDisponiveisParaCopia,
    setPossuiTurmasDisponiveisParaCopia,
  ] = useState(false);
  const [emEdicao, setEmEdicao] = useState(false);
  const [carregandoDados, setCarregandoDados] = useState(false);
  const [exibirCopiarConteudo, setExibirCopiarConteudo] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [listaBimestresPreenchidos, setListaBimestresPreenchidos] = useState(
    []
  );
  const [bimestreExpandido, setBimestreExpandido] = useState('');
  const [listaErros, setListaErros] = useState([[], [], [], []]);
  const [refsPainel] = useState([useRef(), useRef(), useRef(), useRef()]);

  const [
    listaDisciplinasPlanejamento,
    setListaDisciplinasPlanejamento,
  ] = useState([]);
  const [
    codigoDisciplinaSelecionada,
    setCodigoDisciplinaSelecionada,
  ] = useState(undefined);

  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [errosModal, setErrosModal] = useState([]);

  const onChangeDisciplinas = codigoDisciplina => {
    const disciplina = listaDisciplinas.find(
      c => c.codigoComponenteCurricular.toString() === codigoDisciplina
    );
    setDisciplinaSelecionada(disciplina);
    setCodigoDisciplinaSelecionada(codigoDisciplina);
  };

  const obterPlano = bimestre => {
    const indiceBimestreAlterado = planoAnual.findIndex(
      c => c.bimestre === bimestre
    );
    return planoAnual[indiceBimestreAlterado];
  };

  const onChangeBimestre = bimestre => {
    const plano = obterPlano(bimestre.bimestre);
    plano.descricao = bimestre.descricao;
    plano.objetivosAprendizagem = bimestre.objetivosAprendizagem;
    plano.alterado = true;
    setEmEdicao(true);
  };

  const selecionarObjetivo = (bimestre, objetivo) => {
    const plano = obterPlano(bimestre);
    const indiceObjetivo = plano.objetivosAprendizagem.findIndex(
      c => c.id === objetivo.id
    );
    if (indiceObjetivo > -1) {
      plano.objetivosAprendizagem.splice(indiceObjetivo, 1);
    } else {
      plano.objetivosAprendizagem.push(objetivo);
    }
    plano.alterado = true;
    setPlanoAnual([...planoAnual]);
    setEmEdicao(true);
  };

  const onChangeDescricaoObjetivo = (bimestre, descricao) => {
    const plano = obterPlano(bimestre);
    if (plano.descricao !== descricao) {
      setEmEdicao(true);
      plano.descricao = descricao;
      plano.alterado = true;
      setPlanoAnual([...planoAnual]);
    }
  };

  const validarBimestres = planos => {
    const err = [[], [], [], []];
    let possuiErro = false;
    if (planos && planos.length > 0) {
      planos.forEach(plano => {
        if (
          disciplinaSelecionada.possuiObjetivos &&
          (!plano.objetivosAprendizagem ||
            (!plano.objetivosAprendizagem.length > 0 && !ehEja))
        ) {
          possuiErro = true;
          err[plano.bimestre - 1].push(
            'Ao menos um objetivo de aprendizagem deve ser selecionado.'
          );
        }
        if (!plano.descricao) {
          possuiErro = true;
          err[plano.bimestre - 1].push(
            'A descrição do plano deve ser informada.'
          );
        }
      });
      setListaErros([...err]);
    }

    if (!possuiErro) {
      return null;
    }
    return err;
  };

  const limparErros = () => {
    const err = [[], [], [], []];
    setListaErros(err);
  };

  const cancelar = async () => {
    const confirmou = await confirmar(
      'Atenção',
      'Você não salvou as informações preenchidas',
      'Deseja realmente cancelar as alterações?'
    );
    if (confirmou) {
      limparErros();
      servicoPlanoAnual
        .obter(
          turmaSelecionada.anoLetivo,
          codigoDisciplinaSelecionada,
          turmaSelecionada.unidadeEscolar,
          turmaSelecionada.turma
        )
        .then(resposta => {
          setPlanoAnual(resposta.data);
          setEmEdicao(false);
        })
        .catch(e => erros(e));
    }
  };

  const abrirCopiarConteudo = async () => {
    setExibirCopiarConteudo(true);
  };

  const salvar = () => {
    const plano = {
      anoLetivo: turmaSelecionada.anoLetivo,
      bimestres: planoAnual.filter(c => c.alterado || c.obrigatorio),
      componenteCurricularEolId:
        disciplinaSelecionada.codigoComponenteCurricular,
      turmaId: turmaSelecionada.turma,
      escolaId: turmaSelecionada.unidadeEscolar,
    };

    const err = validarBimestres(plano.bimestres);
    if (!err || err.length === 0) {
      setCarregandoDados(true);
      servicoPlanoAnual
        .salvar(plano)
        .then(resp => {
          setPlanoAnual(resp.data.result);
          setCarregandoDados(false);
          sucesso('Registro salvo com sucesso.');
          setEmEdicao(false);
          setListaBimestresPreenchidos(
            planoAnual
              .filter(c => c.descricao && c.descricao.length > 0)
              .map(c => {
                return { nome: `${c.bimestre} º Bimestre`, valor: c.bimestre };
              })
          );
        })
        .catch(e => {
          setCarregandoDados(false);
          erros(e);
        });
    } else {
      const erro = err.findIndex(c => !!c.length > 0);
      if (erro > -1) {
        const refBimestre = refsPainel[erro];
        if (refBimestre && refBimestre.current) {
          if (erro + 1 !== bimestreExpandido) {
            setBimestreExpandido([erro + 1]);
          }
          window.scrollTo(0, refsPainel[erro].current.offsetTop);
        }
      }
    }
  };

  /**
   * define o bimestre expandido
   */
  useEffect(() => {
    if (planoAnual && planoAnual.length > 0 && !emEdicao) {
      const expandido = planoAnual.find(c => c.obrigatorio);
      if (expandido) setBimestreExpandido([expandido.bimestre]);
    }
  }, [planoAnual]);

  /**
   * expande o bimestre atual
   */
  useEffect(() => {
    if (bimestreExpandido) {
      const refBimestre = refsPainel[bimestreExpandido - 1];
      if (refBimestre && refBimestre.current) {
        setTimeout(() => {
          window.scrollTo(
            0,
            refsPainel[bimestreExpandido - 1].current.offsetTop
          );
        }, 500);
      }
    }
  }, [bimestreExpandido, refsPainel]);

  /**
   *carrega lista de disciplinas
   */
  useEffect(() => {
    if (turmaSelecionada.turma) {
      setEmEdicao(false);
      setCarregandoDados(true);
      servicoDisciplinas
        .obterDisciplinasPorTurma(turmaSelecionada.turma)
        .then(resposta => {
          setCarregandoDados(false);
          setListaDisciplinas(resposta.data);
          if (resposta.data.length === 1) {
            const disciplina = resposta.data[0];
            setDisciplinaSelecionada(disciplina);
            setCodigoDisciplinaSelecionada(
              String(disciplina.codigoComponenteCurricular)
            );
          }
        })
        .catch(e => {
          setCarregandoDados(false);
          erros(e);
        });
    }
  }, [turmaSelecionada.ano, turmaSelecionada.turma]);

  /**
   *carrega a lista de planos
   */
  useEffect(() => {
    if (codigoDisciplinaSelecionada) {
      setCarregandoDados(true);
      setPlanoAnual([]);
      servicoPlanoAnual
        .obter(
          turmaSelecionada.anoLetivo,
          codigoDisciplinaSelecionada,
          turmaSelecionada.unidadeEscolar,
          turmaSelecionada.turma
        )
        .then(resposta => {
          setCarregandoDados(false);
          limparErros();
          setPlanoAnual(resposta.data);
          const migrado = resposta.data.filter(c => c.migrado);
          setRegistroMigrado(migrado && migrado.length > 0);
          setEmEdicao(false);
          setListaBimestresPreenchidos(
            resposta.data
              .filter(c => c.descricao && c.descricao.length > 0)
              .map(c => {
                return { nome: `${c.bimestre} º Bimestre`, valor: c.bimestre };
              })
          );
        })
        .catch(e => {
          setCarregandoDados(false);
          setPlanoAnual([]);
          setEmEdicao(false);
          erros(e);
        });

      const turmaPrograma = !!(turmaSelecionada.ano === '0');
      setCarregandoDados(true);
      servicoDisciplinas
        .obterDisciplinasPlanejamento(
          codigoDisciplinaSelecionada,
          turmaSelecionada.turma,
          turmaPrograma,
          disciplinaSelecionada && disciplinaSelecionada.regencia
        )
        .then(resposta => {
          setCarregandoDados(false);
          setListaDisciplinasPlanejamento(
            resposta.data.map(disciplina => {
              return {
                ...disciplina,
                selecionada: false,
              };
            })
          );
        })
        .catch(e => {
          setCarregandoDados(false);
          setPlanoAnual([]);
          setEmEdicao(false);
          erros(e);
        });
    }
  }, [codigoDisciplinaSelecionada, disciplinaSelecionada, turmaSelecionada]);

  useEffect(() => {
    setPossuiTurmaSelecionada(turmaSelecionada && turmaSelecionada.turma);
    setEmEdicao(false);
    if (turmaSelecionada && turmaSelecionada.turma) {
      setEhEja(
        turmaSelecionada.modalidade.toString() === modalidade.EJA.toString()
      );
    }
  }, [turmaSelecionada]);

  useEffect(() => {
    const errosEscopo = [];
    listaErros.forEach((item, index) => {
      if (item.length > 0) {
        item.forEach(err => errosEscopo.push(`${index + 1}º Bimestre: ${err}`));
      }
    });
    setErrosModal(errosEscopo);
  }, [listaErros]);

  const fecharCopiarConteudo = () => {
    setExibirCopiarConteudo(false);
  };

  const onChangePossuiTurmasDisponiveisParaCopia = possuiTurmas => {
    setPossuiTurmasDisponiveisParaCopia(possuiTurmas);
  };

  return (
    <>
      <CopiarConteudo
        visivel={exibirCopiarConteudo}
        listaBimestresPreenchidos={listaBimestresPreenchidos}
        componenteCurricularEolId={codigoDisciplinaSelecionada}
        turmaId={turmaSelecionada.turma}
        onCloseCopiarConteudo={fecharCopiarConteudo}
        planoAnual={{
          anoLetivo: turmaSelecionada.anoLetivo,
          bimestres: planoAnual,
          componenteCurricularEolId:
            disciplinaSelecionada &&
            disciplinaSelecionada.codigoComponenteCurricular,
          turmaId: turmaSelecionada.turma,
          escolaId: turmaSelecionada.unidadeEscolar,
        }}
        onChangePossuiTurmasDisponiveisParaCopia={
          onChangePossuiTurmasDisponiveisParaCopia
        }
      />
      <Loader loading={carregandoDados}>
        <ModalErros
          visivel={errosModal.length > 0}
          erros={errosModal}
          onCloseErrosBimestre={() => setErrosModal([])}
        />
        <div className="col-md-12">
          {!possuiTurmaSelecionada ? (
            <Row className="mb-0 pb-0">
              <Grid cols={12} className="mb-0 pb-0">
                <Alert
                  alerta={{
                    tipo: 'warning',
                    id: 'plano-anual-selecione-turma',
                    mensagem: 'Você precisa escolher uma turma.',
                    estiloTitulo: { fontSize: '18px' },
                  }}
                  className="mb-0"
                />
              </Grid>
            </Row>
          ) : null}
        </div>
        <Grid cols={12} className="p-0">
          <Titulo>
            {ehEja ? 'Plano Semestral' : 'Plano Anual'}
            {registroMigrado && (
              <RegistroMigrado className="float-right">
                Registro Migrado
              </RegistroMigrado>
            )}
          </Titulo>
        </Grid>
        <Card className="col-md-12 p-0 float-right" mx="mx-0">
          <div className="col-md-4 col-xs-12">
            <SelectComponent
              name="disciplinas"
              id="disciplinas"
              lista={listaDisciplinas}
              valueOption="codigoComponenteCurricular"
              valueText="nome"
              onChange={onChangeDisciplinas}
              valueSelect={codigoDisciplinaSelecionada}
              placeholder="Selecione um componente curricular"
              disabled={listaDisciplinas && listaDisciplinas.length === 1}
            />
          </div>
          <div className="col-md-8 col-sm-2 d-flex justify-content-end">
            <Button
              label="Copiar Conteúdo"
              icon="share-square"
              color={Colors.Azul}
              className="mr-3"
              border
              onClick={abrirCopiarConteudo}
              disabled={emEdicao || !possuiTurmasDisponiveisParaCopia}
            />
            <Button
              label="Voltar"
              icon="arrow-left"
              color={Colors.Azul}
              border
              className="mr-3"
              onClick={() => history.push('/')}
            />
            <Button
              label="Cancelar"
              color={Colors.Roxo}
              border
              bold
              disabled={!emEdicao}
              className="mr-3"
              disabled={!emEdicao || !Object.entries(turmaSelecionada).length}
              onClick={cancelar}
            />
            <Button
              label="Salvar"
              color={Colors.Roxo}
              bold
              onClick={salvar}
              disabled={!emEdicao}
            />
          </div>
          <Grid cols={12} className="p-2">
            <ContainerBimestres>
              <Collapse
                bordered={false}
                expandIconPosition="right"
                defaultActiveKey={bimestreExpandido}
                activeKey={bimestreExpandido}
                onChange={c => {
                  setBimestreExpandido(c);
                }}
              >
                {planoAnual &&
                  planoAnual.length > 0 &&
                  planoAnual.map(plano => (
                    <Panel
                      header={`${plano.bimestre}º Bimestre`}
                      key={plano.bimestre}
                    >
                      <div ref={refsPainel[plano.bimestre - 1]}>
                        <Bimestre
                          className="fade"
                          disciplinas={listaDisciplinasPlanejamento}
                          bimestre={plano}
                          ano={turmaSelecionada.ano}
                          ehEja={ehEja}
                          ehMedio={
                            turmaSelecionada.modalidade.toString() ===
                            modalidade.ENSINO_MEDIO.toString()
                          }
                          disciplinaSemObjetivo={
                            !disciplinaSelecionada.possuiObjetivos
                          }
                          onChange={onChangeBimestre}
                          key={plano.bimestre}
                          erros={listaErros[plano.bimestre - 1]}
                          onCloseErrosBimestre={() => {
                            setListaErros(
                              listaErros.map((item, index) =>
                                plano.bimestre - 1 === index ? [] : item
                              )
                            );
                          }}
                          selecionarObjetivo={selecionarObjetivo}
                          onChangeDescricaoObjetivo={onChangeDescricaoObjetivo}
                        />
                      </div>
                    </Panel>
                  ))}
              </Collapse>
            </ContainerBimestres>
          </Grid>
        </Card>
      </Loader>
    </>
  );
};

export default PlanoAnual;
