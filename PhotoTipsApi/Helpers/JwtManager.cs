using System.Collections.Generic;
using JetBrains.Annotations;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using PhotoTipsApi.Models;
using PhotoTipsApi.Repositories;

namespace PhotoTipsApi.Helpers
{
    public class JwtManager
    {
        private readonly JwtBuilder _builder;
        private readonly string _secret;

        public JwtManager(IJwtAlgorithm algorithm = null, IJsonSerializer serializer = null,
            IBase64UrlEncoder urlEncoder = null, string secret = null)
        {
            _builder = new JwtBuilder().WithAlgorithm(algorithm ?? new HMACSHA256Algorithm())
                .WithSerializer(serializer ?? new JsonNetSerializer())
                .WithUrlEncoder(urlEncoder ?? new JwtBase64UrlEncoder())
                .WithSecret(secret ?? "43dcd4fa564342456b373347b5f68389ccc3e04c638a1e8735ab1c4cc5a6eea4");
        }

        public string Encode(object payload)
        {
            return _builder.AddClaim("payload", payload).Encode();
        }

        public User CheckUser(UserRepository userRepository, string token)
        {
            IDictionary<string, string> payload;
            _builder.MustVerifySignature().Decode<IDictionary<string, IDictionary<string, string>>>(token)
                .TryGetValue("payload", out payload);

            if (payload != null && payload.TryGetValue("passwordHash", out var passwordHash))
            {
                if (payload.TryGetValue("email", out var email))
                    return userRepository.FindByEmailAndPassword(email, passwordHash);

                if (payload.TryGetValue("phoneNumber", out var phoneNumber))
                    return userRepository.FindByPhoneNumberAndPassword(phoneNumber, passwordHash);
            }

            return null;
        }
    }
}