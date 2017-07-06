using System.Security.Cryptography;
using System.Text;

namespace BattleShip.Repository.RepositoryHelpers
{
    public static class PasswordHelper
    {
        public static string GetSha512CngPasswordHash(string password)
        {
            var data = Encoding.UTF8.GetBytes(password);
            byte[] hash;

            using (var sha512Cng = new SHA512Cng())
            {
                hash = sha512Cng.ComputeHash(data);
            }

            return Encoding.UTF8.GetString(hash);
        }
    }
}