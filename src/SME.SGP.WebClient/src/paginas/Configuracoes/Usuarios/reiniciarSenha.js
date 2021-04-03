import { Form, Formik } from 'formik';
import React, { useEffect, useState, useMemo, useCallback } from 'react';
import * as Yup from 'yup';
import moment from 'moment';
import { Loader } from '~/componentes';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import { Colors } from '~/componentes/colors';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import SelectComponent from '~/componentes/select';
import DataTable from '~/componentes/table/dataTable';
import { erros, confirmar } from '~/servicos/alertas';
import api from '~/servicos/api';
import { store } from '~/redux';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

import FiltroHelper from '~/componentes-sgp/filtro/helper';

export default function ReiniciarSenha({ perfilSelecionado }) {
  const [linhaSelecionada, setLinhaSelecionada] = useState({});
  const [listaUsuario, setListaUsuario] = useState([]);

  const [listaDres, setListaDres] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState('');
  const [listaUes, setListaUes] = useState([]);
  const [ueSelecionada, setUeSelecionada] = useState('');
  const [nomeUsuarioSelecionado, setNomeUsuarioSelecionado] = useState('');
  const [rfSelecionado, setRfSelecionado] = useState('');
  const [emailUsuarioSelecionado, setEmailUsuarioSelecionado] = useState('');
  const [exibirModalReiniciarSenha, setExibirModalReiniciarSenha] = useState(
    false
  );
  const [
    exibirModalMensagemReiniciarSenha,
    setExibirModalMensagemReiniciarSenha,
  ] = useState(false);
  const [mensagemSenhaAlterada, setMensagemSenhaAlterada] = useState('');

  const [semEmailCadastrado, setSemEmailCadastrado] = useState(false);
  const [refForm, setRefForm] = useState();

  const [dreDesabilitada, setDreDesabilitada] = useState(false);
  const [ueDesabilitada, setUeDesabilitada] = useState(false);

  const [carregando, setCarregando] = useState(false);

  const { usuario } = store.getState();
  const anoLetivo = useMemo(
    () =>
      (usuario.turmaSelecionada && usuario.turmaSelecionada.anoLetivo) ||
      moment().year(),
    [usuario.turmaSelecionada]
  );

  const consideraHistorico = useMemo(
    () =>
      (usuario.turmaSelecionada &&
        !!usuario.turmaSelecionada.consideraHistorico) ||
      false,
    [usuario.turmaSelecionada]
  );

  const permissoesTela = usuario.permissoes[RotasDto.REINICIAR_SENHA];

  const [validacoes] = useState(
    Yup.object({
      emailUsuario: Yup.string()
        .email('Digite um e-mail válido.')
        .required('E-mail é obrigatório'),
    })
  );

  const colunas = [
    {
      title: 'Nome do usuário',
      dataIndex: 'nomeServidor',
    },
    {
      title: 'Registro Funcional (RF)',
      dataIndex: 'codigoRf',
    },
    {
      title: 'Ação',
      dataIndex: 'acaoReiniciar',
      render: (texto, linha) => {
        return (
          <div className="botao-reiniciar-tabela-acao">
            <Button
              label="Reiniciar"
              color={Colors.Roxo}
              disabled={!permissoesTela.podeAlterar}
              border
              className="ml-2 text-center"
              onClick={() => onClickReiniciar(linha)}
            />
          </div>
        );
      },
    },
  ];

  useEffect(() => {
    const carregarDres = async () => {
      const dres = await api.get(
        `v1/abrangencias/${consideraHistorico}/dres?anoLetivo=${anoLetivo}`
      );
      if (dres.data) {
        setListaDres(dres.data.sort(FiltroHelper.ordenarLista('nome')));
      } else {
        setListaDres([]);
      }
    };

    carregarDres();
    verificaSomenteConsulta(permissoesTela);
  }, [anoLetivo, consideraHistorico, permissoesTela]);

  useEffect(() => {
    let desabilitada = !listaDres || listaDres.length === 0;

    if (!desabilitada && listaDres.length === 1) {
      setDreSelecionada(String(listaDres[0].codigo));
      desabilitada = true;
    }

    setDreDesabilitada(desabilitada);
  }, [listaDres]);

  useEffect(() => {
    let desabilitada = !listaUes || listaUes.length === 0;

    if (!desabilitada && listaUes.length === 1) {
      setUeSelecionada(String(listaUes[0].codigo));
      desabilitada = true;
    }

    setUeDesabilitada(desabilitada);
  }, [listaUes]);

  const onChangeDre = dre => {
    setDreSelecionada(!dre ? '' : dre);
    setUeSelecionada('');
    setListaUes([]);
  };

  const onChangeUe = ue => {
    setUeSelecionada(ue);
  };

  const onChangeNomeUsuario = nomeUsuario => {
    setNomeUsuarioSelecionado(nomeUsuario.target.value);
  };

  const onChangeRf = rf => {
    setRfSelecionado(rf.target.value);
  };

  const carregarUes = useCallback(
    async dre => {
      const ues = await api.get(
        `/v1/abrangencias/${consideraHistorico}/dres/${dre}/ues?consideraNovasUEs=${true}`
      );
      if (ues.data) {
        setListaUes(ues.data);
      } else {
        setListaUes([]);
      }
    },
    [consideraHistorico]
  );

  useEffect(() => {
    if (dreSelecionada) {
      carregarUes(dreSelecionada);
    }
  }, [carregarUes, dreSelecionada]);

  const onClickFiltrar = async () => {
    if (dreSelecionada) {
      setCarregando(true);
      const parametrosPost = {
        codigoDRE: dreSelecionada,
        nomeServidor: nomeUsuarioSelecionado,
        codigoRF: rfSelecionado,
      };

      if (ueSelecionada && ueSelecionada.length > 0)
        parametrosPost.codigoUE = ueSelecionada;

      const lista = await api
        .post(`v1/unidades-escolares/funcionarios`, parametrosPost)
        .catch(() => {
          setListaUsuario([]);
        })
        .finally(() => setCarregando(false));

      if (lista && lista.data) {
        setListaUsuario([]);
        setListaUsuario(lista.data);
      } else {
        setListaUsuario([]);
      }
    } else {
      setListaUsuario([]);
    }
  };

  const onClickReiniciar = async linha => {
    if (!permissoesTela.podeAlterar) return;

    setLinhaSelecionada(linha);
    const confirmou = await confirmar(
      'Reiniciar Senha',
      '',
      'Deseja realmente reiniciar essa senha?',
      'Reiniciar',
      'Cancelar'
    );
    if (confirmou) {
      reiniciarSenha(linha);
    }
  };

  const reiniciarSenha = async linha => {
    const parametros = {
      dreCodigo: dreSelecionada,
      ueCodigo: ueSelecionada,
    };

    let deveAtualizarEmail = false;
    setCarregando(true);
    await api
      .put(`v1/autenticacao/${linha.codigoRf}/reiniciar-senha`, parametros)
      .then(resposta => {
        setExibirModalMensagemReiniciarSenha(true);
        setMensagemSenhaAlterada(resposta.data.mensagem);
      })
      .catch(error => {
        if (error && error.response && error.response.data) {
          deveAtualizarEmail = error.response.data.deveAtualizarEmail;
        }
        setCarregando(false);
      });
    if (deveAtualizarEmail) {
      setEmailUsuarioSelecionado('');
      setSemEmailCadastrado(true);
      setExibirModalReiniciarSenha(true);
    } else {
      setSemEmailCadastrado(false);
      onClickFiltrar();
    }
    setCarregando(false);
  };

  const onCloseModalReiniciarSenha = () => {
    setExibirModalReiniciarSenha(false);
    setExibirModalMensagemReiniciarSenha(false);
    setSemEmailCadastrado(false);
    refForm.resetForm();
  };

  const onConfirmarReiniciarSenha = async form => {
    const parametro = { novoEmail: form.emailUsuario };
    onCloseModalReiniciarSenha();
    setCarregando(true);
    api
      .put(`v1/usuarios/${linhaSelecionada.codigoRf}/email`, parametro)
      .then(() => {
        reiniciarSenha(linhaSelecionada);
        refForm.resetForm();
        setCarregando(false);
      })
      .catch(e => {
        erros(e);
        setCarregando(false);
      });
  };

  const onCancelarReiniciarSenha = () => {
    onCloseModalReiniciarSenha();
  };

  const validaSeTemEmailCadastrado = () => {
    return semEmailCadastrado
      ? `Este usuário não tem e-mail cadastrado, para seguir com
     o processo de reinicio da senha é obrigatório informar um e-mail válido.`
      : null;
  };

  return (
    <Loader loading={carregando}>
      <div className="row">
        <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 pb-2">
          <SelectComponent
            name="dre-reiniciar-senha"
            id="dre-reiniciar-senha"
            lista={listaDres}
            disabled={!permissoesTela.podeConsultar || dreDesabilitada}
            valueOption="codigo"
            valueText="nome"
            onChange={onChangeDre}
            valueSelect={String(dreSelecionada) || ''}
            label="Diretoria Regional de Educação (DRE)"
            placeholder="Diretoria Regional de Educação (DRE)"
          />
        </div>
        <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 pb-2">
          <SelectComponent
            name="ues-list"
            id="ues-list"
            lista={listaUes}
            disabled={!permissoesTela.podeConsultar || ueDesabilitada}
            valueOption="codigo"
            valueText="nome"
            onChange={onChangeUe}
            valueSelect={ueSelecionada || ''}
            label="Unidade Escolar (UE)"
            placeholder="Unidade Escolar (UE)"
          />
        </div>
      </div>

      <div className="row">
        <div className="col-sm-12 col-md-6 col-lg-6 col-xl-6 pb-3">
          <CampoTexto
            label="Nome do usuário"
            placeholder="Nome do usuário"
            onChange={onChangeNomeUsuario}
            desabilitado={!permissoesTela.podeConsultar}
            value={nomeUsuarioSelecionado}
          />
        </div>
        <div className="col-sm-12 col-md-6 col-lg-4 col-xl-5 pb-3">
          <CampoTexto
            label="Registro Funcional (RF)"
            placeholder="Registro Funcional (RF)"
            onChange={onChangeRf}
            desabilitado={!permissoesTela.podeConsultar}
            value={rfSelecionado}
          />
        </div>
        <div className="col-sm-12 col-md-12 col-lg-2 col-xl-1 pb-3">
          <Button
            label="Filtrar"
            color={Colors.Azul}
            disabled={perfilSelecionado || !dreSelecionada}
            border
            className="text-center d-block mt-4 float-right w-100"
            onClick={onClickFiltrar}
          />
        </div>
      </div>

      {listaUsuario.length > 0 && (
        <div className="row">
          <div className="col-md-12 pt-4">
            <DataTable
              rowKey="codigoRf"
              columns={colunas}
              dataSource={listaUsuario}
            />
          </div>
        </div>
      )}

      <Formik
        ref={refFormik => setRefForm(refFormik)}
        enableReinitialize
        initialValues={{
          emailUsuario: emailUsuarioSelecionado,
        }}
        validationSchema={validacoes}
        onSubmit={values => onConfirmarReiniciarSenha(values)}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <ModalConteudoHtml
              key="reiniciarSenha"
              visivel={exibirModalReiniciarSenha}
              onConfirmacaoPrincipal={() => {
                form.validateForm().then(() => form.handleSubmit(e => e));
              }}
              onConfirmacaoSecundaria={() => onCancelarReiniciarSenha()}
              onClose={onCloseModalReiniciarSenha}
              labelBotaoPrincipal="Cadastrar e reiniciar"
              tituloAtencao={semEmailCadastrado ? 'Atenção' : null}
              perguntaAtencao={validaSeTemEmailCadastrado()}
              labelBotaoSecundario="Cancelar"
              titulo="Reiniciar Senha"
              closable
            >
              <b> Deseja realmente reiniciar essa senha? </b>

              <CampoTexto
                label="E-mail"
                name="emailUsuario"
                form={form}
                maxlength="50"
              />
            </ModalConteudoHtml>
          </Form>
        )}
      </Formik>

      <ModalConteudoHtml
        key="exibirModalMensagemReiniciarSenha"
        visivel={exibirModalMensagemReiniciarSenha}
        onClose={onCloseModalReiniciarSenha}
        onConfirmacaoPrincipal={onCloseModalReiniciarSenha}
        labelBotaoPrincipal="OK"
        titulo="Senha reiniciada"
        esconderBotaoSecundario
        closable
      >
        <b> {mensagemSenhaAlterada} </b>
      </ModalConteudoHtml>
    </Loader>
  );
}
