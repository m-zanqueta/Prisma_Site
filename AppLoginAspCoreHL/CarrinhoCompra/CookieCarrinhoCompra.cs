using Newtonsoft.Json;
using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Cookie;

namespace AppLoginAspCoreHL.CarrinhoCompra
{
    public class CookieCarrinhoCompra
    {
        private string Key = "Carrinho.Compras";
        private Cookie.Cookie _cookie;

        public CookieCarrinhoCompra(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        public void Salvar(List<Livro> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }
        public List<Livro> Consultar()
        {
            if (_cookie.Existe(Key))
            {
                string valor = _cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<Livro>>(valor);
            }else
            {
                return new List<Livro>();
            }
        }
        public void Cadastrar(Livro item) 
        {
            List<Livro> Lista;
            if (_cookie.Existe(Key)) 
            {
                Lista = Consultar();
                var ItemLocalizado = Lista.SingleOrDefault(a => a.Id == item.Id);
                if (ItemLocalizado == null) 
                {
                    Lista.Add(item);
                }
                else
                {
                    ItemLocalizado.QuantidadeEstq = ItemLocalizado.QuantidadeEstq + 1;
                }
            }else
            {
                Lista = new List<Livro>();
                Lista.Add(item);
            }
            Salvar(Lista);
        }
        public void Atuaizar(Livro item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.Id == item.Id);

            if(ItemLocalizado != null)
            {
                ItemLocalizado.QuantidadeEstq = item.QuantidadeEstq + 1;
                Salvar(Lista);
            }
        }
        public void Remover(Livro item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.Id == item.Id);

            if(ItemLocalizado != null)
            {
                Lista.Remove(ItemLocalizado);
                Salvar(Lista);
            }
        }
        public bool Existe(string Key)
        {
            if(_cookie.Existe(Key))
            {
                return false;
            }

            return true;
        }
        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }
    }
}
