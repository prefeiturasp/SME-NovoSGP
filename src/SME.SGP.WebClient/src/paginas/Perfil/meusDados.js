import React, { useState } from 'react';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';
import history from '~/servicos/history';
import Button from '~/componentes/button';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import { store } from '~/redux';
import { useSelector } from 'react-redux';
import { meusDados } from '~/redux/modulos/usuario/actions';
import AlertaBalao from '~/componentes/alertaBalao';
import { DadosPerfil, Perfil, Botao, Icone, Conteudo, SelecionarFoto, Topo, MensagemAlerta } from './meusDados.css';

const MeusDados = () => {
  const usuarioStore = useSelector(store => store.usuario);
  const [foto, setFoto] = useState(usuarioStore.meusDados.foto);
  const [alterarFoto, setAlterarFoto] = useState(false);
  const [ehFotoInvalida, setEhFotoInvalida] = useState(false);
  const [desabilitaConfirmar, setDesabilitaConfirmar] = useState(true);

  const irParaDashboard = () => {
    history.push('/');
  }

  const mostrarModal = () => {
    setEhFotoInvalida(false);
    setFoto(usuarioStore.meusDados.foto);
    setAlterarFoto(!alterarFoto);
    setDesabilitaConfirmar(true);
  }

  const selecionarNovaFoto = () => {
    window.document.getElementById('selecionar-foto').click();
  }

  const arquivoSelecionado = event => {
    const arquivo = event.target.files[0];
    if (arquivo && arquivo.size <= 2000000) {
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
            setDesabilitaConfirmar(false)
          }
        } else {
          setEhFotoInvalida(true);
        }
      }
    } else {
      setEhFotoInvalida(true);
    }
  }

  return (
    <div>
      <Cabecalho pagina="Meus Dados" />
      <Card>
        <ModalConteudoHtml
          key={'trocarFoto'}
          visivel={alterarFoto}
          onConfirmacaoPrincipal={() => { }}
          onConfirmacaoSecundaria={mostrarModal}
          onClose={() => { }}
          labelBotaoPrincipal="Confirmar"
          labelBotaoSecundario="Cancelar"
          desabilitarBotaoPrincipal={desabilitaConfirmar}
          titulo="Alterar Foto"
          closable={true}
        >
          <DadosPerfil className="col-12">
            <img className="img-edit" id="foto-perfil" src={foto} />
          </DadosPerfil>
          <DadosPerfil className="col-12">
            <SelecionarFoto className="text-center" onClick={selecionarNovaFoto}>
              <input type="file" hidden accept="image/jpeg, image/png"
                id="selecionar-foto" onChange={arquivoSelecionado} />
              Selecionar nova foto
            </SelecionarFoto>
            <AlertaBalao maxWidth={294} marginTop={14} mostrarAlerta={ehFotoInvalida}
              texto="A resolução mínima é de 180 x 180 pixels, com tamanho máximo de 2Mb." />
          </DadosPerfil>
        </ModalConteudoHtml>
        <MensagemAlerta>
          <span className="titulo">Alerta</span><br />
          <span>Suas alterações não foram salvas, deseja salvar agora?</span>
          <div className="d-flex justify-content-end">
            <Button
              key="btn-confirma-alteracao"
              label="Não"
              color={Base.Azul}
              bold
              border
              className="mr-2 padding-btn-confirmacao"
            />
            <Button
              key="btn-cancela-alteracao"
              label="Sim"
              color={Base.Azul}
              bold
              className="padding-btn-confirmacao"
            />
          </div>
        </MensagemAlerta>
        <Topo className="col-12 d-flex justify-content-end">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Base.Azul}
            border
            className="mr-3"
            onClick={irParaDashboard}
          />
        </Topo>
        <Perfil className="col-4">
          <DadosPerfil className="col-12">
            <img id="foto-perfil" className="img-profile" src={usuarioStore.meusDados.foto} />
            <Botao className="text-center" onClick={mostrarModal}>
              <Icone>
                <i className="fas fa-camera" />
              </Icone>
            </Botao>
          </DadosPerfil>
          <DadosPerfil className="text-center">
            <span className="nome">{usuarioStore.meusDados.nome}</span>
            <span hidden={!usuarioStore.meusDados.rf}>RF: {usuarioStore.meusDados.rf}</span>
            <span hidden={!usuarioStore.meusDados.cpf}>CPF: {usuarioStore.meusDados.cpf}</span>
            <span hidden={!usuarioStore.meusDados.empresa}>Empresa: {usuarioStore.meusDados.empresa}</span>
          </DadosPerfil>
        </Perfil>
        <Conteudo className="col-8">
        </Conteudo>
      </Card>
    </div>
  );
}

export default MeusDados;
