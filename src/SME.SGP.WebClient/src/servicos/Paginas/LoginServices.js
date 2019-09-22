import Axios from "axios"
import api from "../api";

class LoginService {

    autenticar = async (Login) => {

        return await api.post(this.obtenhaUrlAutenticacao(), { login: Login.usuario, senha: Login.senha })
            .then(res => { return { sucesso: true, mensagem: "Usuario Logado com sucesso", dados: res.data } })
            .catch(err => {
                
                const status = err.response.status;

                if(status === 401)
                    return { sucesso: false, erroGeral: "Usuário e/ou senha invalida" }; 
                
                return { sucesso: false, errosModal: err.data.mensagens }; 
            
            });
    };

    obtenhaUrlAutenticacao = () =>{
        return "v1/autenticacao";
    };

}

export default new LoginService();