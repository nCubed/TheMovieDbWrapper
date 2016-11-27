using System;

namespace DM.MovieApi.ApiResponse
{
    public class ApiRateLimit
    {
        private static readonly DateTime UnixEpoch = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );
        private long _resetUnixSeconds;

        public ApiRateLimit( int allowed, int remaining, long resetUnixSeconds )
        {
            Allowed = allowed;
            Remaining = remaining;
            ResetUnixSeconds = resetUnixSeconds;
        }

        public int Allowed { get; }

        public int Remaining { get; }

        public DateTime ResetDate { get; private set; }

        public long ResetUnixSeconds
        {
            get { return _resetUnixSeconds; }
            private set
            {
                _resetUnixSeconds = value;
                ResetDate = UnixEpoch.AddSeconds( _resetUnixSeconds );
            }
        }

        public override string ToString()
            => $"{Remaining} of {Allowed}, Resets on: {ResetDate}";
    }
}
