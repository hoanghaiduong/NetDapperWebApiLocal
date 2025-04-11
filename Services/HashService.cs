
using System.Security.Cryptography;
using System.Text;

namespace NetDapperWebApi.Services
{
    public static class HashService
    {
        // Hàm băm mật khẩu sử dụng SHA-256 kèm theo salt
        public static string EnhancedHashPassword(string password)
        {
            // Tạo một salt ngẫu nhiên
            var salt = GenerateSalt();

            // Chuyển đổi mật khẩu và salt thành chuỗi byte và băm chúng
            var hash = ComputeSHA256Hash(password, salt);

            // Trả về salt và hash (mã hóa base64) dưới dạng chuỗi, cách nhau bởi dấu ':'
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        // Hàm xác thực mật khẩu với hash đã mã hóa
        public static bool VerifyPassword(string password, string storedHash)
        {
            // Tách salt và hash ra từ chuỗi lưu trữ
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            // Giải mã base64 để lấy lại salt và hash từ chuỗi lưu trữ
            var salt = Convert.FromBase64String(parts[0]);
            var storedHashBytes = Convert.FromBase64String(parts[1]);

            // Băm mật khẩu đã nhập với salt đã lưu trữ
            var hash = ComputeSHA256Hash(password, salt);

            // So sánh hash vừa tạo với hash đã lưu
            return AreHashesEqual(storedHashBytes, hash);
        }

        // Hàm tạo salt ngẫu nhiên
        private static byte[] GenerateSalt(int length = 16)
        {
            var salt = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Hàm băm sử dụng SHA-256
        private static byte[] ComputeSHA256Hash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Chuyển đổi mật khẩu thành chuỗi byte
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                // Kết hợp salt và mật khẩu
                var saltedPassword = new byte[salt.Length + passwordBytes.Length];
                Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

                // Băm chuỗi kết hợp này
                return sha256.ComputeHash(saltedPassword);
            }
        }

        // Hàm so sánh hai hash
        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            // So sánh hai mảng byte
            var minHashLength = Math.Min(firstHash.Length, secondHash.Length);
            for (int i = 0; i < minHashLength; i++)
            {
                if (firstHash[i] != secondHash[i])
                {
                    return false;
                }
            }
            return firstHash.Length == secondHash.Length;
        }
    }
}