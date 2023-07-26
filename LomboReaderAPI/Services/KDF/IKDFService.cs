namespace DevOpseTest.Services.KDF
{
    /*
     * Key Derivation Function Service(by rfc-2898 https://datatracker.ietf.org/doc/html/rfc2898#section-5)
    */
    public interface IKDFService
    {
        String GetDirivedKey(String password, string salt);
    }
}
