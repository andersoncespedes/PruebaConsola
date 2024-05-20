using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaConsole.Configuration;
public class RateLimiter
{
    
    private readonly Dictionary<string, int> requestCounts = new Dictionary<string, int>();
    private readonly int _maxRequest;
    private readonly TimeSpan _perInterval;
    private readonly Timer _timer;
    public RateLimiter()
    {
        _maxRequest = 10;
        _perInterval = TimeSpan.FromSeconds(10);
        _timer = new Timer(ResetCounts, null, _perInterval, _perInterval);
    }
    public bool AllowRequest(string ip)
    {
        lock (requestCounts)
        {
            if (!requestCounts.ContainsKey(ip))
            {
                requestCounts[ip] = 1;
                return true;
            }

            if (requestCounts[ip] < _maxRequest)
            {
                requestCounts[ip]++;
                return true;
            }

            return false;
        }
    }
    private void ResetCounts(object state)
    {
        lock (requestCounts)
        {
            requestCounts.Clear();
        }
    }
}