import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { Progress } from 'antd';
import Cabecalho from '~/componentes-sgp/cabecalho';
import AlertaBalao from '~/componentes/alertaBalao';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors, Base } from '~/componentes/colors';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import history from '~/servicos/history';
import api from '~/servicos/api';
import { erros } from '~/servicos/alertas';
import { meusDados } from '~/redux/modulos/usuario/actions';
import {
  Botao,
  Conteudo,
  DadosPerfil,
  Icone,
  MensagemAlerta,
  Perfil,
  SelecionarFoto,
  Topo,
  BarraProgresso,
} from './meusDados.css';
import { store } from '~/redux';
import DadosEmail from './dadosEmail';

const MeusDados = () => {
  const usuarioStore = useSelector(store => store.usuario);
  const [foto, setFoto] = useState(usuarioStore.meusDados.foto);
  const [alterarFoto, setAlterarFoto] = useState(false);
  const [ehFotoInvalida, setEhFotoInvalida] = useState(false);
  const [desabilitaConfirmar, setDesabilitaConfirmar] = useState(true);
  const [ocultarAlteracoesNaoSalvas, setOcultarAlteracoesNaoSalvas] = useState(
    true
  );
  const [ocultarProgresso, setOcultarProgresso] = useState(true);
  const [progresso, setProgresso] = useState(0);

  const irParaDashboard = () => {
    history.push('/');
  };

  const ocultarModal = () => {
    setEhFotoInvalida(false);
    setFoto(usuarioStore.meusDados.foto);
    setAlterarFoto(!alterarFoto);
    setDesabilitaConfirmar(true);
    setOcultarAlteracoesNaoSalvas(true);
    setOcultarProgresso(true);
  };

  const selecionarNovaFoto = () => {
    window.document.getElementById('selecionar-foto').click();
  };

  const arquivoSelecionado = event => {
    const arquivo = event.target.files[0];
    if (arquivo) {
      if (arquivo.size <= 2000000) {
        const img = new Image();
        img.src = window.URL.createObjectURL(arquivo);
        img.onload = e => {
          if (img.naturalHeight > 180 && img.naturalWidth > 180) {
            const fileReader = new FileReader();
            fileReader.readAsDataURL(arquivo);
            fileReader.onloadend = () => {
              const novaFoto = fileReader.result;
              setEhFotoInvalida(false);
              setFoto(novaFoto);
              setDesabilitaConfirmar(false);
            };
          } else {
            setEhFotoInvalida(true);
          }
        };
      } else {
        setEhFotoInvalida(true);
      }
    }
  };

  const cancelarTrocaFoto = () => {
    if (foto !== usuarioStore.meusDados.foto) {
      setOcultarAlteracoesNaoSalvas(false);
    } else {
      ocultarModal();
    }
  };

  const salvarFoto = () => {
    setOcultarProgresso(false);
    const dados = { imagemBase64: foto };
    api
      .post('v1/usuarios/imagens/perfil', dados, {
        onUploadProgress: progressEvent => {
          setProgresso(
            Math.round((progressEvent.loaded / progressEvent.total) * 100)
          );
        },
      })
      .then(resp => {
        const novosDados = usuarioStore.meusDados;
        novosDados.foto = resp.data;
        store.dispatch(meusDados(novosDados));
        ocultarModal();
      })
      .catch(erro => {
        erros(erro);
      });
  };

  return (
    <div>
      <Cabecalho pagina="Meus Dados" />
      <Card>
        <ModalConteudoHtml
          key="trocarFoto"
          visivel={alterarFoto}
          onConfirmacaoPrincipal={salvarFoto}
          onConfirmacaoSecundaria={cancelarTrocaFoto}
          labelBotaoPrincipal="Confirmar"
          labelBotaoSecundario="Cancelar"
          desabilitarBotaoPrincipal={desabilitaConfirmar}
          esconderBotoes={!ocultarAlteracoesNaoSalvas}
          titulo="Alterar Foto"
          closable
          onClose={cancelarTrocaFoto}
        >
          <div
            id="troca-foto"
            hidden={!ocultarAlteracoesNaoSalvas || !ocultarProgresso}
          >
            <DadosPerfil className="col-12">
              <img className="img-edit" id="foto-perfil" src={foto} />
            </DadosPerfil>
            <DadosPerfil className="col-12">
              <SelecionarFoto
                className="text-center"
                onClick={selecionarNovaFoto}
              >
                <input
                  type="file"
                  hidden
                  accept="image/jpeg, image/png"
                  id="selecionar-foto"
                  onChange={arquivoSelecionado}
                />
                Selecionar nova foto
              </SelecionarFoto>
              <AlertaBalao
                maxWidth={294}
                marginTop={14}
                mostrarAlerta={ehFotoInvalida}
                texto="A resolução mínima é de 180 x 180 pixels, com tamanho máximo de 2Mb."
              />
            </DadosPerfil>
          </div>
          <MensagemAlerta
            id="mensagem-alerta"
            hidden={ocultarAlteracoesNaoSalvas}
          >
            <span className="titulo">Alerta</span>
            <br />
            <span>Suas alterações não foram salvas, deseja salvar agora?</span>
            <div className="d-flex justify-content-end p-t-20">
              <Button
                key="btn-confirma-alteracao"
                label="Não"
                color={Colors.Azul}
                bold
                border
                className="mr-2 padding-btn-confirmacao"
                onClick={ocultarModal}
              />
              <Button
                key="btn-cancela-alteracao"
                label="Sim"
                color={Colors.Azul}
                bold
                className="padding-btn-confirmacao"
                onClick={salvarFoto}
              />
            </div>
          </MensagemAlerta>
          <BarraProgresso hidden={ocultarProgresso}>
            <span>Salvando..</span>
            <Progress
              strokeColor={Base.Roxo}
              percent={progresso}
              showInfo={false}
            />
          </BarraProgresso>
        </ModalConteudoHtml>
        <Topo className="col-12 d-flex justify-content-end">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-3"
            onClick={irParaDashboard}
          />
        </Topo>
        <Perfil className="col-4">
          <DadosPerfil className="col-12">
            {/* <img id="foto-perfil" className="img-profile" src={usuarioStore.meusDados.foto} />
            <Botao className="text-center" onClick={ocultarModal}>
              <Icone>
                <i className="fas fa-camera" />
              </Icone>
            </Botao> */}
            <i className="fas fa-user-circle icone-perfil"></i>
          </DadosPerfil>
          <DadosPerfil className="text-center">
            <span className="nome">{usuarioStore.meusDados.nome}</span>
            <span hidden={!usuarioStore.meusDados.rf}>
              RF: {usuarioStore.meusDados.rf}
            </span>
            <span hidden={!usuarioStore.meusDados.cpf}>
              CPF: {usuarioStore.meusDados.cpf}
            </span>
            <span hidden={!usuarioStore.meusDados.empresa}>
              Empresa: {usuarioStore.meusDados.empresa}
            </span>
          </DadosPerfil>
        </Perfil>
        <Conteudo className="col-8">
          <DadosEmail />
        </Conteudo>
      </Card>
    </div>
  );
};

export default MeusDados;
