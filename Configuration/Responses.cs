using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PruebaConsole.Configuration;
public  class Responses
{
    private readonly TimeSpan _timeSpan;
    private readonly Timer _timer;
    public Responses(){
        _timeSpan = TimeSpan.FromMinutes(2);
        _timer = new Timer(TimerResponse, null, _timeSpan, _timeSpan);
    }
    public  void SelectResponse(HttpListenerContext context)
    {
    }
    private void TimerResponse(object state){

    }
}