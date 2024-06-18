using Org.BouncyCastle.Asn1.Cmp;

namespace AppLoginAspCoreHL.Libraries.Sessao
{
    public class Sessao
    {
        IHttpContextAccessor _context;
        public Sessao(IHttpContextAccessor context)
        {
            _context = context;
        }
        public void Cadastrar(string Key, string Valor)
        {
            _context.HttpContext.Session.SetString(Key, Valor);
        }
        public string Consultar(string Key) 
        {
            return _context.HttpContext.Session.GetString(Key);
        }
        public bool Existe(string Key)
        {
            if (_context.HttpContext.Session.GetString(Key) == null)
            {
                return false;
            }
            return true;
        }
        public void Remover(string Key)
        {
            _context.HttpContext.Session.Remove(Key);
        }
        public void RemoverTodos()
        {
            _context.HttpContext.Session.Clear();
        }
        public void Atualizar(string Key, string Valor)
        {
            if(Existe(Key))
            {
                _context.HttpContext.Session.Remove(Key);
            }
            _context.HttpContext.Session.SetString(Key, Valor);
        }
    }
}