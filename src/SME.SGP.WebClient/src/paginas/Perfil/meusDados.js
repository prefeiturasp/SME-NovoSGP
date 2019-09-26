import React, { useState } from 'react';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';
import history from '~/servicos/history';
import Button from '~/componentes/button';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';

const MeusDados = () => {

    const dados = {
        nome: 'Teste com Sobrenome Um tanto Quanto Maior',
        rf: '123456',
        cpf: '12345678901',
        empresa: 'SME',
        foto: 'https://graziellanicolai.com.br/wp-content/uploads/2018/03/Graziella-perfil.jpg'
    }

    const [alterarFoto, setAlterarFoto] = useState(false);

    const Perfil = styled.div`
        padding: 0 !important;
        margin: 0 !important;
        border-right: 1px solid ${Base.CinzaBarras};
    `;

    const DadosPerfil = styled.div`
        display: flex;
        justify-content: center;
        flex-direction: column;
        align-items: center;

        @media (max-width: 850px) {
          img{
            width: 100px !important;
            height: 100px !important;
          }
        }

        @media (max-width: 700px) {
          img{
            width: 70px !important;
            height: 70px !important;
          }
        }

        .img-edit{
            width: 215px !important; 
            height: 215px !important;
        }

        img{
            width: 172px;
            height: 172px;
            border-radius: 50%;
        }
        span{
            font-size: 14px;
        }
        .nome{
            font-size: 18px !important;
            font-weight: bold !important;
            padding: 20px 0 40px 0 !important;
        }
    `;

    const Botao = styled.a`
        display: flex !important;
        text-align: center !important;
        position: absolute;
        top:76%;
        left:65%;
    `;

    const Icone = styled.div`
        background: ${Base.Roxo};
        color: ${Base.Branco};
        font-size: 17px !important;
        height: 35px !important;
        width: 35px !important;
        vertical-align: middle;
        box-sizing: border-box;
        align-items: center !important;
        border-radius: 50%;
        display: inline-block;
        justify-content: center !important;
        i{
            margin-top:25%;
        }

        @media (max-width: 850px) {
          font-size: 10px !important;
          height: 20px !important;
          width:20px !important;
        }
  `;

    const Conteudo = styled.div``;

    const SelecionarFoto = styled.a`
        color: ${Base.Roxo} !important;
        font-size: 14px !important;
    `;

    const Topo = styled.div`
        padding-bottom: 30px;
        button{
            color: ${Base.Azul} !important;
            background: ${Base.Branco} !important;
            border: 1px solid ${Base.Azul} !important;
        }

        button:hover{
            color: ${Base.Branco} !important;
            background: ${Base.Azul} !important;
        }
    `;

    const irParaDashboard = () => {
        history.push('/');
    }

    const mostrarModal = () => {
        setAlterarFoto(!alterarFoto)
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
                    titulo="Alterar Foto"
                    closable={true}
                >
                    <DadosPerfil className="col-12">
                        <img className="img-edit" id="foto-perfil" src={dados.foto}/>
                    </DadosPerfil>
                    <DadosPerfil className="col-12">
                        <SelecionarFoto className="text-center">Selecionar nova foto</SelecionarFoto>
                    </DadosPerfil>

                </ModalConteudoHtml>
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
                        <img id="foto-perfil" src={dados.foto} />
                        <Botao className="text-center" onClick={mostrarModal}>
                            <Icone>
                                <i className="fas fa-camera" />
                            </Icone>
                        </Botao>
                    </DadosPerfil>
                    <DadosPerfil className="text-center">
                        <span className="nome">{dados.nome}</span>
                        <span hidden={!dados.rf}>RF: {dados.rf}</span>
                        <span hidden={!dados.cpf}>CPF: {dados.cpf}</span>
                        <span hidden={!dados.empresa}>Empresa: {dados.empresa}</span>
                    </DadosPerfil>
                </Perfil>
                <Conteudo className="col-8">
                </Conteudo>
            </Card>
        </div>
    );
}

export default MeusDados;
