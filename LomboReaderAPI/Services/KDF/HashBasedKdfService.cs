using DevOpseTest.Services.Hash;

namespace DevOpseTest.Services.KDF
{
    public class HashBasedKdfService : IKDFService
    {
        private readonly IHashService _hashService;

        public HashBasedKdfService(IHashService hashService)
        {
            _hashService = hashService;
        }

        public string GetDirivedKey(string password, string salt)
        {
            return _hashService.GetHash(password + salt);
        }
    }
}
