import { Form, Formik } from 'formik';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import { CampoTexto, Colors, Label, Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import Editor from '~/componentes/editor/editor';
import SelectComponent from '~/componentes/select';
import modalidade from '~/dtos/modalidade';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';
import ServicoCompensacaoAusencia from '~/servicos/Paginas/DiarioClasse/ServicoCompensacaoAusencia';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';

import ListaAlunos from './listasAlunos/listaAlunos';
import ListaAlunosAusenciasCompensadas from './listasAlunos/listaAlunosAusenciasCompensadas';
import { Badge, BotaoListaAlunos, ColunaBotaoListaAlunos } from './styles';

const CompensacaoAusenciaForm = ({ match }) => {
  const usuario = useSelector(store => store.usuario);

  const { turmaSelecionada } = usuario;

  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [carregandoDisciplinas, setCarregandoDisciplinas] = useState(false);
  const [
    carregandoListaAlunosFrequencia,
    setCarregandoListaAlunosFrequencia,
  ] = useState(false);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [carregouInformacoes, setCarregouInformacoes] = useState(false);
  const [idCompensacaoAusencia, setIdCompensacaoAusencia] = useState(0);
  const [listaBimestres, setListaBimestres] = useState([]);
  const [auditoria, setAuditoria] = useState([]);
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [listaDisciplinasRegencia, setListaDisciplinasRegencia] = useState([]);
  const [temRegencia, setTemRegencia] = useState(false);
  const [refForm, setRefForm] = useState({});

  const [alunosAusenciaTurma, setAlunosAusenciaTurma] = useState([]);
  const [alunosAusenciaCompensada, setAlunosAusenciaCompensada] = useState([]);
  const [idsAlunos, setIdsAlunos] = useState([]);
  const [
    idsAlunosAusenciaCompensadas,
    setIdsAlunosAusenciaCompensadas,
  ] = useState([]);

  const [valoresIniciais, setValoresIniciais] = useState({
    disciplinaId: undefined,
    bimestre: '',
    atividade: '',
    descricao: '',
  });

  const [validacoes] = useState(
    Yup.object({
      disciplinaId: Yup.string().required('Disciplina obrigatória'),
      bimestre: Yup.string().required('Bimestre obrigatório'),
      atividade: Yup.string()
        .required('Atividade obrigatória')
        .max(250, 'Máximo 250 caracteres'),
    })
  );

  const resetarForm = useCallback(() => {
    if (refForm && refForm.resetForm) {
      refForm.resetForm();
    }
    setListaDisciplinasRegencia([]);
    setTemRegencia(false);
  }, [refForm]);

  const onChangeCampos = () => {
    if (carregouInformacoes && !modoEdicao) {
      setModoEdicao(true);
    }
  };

  const selecionarDisciplina = indice => {
    const disciplinas = [...listaDisciplinasRegencia];
    disciplinas[indice].selecionada = !disciplinas[indice].selecionada;
    disciplinas[indice].codigo = disciplinas[indice].codigoComponenteCurricular;
    setListaDisciplinasRegencia(disciplinas);
    onChangeCampos();
  };

  const obterDisciplinasRegencia = async codigoDisciplinaSelecionada => {
    const disciplina = listaDisciplinas.find(
      c => c.codigoComponenteCurricular == codigoDisciplinaSelecionada
    );
    // TODO REMOVER
    // if (disciplina) {
    //   disciplina.regencia = true;
    // }
    // TODO REMOVER
    if (disciplina && disciplina.regencia) {
      const disciplinasRegencia = await ServicoDisciplina.obterDisciplinasPlanejamento(
        codigoDisciplinaSelecionada,
        turmaSelecionada.turma,
        false,
        disciplina.regencia
      ).catch(e => erros(e));

      if (
        disciplinasRegencia &&
        disciplinasRegencia.data &&
        disciplinasRegencia.data.length
      ) {
        setListaDisciplinasRegencia(disciplinasRegencia.data);
        setTemRegencia(true);
      }
    } else {
      setListaDisciplinasRegencia([]);
      setTemRegencia(false);
    }
  };

  const onChangeDisciplina = (codigoDisciplina, form) => {
    setAlunosAusenciaTurma([]);
    setAlunosAusenciaCompensada([]);
    setIdsAlunosAusenciaCompensadas([]);
    setIdsAlunos([]);
    obterDisciplinasRegencia(codigoDisciplina);
    onChangeCampos();
    form.setFieldValue('bimestre', '');
  };

  useEffect(() => {
    const obterDisciplinas = async () => {
      setCarregandoDisciplinas(true);
      const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
        turmaSelecionada.turma
      );
      if (disciplinas.data && disciplinas.data.length) {
        setListaDisciplinas(disciplinas.data);
      } else {
        setListaDisciplinas([]);
      }

      if (disciplinas.data && disciplinas.data.length === 1) {
        setDesabilitarDisciplina(true);

        if (!(match && match.params && match.params.id)) {
          const disciplina = disciplinas.data[0];
          const valoresIniciaisForm = {
            disciplinaId: String(disciplina.codigoComponenteCurricular),
            bimestre: '',
            atividade: '',
            descricao: '',
          };
          setValoresIniciais(valoresIniciaisForm);
        }
      }
      if (!(match && match.params && match.params.id)) {
        setCarregouInformacoes(true);
      }
      setCarregandoDisciplinas(false);
    };

    if (turmaSelecionada.turma) {
      resetarForm();
      obterDisciplinas(turmaSelecionada.turma);
    } else {
      resetarForm();
    }

    let listaBi = [];
    if (turmaSelecionada.modalidade == modalidade.EJA) {
      listaBi = [
        { valor: 1, descricao: '1°' },
        { valor: 2, descricao: '2°' },
      ];
    } else {
      listaBi = [
        { valor: 1, descricao: '1°' },
        { valor: 2, descricao: '2°' },
        { valor: 3, descricao: '3°' },
        { valor: 4, descricao: '4°' },
      ];
    }
    setListaBimestres(listaBi);
  }, [match, turmaSelecionada.modalidade, turmaSelecionada.turma, resetarForm]);

  const removerAlunosDuplicadosEdicao = (alunosTurma, alunosEdicao) => {
    const novaLista = alunosTurma.filter(
      aluno => !alunosEdicao.find(al => al.id == aluno.id)
    );
    return novaLista;
  };

  const obterAlunosComAusencia = useCallback(
    async (disciplinaId, bimestre, listaAlunosEdicao) => {
      setCarregandoListaAlunosFrequencia(true);
      const alunos = await ServicoCompensacaoAusencia.obterAlunosComAusencia(
        turmaSelecionada.turma,
        disciplinaId,
        bimestre
      ).catch(e => {
        setCarregandoListaAlunosFrequencia(false);
        setAlunosAusenciaTurma([]);
        erros(e);
      });
      if (alunos && alunos.data && alunos.data.length) {
        if (listaAlunosEdicao && listaAlunosEdicao.length) {
          const listaSemDuplicados = removerAlunosDuplicadosEdicao(
            alunos.data,
            listaAlunosEdicao
          );
          setAlunosAusenciaTurma([...listaSemDuplicados]);
        } else {
          setAlunosAusenciaTurma([...alunos.data]);
        }
      } else {
        setAlunosAusenciaTurma([]);
      }
      setCarregandoListaAlunosFrequencia(false);
    },
    [turmaSelecionada.turma]
  );

  useEffect(() => {
    const consultaPorId = async () => {
      setBreadcrumbManual(
        match.url,
        'Alterar Compensação de Ausência',
        '/diario-classe/compensacao-ausencia'
      );
      setIdCompensacaoAusencia(match.params.id);

      const dadosEdicao = await ServicoCompensacaoAusencia.obterPorId(
        match.params.id
      ).catch(e => erros(e));

      if (dadosEdicao && dadosEdicao.data) {
        setValoresIniciais({
          disciplinaId: String(dadosEdicao.data.disciplinaId),
          bimestre: String(dadosEdicao.data.bimestre),
          atividade: dadosEdicao.data.atividade,
          descricao: dadosEdicao.data.descricao,
        });
        if (dadosEdicao.data.alunos && dadosEdicao.data.alunos.length) {
          setAlunosAusenciaCompensada(dadosEdicao.data.alunos);
        }

        obterAlunosComAusencia(
          dadosEdicao.data.disciplinaId,
          dadosEdicao.data.bimestre,
          dadosEdicao.data.alunos
        );

        setAuditoria({
          criadoPor: dadosEdicao.data.criadoPor,
          criadoRf: dadosEdicao.data.criadoRf,
          criadoEm: dadosEdicao.data.criadoEm,
          alteradoPor: dadosEdicao.data.alteradoPor,
          alteradoRf: dadosEdicao.data.alteradoRf,
          alteradoEm: dadosEdicao.data.alteradoEm,
        });
        setExibirAuditoria(true);
        setCarregouInformacoes(true);
      }
      setNovoRegistro(false);
    };

    if (turmaSelecionada.turma && match && match.params && match.params.id) {
      consultaPorId();
    }
  }, [match, turmaSelecionada.turma, obterAlunosComAusencia]);

  const onChangeBimestre = (bimestre, form) => {
    setAlunosAusenciaCompensada([]);
    setIdsAlunosAusenciaCompensadas([]);
    setIdsAlunos([]);
    if (bimestre && form && form.values.disciplinaId) {
      obterAlunosComAusencia(form.values.disciplinaId, bimestre);
    } else {
      setAlunosAusenciaTurma([]);
    }
    onChangeCampos();
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length == 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  const onClickExcluir = async () => {
    if (!novoRegistro) {
      const confirmado = await confirmar(
        'Excluir compensação',
        '',
        'Você tem certeza que deseja excluir este registro',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const excluir = await ServicoCompensacaoAusencia.deletar([
          idCompensacaoAusencia,
        ]).catch(e => erros(e));

        if (excluir && excluir.status == 200) {
          sucesso('Compensação excluída com sucesso.');
          history.push('/diario-classe/compensacao-ausencia');
        }
      }
    }
  };

  const resetarTelaEdicaoComId = async form => {
    const dadosEdicao = await ServicoCompensacaoAusencia.obterPorId(
      match.params.id
    ).catch(e => {
      erros(e);
    });
    if (dadosEdicao && dadosEdicao.status == 200) {
      setIdsAlunos([]);
      setIdsAlunosAusenciaCompensadas([]);
      if (dadosEdicao.data.alunos && dadosEdicao.data.alunos.length) {
        setAlunosAusenciaCompensada(dadosEdicao.data.alunos);
      } else {
        setAlunosAusenciaCompensada([]);
      }
      obterAlunosComAusencia(
        form.values.disciplinaId,
        form.values.bimestre,
        dadosEdicao.data.alunos
      );
      form.resetForm();
      setModoEdicao(false);
    }
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        if (match && match.params && match.params.id) {
          resetarTelaEdicaoComId(form);
        } else {
          setIdsAlunos([]);
          setAlunosAusenciaTurma([]);
          setIdsAlunosAusenciaCompensadas([]);
          setAlunosAusenciaCompensada([]);
          form.resetForm();
          setModoEdicao(false);
        }
      }
    }
  };

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickVoltar = async form => {
    if (modoEdicao) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        validaAntesDoSubmit(form);
      }
    } else {
      history.push('/diario-classe/compensacao-ausencia');
    }
  };

  const onClickCadastrar = async valoresForm => {
    const paramas = valoresForm;
    paramas.id = idCompensacaoAusencia;
    paramas.turmaId = turmaSelecionada.turma;

    paramas.disciplinasRegenciaIds = [];
    if (temRegencia) {
      const somenteSelecionados = listaDisciplinasRegencia.filter(
        item => item.selecionada
      );
      paramas.disciplinasRegenciaIds = somenteSelecionados.map(item =>
        String(item.codigoComponenteCurricular)
      );
    }
    paramas.alunos = alunosAusenciaCompensada.map(item => {
      return {
        id: item.id,
        qtdFaltasCompensadas: item.quantidadeFaltasCompensadas,
      };
    });
    console.log(paramas);

    const cadastrado = await ServicoCompensacaoAusencia.salvar(
      paramas.id,
      paramas
    ).catch(e => erros(e));

    if (cadastrado && cadastrado.status == 200) {
      if (idCompensacaoAusencia) {
        sucesso('Tipo de feriado alterado com sucesso.');
      } else {
        sucesso('Novo tipo de feriado criado com sucesso.');
      }
      history.push('/diario-classe/compensacao-ausencia');
    }
  };

  const obterListaAlunosComIdsSelecionados = (list, ids) => {
    return list.filter(item => ids.find(id => id == item.id));
  };

  const obterListaAlunosSemIdsSelecionados = (list, ids) => {
    return list.filter(item => !ids.find(id => id == item.id));
  };

  const onClickAdicionarAlunos = () => {
    if (idsAlunos && idsAlunos.length) {
      const novaListaAlunosAusenciaCompensada = obterListaAlunosComIdsSelecionados(
        alunosAusenciaTurma,
        idsAlunos
      );

      const novaListaAlunos = obterListaAlunosSemIdsSelecionados(
        alunosAusenciaTurma,
        idsAlunos
      );

      onChangeCampos();
      setAlunosAusenciaTurma([...novaListaAlunos]);
      setAlunosAusenciaCompensada([
        ...novaListaAlunosAusenciaCompensada,
        ...alunosAusenciaCompensada,
      ]);
      setIdsAlunos([]);
    }
  };

  const onClickRemoverAlunos = async () => {
    if (idsAlunosAusenciaCompensadas && idsAlunosAusenciaCompensadas.length) {
      const listaAlunosRemover = alunosAusenciaCompensada.filter(item =>
        idsAlunosAusenciaCompensadas.find(id => id == item.id)
      );
      const confirmado = await confirmar(
        'Excluir aluno',
        listaAlunosRemover.map(item => {
          return `${item.id} - ${item.nome}`;
        }),
        'A frequência de aluno será recalculada somente quando salvar as suas alteraçãos. Deseja continuar?',
        'Excluir',
        'Cancelar',
        true
      );

      if (confirmado) {
        const novaListaAlunos = obterListaAlunosComIdsSelecionados(
          alunosAusenciaCompensada,
          idsAlunosAusenciaCompensadas
        );

        const novaListaAlunosAusenciaCompensada = obterListaAlunosSemIdsSelecionados(
          alunosAusenciaCompensada,
          idsAlunosAusenciaCompensadas
        );

        onChangeCampos();
        setAlunosAusenciaTurma([...novaListaAlunos, ...alunosAusenciaTurma]);
        setAlunosAusenciaCompensada([...novaListaAlunosAusenciaCompensada]);
        setIdsAlunosAusenciaCompensadas([]);
      }
    }
  };

  const onSelectRowAlunos = ids => {
    setIdsAlunos(ids);
  };

  const onSelectRowAlunosAusenciaCompensada = ids => {
    setIdsAlunosAusenciaCompensadas(ids);
  };

  const atualizarValoresListaCompensacao = novaListaAlunos => {
    onChangeCampos();
    setAlunosAusenciaCompensada([...novaListaAlunos]);
  };

  return (
    <>
      <Cabecalho pagina="Cadastrar Compensação de Ausência" />
      <Card>
        <Loader loading={carregandoListaAlunosFrequencia} tip="">
          <Formik
            enableReinitialize
            ref={refF => setRefForm(refF)}
            initialValues={valoresIniciais}
            validationSchema={validacoes}
            onSubmit={onClickCadastrar}
            validateOnChange
            validateOnBlur
          >
            {form => (
              <Form className="col-md-12 mb-4">
                <div className="d-flex justify-content-end pb-4">
                  <Button
                    id="btn-voltar"
                    label="Voltar"
                    icon="arrow-left"
                    color={Colors.Azul}
                    border
                    className="mr-2"
                    onClick={() => onClickVoltar(form)}
                  />
                  <Button
                    id="btn-cancelar"
                    label="Cancelar"
                    color={Colors.Roxo}
                    border
                    className="mr-2"
                    onClick={() => onClickCancelar(form)}
                    disabled={!modoEdicao}
                  />
                  <Button
                    id="btn-excluir"
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    disabled={novoRegistro}
                    onClick={onClickExcluir}
                  />
                  <Button
                    id="btn-salvar"
                    label={`${
                      idCompensacaoAusencia > 0 ? 'Alterar' : 'Cadastrar'
                    }`}
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-2"
                    onClick={() => validaAntesDoSubmit(form)}
                  />
                </div>

                <div className="row">
                  <div className="col-sm-12 col-md-8 col-lg-4 col-xl-4 mb-2">
                    <Loader loading={carregandoDisciplinas} tip="">
                      <SelectComponent
                        form={form}
                        id="disciplina"
                        label="Disciplina"
                        name="disciplinaId"
                        lista={listaDisciplinas}
                        valueOption="codigoComponenteCurricular"
                        valueText="nome"
                        onChange={valor => onChangeDisciplina(valor, form)}
                        placeholder="Disciplina"
                        disabled={desabilitarDisciplina || !novoRegistro}
                      />
                    </Loader>
                  </div>
                  <div className="col-sm-12 col-md-4 col-lg-2 col-xl-2 mb-2">
                    <SelectComponent
                      form={form}
                      id="bimestre"
                      label="Bimestre"
                      name="bimestre"
                      lista={listaBimestres}
                      valueOption="valor"
                      valueText="descricao"
                      onChange={bi => onChangeBimestre(bi, form)}
                      placeholder="Bimestre"
                      disabled={!novoRegistro}
                    />
                  </div>
                  <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
                    <CampoTexto
                      form={form}
                      label="Atividade"
                      placeholder="Atividade"
                      name="atividade"
                      onChange={onChangeCampos}
                      type="input"
                      maxLength="250"
                    />
                  </div>
                  {temRegencia && listaDisciplinasRegencia && (
                    <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
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
                    </div>
                  )}

                  <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                    <Editor
                      form={form}
                      name="descricao"
                      onChange={onChangeCampos}
                      label="Detalhamento da atividade"
                    />
                  </div>
                </div>
                <div className="row mt-2">
                  <div className="col-md-5">
                    <ListaAlunos
                      lista={alunosAusenciaTurma}
                      onSelectRow={onSelectRowAlunos}
                      idsAlunos={idsAlunos}
                    />
                  </div>
                  <ColunaBotaoListaAlunos className="col-md-2">
                    <BotaoListaAlunos
                      className="mb-2"
                      onClick={onClickAdicionarAlunos}
                    >
                      <i className="fas fa-chevron-right" />
                    </BotaoListaAlunos>
                    <BotaoListaAlunos onClick={onClickRemoverAlunos}>
                      <i className="fas fa-chevron-left" />
                    </BotaoListaAlunos>
                  </ColunaBotaoListaAlunos>
                  <div className="col-md-5">
                    <ListaAlunosAusenciasCompensadas
                      listaAusenciaCompensada={alunosAusenciaCompensada}
                      onSelectRow={onSelectRowAlunosAusenciaCompensada}
                      idsAlunosAusenciaCompensadas={
                        idsAlunosAusenciaCompensadas
                      }
                      atualizarValoresListaCompensacao={
                        atualizarValoresListaCompensacao
                      }
                    />
                  </div>
                </div>
              </Form>
            )}
          </Formik>
          {exibirAuditoria ? (
            <Auditoria
              criadoEm={auditoria.criadoEm}
              criadoPor={auditoria.criadoPor}
              alteradoPor={auditoria.alteradoPor}
              alteradoEm={auditoria.alteradoEm}
            />
          ) : (
            ''
          )}
        </Loader>
      </Card>
    </>
  );
};

export default CompensacaoAusenciaForm;
