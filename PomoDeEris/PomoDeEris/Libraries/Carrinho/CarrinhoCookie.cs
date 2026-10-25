using Google.Protobuf.Collections;
using Newtonsoft.Json;
using PomoDeEris.Models;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace PomoDeEris.Libraries.Carrinho
{
    public class CarrinhoCookie
    {
        private string Key = "Carrinho.Compras";

        private Cookie.Cookie _cookie;

        public CarrinhoCookie(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        public void Salvar(List<Pacote> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }

        public List<Pacote> Consultar()
        {
            if (_cookie.Existe(Key))
            {
                string valor = _cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<Pacote>>(valor);

            }
            else
            {
                return new List<Pacote>();
            }
        }
        public void Cadastrar(Pacote item)
        {
            List<Pacote> Lista;
            if (_cookie.Existe(Key))
            {
                Lista = Consultar();
                var itemLocalizado = Lista.SingleOrDefault(a => a.IdPacote == item.IdPacote);
                if (itemLocalizado == null)
                {
                    Lista.Add(item);
                }
                else
                {
                    itemLocalizado.Quantidade = itemLocalizado.Quantidade + 1;
                }
            }
            else
            {
                Lista = new List<Pacote>();
                Lista.Add(item);
            }
            Salvar(Lista);
        }

        //Atualiza
        public void Atualizar(Pacote item)
        {
            var Lista = Consultar();
            var itemLocalizado = Lista.SingleOrDefault(a => a.IdPacote == item.IdPacote);

            if (itemLocalizado != null)
            {
                itemLocalizado.Quantidade = itemLocalizado.Quantidade + 1;
                Salvar(Lista);
            }
        }

        //remove item 
        public void Remover(Pacote item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.IdPacote == item.IdPacote);

            if (ItemLocalizado != null)
            {
                Lista.Remove(ItemLocalizado);
                Salvar(Lista);
            }
        }
        public bool Existe(string Key)
        {
            if (_cookie.Existe(Key))
            {
                return false;
            }
            return true;
        }
        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }
        public int ContarItens()
        {
            var Lista = Consultar();
            return Lista.Count;
        }

    }
}
