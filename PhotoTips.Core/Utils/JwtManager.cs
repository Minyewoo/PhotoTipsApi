using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using PhotoTips.Core.Models;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Core.Utils
{
    public class JwtManager
    {
        private readonly JwtBuilder _builder;
        private const string Secret = "43dcd4fa564342456b373347b5f68389ccc3e04c638a1e8735ab1c4cc5a6eea4";

        public JwtManager(IJwtAlgorithm algorithm = null, IJsonSerializer serializer = null,
            IBase64UrlEncoder urlEncoder = null, string secret = null)
        {
            _builder = new JwtBuilder().WithAlgorithm(algorithm ?? new HMACSHA256Algorithm())
                .WithSerializer(serializer ?? new JsonNetSerializer())
                .WithUrlEncoder(urlEncoder ?? new JwtBase64UrlEncoder())
                .WithSecret(secret ?? Secret);
        }

        public string GetToken(User user)
        {
            return user.Email != null
                ? Encode(new Dictionary<string, string>
                    {{"email", user.Email}, {"passwordHash", user.PasswordHash}})
                : Encode(new Dictionary<string, string>
                    {{"phoneNumber", user.PhoneNumber}, {"passwordHash", user.PasswordHash}});
        }

        public string Encode(object payload)
        {
            return _builder.AddClaim("payload", payload).Encode();
        }

        public IDictionary<string, string> Decode(string token)
        {
            IDictionary<string, string> payload;
            _builder.MustVerifySignature().Decode<IDictionary<string, IDictionary<string, string>>>(token)
                .TryGetValue("payload", out payload);

            return payload;
        }

        public async Task<User> FindUserByToken(string userToken, IUserRepository userRepository,
            CancellationToken cancellationToken)
        {
            var payload = Decode(userToken);

            if (payload == null || !payload.TryGetValue("passwordHash", out var passwordHash))
                return null;

            User user;

            if (payload.TryGetValue("email", out var email))
                user = await userRepository.FindByEmailAndPassword(email, passwordHash, cancellationToken);

            else if (payload.TryGetValue("phoneNumber", out var phoneNumber))
                user = await userRepository.FindByPhoneNumberAndPassword(phoneNumber, passwordHash, cancellationToken);
            else return null;

            return user;
        }
    }
}