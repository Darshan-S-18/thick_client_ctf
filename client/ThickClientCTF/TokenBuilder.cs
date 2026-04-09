using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ThickClientCTF
{
    internal sealed class TokenBuilder
    {
        private readonly MainForm _owner;

        public TokenBuilder(MainForm owner)
        {
            _owner = owner;
        }

        public string BuildToken()
        {
            var fragmentA = BuildExecutionFragment();
            var fragmentB = BuildInteractionFragment();
            var fragmentC = BuildRuntimeFragment();

            var payload = string.Join("|", new[] { fragmentA.methodHash, fragmentA.timestamp, fragmentB, fragmentC });
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
        }

        private (string methodHash, string timestamp) BuildExecutionFragment()
        {
            var checkMethod = typeof(MainForm).GetMethod("CheckAccess", BindingFlags.NonPublic | BindingFlags.Instance);
            if (checkMethod == null)
            {
                throw new InvalidOperationException("Access gate method missing");
            }

            var il = checkMethod.GetMethodBody()?.GetILAsByteArray();
            if (il == null || il.Length == 0)
            {
                throw new InvalidOperationException("Unable to inspect method body");
            }

            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(il);
                var methodHash = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmm");
                return (methodHash, timestamp);
            }
        }

        private string BuildInteractionFragment()
        {
            var flags = new[]
            {
                _owner.ButtonWasRevealed,
                _owner.ButtonWasClicked,
                _owner.DecoyModeEnabled
            };

            var packed = flags.Select(x => x ? "1" : "0");
            return string.Concat(packed);
        }

        private string BuildRuntimeFragment()
        {
            var entropy = _owner.RuntimeSeed;
            var user = Environment.UserName ?? "unknown";
            var machine = Environment.MachineName ?? "host";
            var source = $"{entropy}:{user}:{machine}";

            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(source));
                return Convert.ToBase64String(bytes).TrimEnd('=');
            }
        }
    }
}
