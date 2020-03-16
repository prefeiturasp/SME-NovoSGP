import React, { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import * as Yup from 'yup';
import { Formik, Form } from 'formik';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';
import RadioGroupButton from '~/componentes/radioGroupButton';
import CampoTexto from '~/componentes/campoTexto';
import SelectComponent from '~/componentes/select';
import { Colors, Label } from '~/componentes';
import history from '~/servicos/history';
import TextEditor from '~/componentes/textEditor';
import { Div, Titulo, Badge, InseridoAlterado } from './avaliacao.css';
import RotasDTO from '~/dtos/rotasDto';
import ServicoAvaliacao from '~/servicos/Paginas/Calendario/ServicoAvaliacao';
import { erro, sucesso, confirmar } from '~/servicos/alertas';
import ModalCopiarAvaliacao from './componentes/ModalCopiarAvaliacao';

const AvaliacaoForm = ({ match }) => {
  const [
    mostrarModalCopiarAvaliacao,
    setMostrarModalCopiarAvaliacao,
  ] = useState(false);
  const permissaoTela = useSelector(
    state => state.usuario.permissoes[RotasDTO.CADASTRO_DE_AVALIACAO]
  );

  const botaoCadastrarRef = useRef(null);
  const [refForm, setRefForm] = useState({});

  const [modoEdicao, setModoEdicao] = useState(false);

  const clicouBotaoVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmado) {
        if (botaoCadastrarRef.current) botaoCadastrarRef.current.click();
      } else {
        history.push(RotasDTO.CALENDARIO_PROFESSOR);
      }
    } else {
      history.push(RotasDTO.CALENDARIO_PROFESSOR);
    }
  };

  const [idAvaliacao, setIdAvaliacao] = useState('');
  const [inseridoAlterado, setInseridoAlterado] = useState({
    alteradoEm: '',
    alteradoPor: '',
    criadoEm: '',
    criadoPor: '',
  });

  const aoTrocarCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  const clicouBotaoExcluir = async () => {
    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir este registro?'
    );
    if (confirmado) {
      const exclusao = await ServicoAvaliacao.excluir(idAvaliacao);
      if (exclusao && exclusao.status === 200) {
        history.push(RotasDTO.CALENDARIO_PROFESSOR);
      } else {
        erro(exclusao);
      }
    }
  };

  const clicouBotaoCadastrar = (form, e) => {
    e.persist();
    form.validateForm().then(() => form.handleSubmit(e));
  };

  const eventoAulaCalendarioEdicao = useSelector(
    store => store.calendarioProfessor.eventoAulaCalendarioEdicao
  );

  const diaAvaliacao = useSelector(
    store => store.calendarioProfessor.diaSelecionado
  );

  const [descricao, setDescricao] = useState('');
  const [copias, setCopias] = useState([]);
  const [listaDisciplinasRegencia, setListaDisciplinasRegencia] = useState([]);
  const [
    listaDisciplinasSelecionadas,
    setListaDisciplinasSelecionadas,
  ] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarCopiarAvaliacao, setDesabilitarCopiarAvaliacao] = useState(
    false
  );

  const usuario = useSelector(store => store.usuario);

  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  const cadastrarAvaliacao = async dados => {
    const avaliacao = {};

    if (Object.entries(eventoAulaCalendarioEdicao).length) {
      avaliacao.dreId = eventoAulaCalendarioEdicao.dre;
      avaliacao.turmaId = eventoAulaCalendarioEdicao.turma;
      avaliacao.ueId = eventoAulaCalendarioEdicao.unidadeEscolar;
    } else if (Object.entries(turmaSelecionada).length) {
      avaliacao.dreId = turmaSelecionada.dre;
      avaliacao.turmaId = turmaSelecionada.turma;
      avaliacao.ueId = turmaSelecionada.unidadeEscolar;
    }

    const disciplinas = [];
    listaDisciplinasRegencia.forEach(disciplina => {
      if (
        !disciplinas.includes(disciplina.codigoComponenteCurricular) &&
        disciplina.selecionada
      )
        disciplinas.push(`${disciplina.codigoComponenteCurricular}`);
    });
    avaliacao.disciplinaContidaRegenciaId = disciplinas;

    avaliacao.dataAvaliacao = window.moment(diaAvaliacao).format();
    avaliacao.descricao = descricao;

    dados.disciplinasId = Array.isArray(dados.disciplinasId)
      ? [...dados.disciplinasId]
      : [dados.disciplinasId];

    const dadosValidacao = {
      ...dados,
      ...avaliacao,
      turmasParaCopiar: copias.map(z => ({
        turmaId: z.turmaId,
        dataAtividadeAvaliativa: z.dataAvaliacao,
      })),
    };

    delete dadosValidacao.categoriaId;
    delete dadosValidacao.descricao;

    if (descricao.length <= 500) {
      const validacao = await ServicoAvaliacao.validar(dadosValidacao);

      if (validacao && validacao.status === 200) {
        const salvar = await ServicoAvaliacao.salvar(idAvaliacao, {
          ...dados,
          ...avaliacao,
          turmasParaCopiar: copias.map(z => ({
            turmaId: z.turmaId,
            dataAtividadeAvaliativa: z.dataAvaliacao,
          })),
        });

        if (salvar && salvar.status === 200) {
          if (salvar.data && salvar.data.length) {
            salvar.data.forEach(item => {
              if (item.mensagem.includes('Erro')) {
                erro(item.mensagem);
              } else {
                sucesso(item.mensagem);
              }
            });
          } else {
            sucesso(
              `Avaliação ${
                idAvaliacao ? 'atualizada' : 'cadastrada'
              } com sucesso.`
            );
          }
          history.push(RotasDTO.CALENDARIO_PROFESSOR);
        } else {
          erro(salvar);
        }
      } else {
        erro(validacao);
      }
    } else erro('A descrição não deve ter mais de 500 caracteres');
  };

  const categorias = { NORMAL: 1, INTERDISCIPLINAR: 2 };

  const montaValidacoes = categoria => {
    const ehInterdisciplinar = categoria === categorias.INTERDISCIPLINAR;
    const val = {
      categoriaId: Yup.string().required('Selecione a categoria'),
      disciplinasId: Yup.string()
        .required('Selecione o componente curricular')
        .test({
          name: 'quantidadeDisciplinas',
          exclusive: true,
          message:
            'Para categoria Interdisciplinar informe mais que um componente curricular',
          test: value => (ehInterdisciplinar ? value.length > 1 : true),
        }),
      tipoAvaliacaoId: Yup.string().required(
        'Selecione o tipo de atividade avaliativa'
      ),
      nome: Yup.string().required('Preencha o nome da atividade avaliativa'),
      descricao: Yup.string().max(
        500,
        'A descrição não deve ter mais de 500 caracteres'
      ),
    };
    setValidacoes(Yup.object(val));
  };

  const [validacoes, setValidacoes] = useState(undefined);

  const [dataAvaliacao, setdataAvaliacao] = useState();

  const [listaDisciplinas, setListaDisciplinas] = useState([]);

  const [listaCategorias, setListaCategorias] = useState([
    { label: 'Normal', value: categorias.NORMAL },
    {
      label: 'Interdisciplinar',
      value: categorias.INTERDISCIPLINAR,
      disabled: true,
    },
  ]);

  const campoNomeRef = useRef(null);
  const textEditorRef = useRef(null);

  const aoTrocarTextEditor = valor => {
    setDescricao(valor);
    aoTrocarCampos();
  };

  const [dadosAvaliacao, setDadosAvaliacao] = useState();
  const inicial = {
    categoriaId: 1,
    disciplinasId: undefined,
    disciplinaContidaRegenciaId: [],
    nome: '',
    tipoAvaliacaoId: undefined,
  };

  const clicouBotaoCancelar = form => {
    if (!idAvaliacao) {
      form.resetForm();
      setDadosAvaliacao(inicial);
      aoTrocarTextEditor('');
    }
  };

  const obterDisciplinas = async () => {
    const disciplinas = await ServicoAvaliacao.listarDisciplinas(
      usuario.rf,
      turmaId
    );
    if (disciplinas.data) {
      const dadosDsiciplina = disciplinas.data;
      await setListaDisciplinas(dadosDsiciplina);
      if (dadosDsiciplina.length > 1) {
        listaCategorias.map(categoria => {
          if (categoria.value === categorias.INTERDISCIPLINAR) {
            categoria.disabled = false;
          }
        });
        setListaCategorias([...listaCategorias]);
      }
    }
  };

  const [disciplinaDesabilitada, setDisciplinaDesabilitada] = useState(false);
  const [temRegencia, setTemRegencia] = useState(false);

  const obterDisciplinasRegencia = async () => {
    const disciplinasRegencia = await ServicoAvaliacao.listarDisciplinasRegencia(
      turmaId
    );
    if (disciplinasRegencia.data) {
      setListaDisciplinasRegencia(disciplinasRegencia.data);
      setTemRegencia(true);
    }
  };

  useEffect(() => {
    if (!idAvaliacao && listaDisciplinas.length === 1) {
      if (listaDisciplinas[0].regencia) {
        setTemRegencia(true);
        obterDisciplinasRegencia();
      }
      setDadosAvaliacao({
        ...dadosAvaliacao,
        disciplinasId: listaDisciplinas[0].codigoComponenteCurricular.toString(),
      });
      setDisciplinaDesabilitada(true);
      setDisciplinaSelecionada(listaDisciplinas[0].codigoComponenteCurricular);
    }
  }, [listaDisciplinas]);

  const [listaTiposAvaliacao, setListaTiposAvaliacao] = useState([]);

  const obterlistaTiposAvaliacao = async () => {
    const tipos = await ServicoAvaliacao.listarTipos();
    if (tipos.data && tipos.data.items) {
      const lista = [];
      tipos.data.items.forEach(tipo => {
        lista.push({ nome: tipo.nome, id: tipo.id });
      });
      setListaTiposAvaliacao(lista);
    }
  };

  useEffect(() => {
    setdataAvaliacao(window.moment(diaAvaliacao));
    montaValidacoes(categorias.NORMAL);
    obterDisciplinas();
    obterlistaTiposAvaliacao();

    if (!idAvaliacao) setDadosAvaliacao(inicial);

    if (match && match.params && match.params.id)
      setIdAvaliacao(match.params.id);
  }, []);

  const validaInterdisciplinar = categoriaSelecionada => {
    if (categoriaSelecionada == categorias.INTERDISCIPLINAR) {
      setCopias([]);
      setDesabilitarCopiarAvaliacao(true);
    } else {
      setDesabilitarCopiarAvaliacao(false);
    }
  };

  const obterAvaliacao = async () => {
    const avaliacao = await ServicoAvaliacao.buscar(idAvaliacao);
    if (avaliacao && avaliacao.data) {
      setListaDisciplinasSelecionadas(avaliacao.data.disciplinasId);
      setDisciplinaSelecionada(avaliacao.data.disciplinasId[0]);
      validaInterdisciplinar(avaliacao.data.categoriaId);
      const tipoAvaliacaoId = avaliacao.data.tipoAvaliacaoId.toString();
      setDadosAvaliacao({ ...avaliacao.data, tipoAvaliacaoId });
      setDescricao(avaliacao.data.descricao);
      setInseridoAlterado({
        alteradoEm: avaliacao.data.alteradoEm,
        alteradoPor: `${avaliacao.data.alteradoPor} (${avaliacao.data.alteradoRF})`,
        criadoEm: avaliacao.data.criadoEm,
        criadoPor: `${avaliacao.data.criadoPor} (${avaliacao.data.criadoRF})`,
      });
      if (
        avaliacao.data.atividadesRegencia &&
        avaliacao.data.atividadesRegencia.length > 0
      ) {
        obterDisciplinasRegencia();
      }
    }
  };

  useEffect(() => {
    if (idAvaliacao) obterAvaliacao();
  }, [idAvaliacao]);

  const selecionarDisciplina = indice => {
    const disciplinas = [...listaDisciplinasRegencia];
    disciplinas[indice].selecionada = !disciplinas[indice].selecionada;
    setListaDisciplinasRegencia(disciplinas);
    aoTrocarCampos();
  };

  useEffect(() => {
    if (
      temRegencia &&
      listaDisciplinasRegencia &&
      listaDisciplinasRegencia.length > 0 &&
      dadosAvaliacao &&
      dadosAvaliacao.atividadesRegencia &&
      dadosAvaliacao.atividadesRegencia.length > 0
    ) {
      const disciplinas = [...listaDisciplinasRegencia];
      listaDisciplinasRegencia.forEach((item, indice) => {
        const disciplina = dadosAvaliacao.atividadesRegencia.filter(
          atividade => {
            return (
              atividade.disciplinaContidaRegenciaId ===
              item.codigoComponenteCurricular.toString()
            );
          }
        );
        if (disciplina && disciplina.length)
          disciplinas[indice].selecionada = true;
      });
      setListaDisciplinasRegencia(disciplinas);
    }
  }, [temRegencia]);

  const resetDisciplinasSelecionadas = form => {
    setListaDisciplinasSelecionadas([]);
    form.values.disciplinasId = [];
  };

  return (
    <Div className="col-12">
      {mostrarModalCopiarAvaliacao ? (
        <ModalCopiarAvaliacao
          show={mostrarModalCopiarAvaliacao}
          onClose={() => setMostrarModalCopiarAvaliacao(false)}
          disciplina={disciplinaSelecionada}
          onSalvarCopias={copiasAvaliacoes => {
            setCopias(copiasAvaliacoes);
            setModoEdicao(true);
          }}
        />
      ) : (
        ''
      )}
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">
          {`Cadastro de avaliação - ${
            dataAvaliacao ? dataAvaliacao.format('dddd') : ''
          }, ${dataAvaliacao ? dataAvaliacao.format('DD/MM/YYYY') : ''} `}
        </Titulo>
      </Grid>
      <Formik
        enableReinitialize
        ref={refForm => setRefForm(refForm)}
        initialValues={dadosAvaliacao}
        onSubmit={dados => cadastrarAvaliacao(dados)}
        validationSchema={validacoes}
        validateOnBlur={false}
        validateOnChange={false}
      >
        {form => (
          <Card className="rounded mb-4 mx-auto">
            <Grid cols={12} className="d-flex justify-content-end mb-3">
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                onClick={clicouBotaoVoltar}
                border
                className="mr-3"
              />
              <Button
                label="Cancelar"
                color={Colors.Roxo}
                onClick={() => clicouBotaoCancelar(form)}
                border
                bold
                className="mr-3"
                disabled={!modoEdicao}
              />
              <Button
                label="Excluir"
                color={Colors.Vermelho}
                border
                className="mr-3"
                disabled={
                  !idAvaliacao || (permissaoTela && !permissaoTela.podeAlterar)
                }
                onClick={clicouBotaoExcluir}
              />
              <Button
                label={idAvaliacao ? 'Alterar' : 'Cadastrar'}
                color={Colors.Roxo}
                onClick={e => clicouBotaoCadastrar(form, e)}
                ref={botaoCadastrarRef}
                disabled={
                  (permissaoTela &&
                    (!permissaoTela.podeIncluir ||
                      !permissaoTela.podeAlterar)) ||
                  !modoEdicao
                }
                border
                bold
              />
            </Grid>
            <Form>
              <Div className="row">
                <Grid cols={12} className="mb-4">
                  <RadioGroupButton
                    id="categoriaId"
                    name="categoriaId"
                    label="Categoria"
                    opcoes={listaCategorias}
                    form={form}
                    onChange={e => {
                      aoTrocarCampos();
                      resetDisciplinasSelecionadas(form);
                      montaValidacoes(e.target.value);
                      validaInterdisciplinar(e.target.value);
                    }}
                  />
                </Grid>
              </Div>
              {temRegencia && listaDisciplinasRegencia && (
                <Div className="row">
                  <Grid cols={12} className="mb-4">
                    <Label text="Componente curricular" />
                    {listaDisciplinasRegencia.map((disciplina, indice) => {
                      return (
                        <Badge
                          key={disciplina.codigoComponenteCurricular}
                          role="button"
                          onClick={e => {
                            e.preventDefault();
                            selecionarDisciplina(indice);
                          }}
                          aria-pressed={disciplina.selecionada && true}
                          alt={disciplina.nome}
                          className="badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mr-2"
                        >
                          {disciplina.nome}
                        </Badge>
                      );
                    })}
                  </Grid>
                </Div>
              )}
              <Div className="row">
                {!temRegencia && (
                  <Grid cols={4} className="mb-4">
                    {listaDisciplinas.length > 1 &&
                    form.values.categoriaId === categorias.INTERDISCIPLINAR ? (
                      <SelectComponent
                        id="disciplinasId"
                        name="disciplinasId"
                        label="Componente curricular"
                        lista={listaDisciplinas}
                        valueOption="codigoComponenteCurricular"
                        valueText="nome"
                        disabled={disciplinaDesabilitada}
                        placeholder="Selecione um componente curricular"
                        valueSelect={listaDisciplinasSelecionadas}
                        form={form}
                        multiple
                        onChange={aoTrocarCampos}
                      />
                    ) : (
                      <SelectComponent
                        id="disciplinasId"
                        name="disciplinasId"
                        label="Componente curricular"
                        lista={listaDisciplinas}
                        valueOption="codigoComponenteCurricular"
                        valueText="nome"
                        disabled={disciplinaDesabilitada}
                        placeholder="Selecione um componente curricular"
                        form={form}
                        onChange={valor => {
                          setDisciplinaSelecionada(valor);
                          aoTrocarCampos();
                        }}
                        valueSelect={disciplinaSelecionada}
                      />
                    )}
                  </Grid>
                )}
                <Grid cols={!temRegencia ? 4 : 6} className="mb-4">
                  <SelectComponent
                    id="tipoAvaliacaoId"
                    name="tipoAvaliacaoId"
                    label="Tipo de Atividade Avaliativa"
                    lista={listaTiposAvaliacao}
                    valueOption="id"
                    valueText="nome"
                    placeholder="Atividade Avaliativa"
                    form={form}
                    onChange={aoTrocarCampos}
                  />
                </Grid>
                <Grid cols={!temRegencia ? 4 : 6} className="mb-4">
                  <Label text="Nome da Atividade Avaliativa" />
                  <CampoTexto
                    name="nome"
                    id="nome"
                    maxlength={50}
                    placeholder="Nome"
                    type="input"
                    form={form}
                    ref={campoNomeRef}
                    onChange={e => {
                      form.setFieldValue('nome', e.target.value);
                      aoTrocarCampos();
                    }}
                  />
                </Grid>
              </Div>
              <Div className="row">
                <Grid cols={12}>
                  <Label text="Descrição" />
                  <TextEditor
                    ref={textEditorRef}
                    name="descricao"
                    id="descricao"
                    onBlur={aoTrocarTextEditor}
                    value={descricao}
                    maxlength={500}
                  />
                </Grid>
              </Div>
              <Div className="row" style={{ marginTop: '14px' }}>
                <Grid
                  style={{ display: 'flex', justifyContent: 'flex-start' }}
                  cols={12}
                >
                  <Button
                    label="Copiar avaliação"
                    icon="clipboard"
                    color={Colors.Azul}
                    border
                    className="btnGroupItem"
                    onClick={() => setMostrarModalCopiarAvaliacao(true)}
                    disabled={desabilitarCopiarAvaliacao}
                  />
                  {copias.length > 0 && (
                    <div style={{ marginLeft: '14px' }}>
                      <span>Avaliação será copiada para: </span>
                      <br />
                      {copias.map(x => (
                        <span style={{ display: 'block' }}>
                          <strong>Turma:</strong> &nbsp;
                          {x.turma[0].desc} <strong>Data: &nbsp;</strong>
                          {window.moment(x.dataAvaliacao).format('DD/MM/YYYY')}
                        </span>
                      ))}
                    </div>
                  )}
                </Grid>
              </Div>
            </Form>
            <Div className="row">
              <Grid cols={12}>
                <InseridoAlterado className="mt-4">
                  {inseridoAlterado.criadoPor && inseridoAlterado.criadoEm ? (
                    <p className="pt-2">
                      INSERIDO por {inseridoAlterado.criadoPor} em{' '}
                      {window.moment(inseridoAlterado.criadoEm).format()}
                    </p>
                  ) : (
                    ''
                  )}

                  {inseridoAlterado.alteradoPor &&
                  inseridoAlterado.alteradoEm ? (
                    <p>
                      ALTERADO por {inseridoAlterado.alteradoPor} em{' '}
                      {window.moment(inseridoAlterado.alteradoEm).format()}
                    </p>
                  ) : (
                    ''
                  )}
                </InseridoAlterado>
              </Grid>
            </Div>
          </Card>
        )}
      </Formik>
    </Div>
  );
};

AvaliacaoForm.propTypes = {
  match: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

AvaliacaoForm.defaultProps = {
  match: {},
};

export default AvaliacaoForm;
