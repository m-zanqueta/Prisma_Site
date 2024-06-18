using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Models.Constant;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using Newtonsoft.Json;

namespace AppLoginAspCoreHL.Libraries.Login
{
    public class LoginColaborador
    {
        private string Key = "Login.Colaborador";
        private Sessao.Sessao _sessao;
        public LoginColaborador(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }
        public void Login(Colaborador colaborador)
        {
            string colaboradorJSONString = JsonConvert.SerializeObject(colaborador);    
            _sessao.Cadastrar(Key, colaboradorJSONString);
        }
        public Colaborador GetColaborador()
        {
            if (_sessao.Existe(Key))
            {
                string colaboradorJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Colaborador>(colaboradorJSONString);
            }
            else
            {
                return null;
            }
        }
        public void Logout()
        {
            _sessao.RemoverTodos();
        }
        
    }
}
