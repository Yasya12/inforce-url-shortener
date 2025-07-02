using Application.Interfaces;
using System.Text;

namespace backend.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly int _base = Alphabet.Length;

        public string GenerateShortCode(int id)
        {
            if (id == 0) return Alphabet[0].ToString();

            var sb = new StringBuilder();
            while (id > 0)
            {
                sb.Insert(0, Alphabet[id % _base]);
                id = id / _base;
            }
            return sb.ToString();
        }
    }

}
