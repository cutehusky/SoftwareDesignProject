using Microsoft.Extensions.Primitives;

namespace SoftwareDesignProject.Services;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Threading;

// https://stackoverflow.com/questions/46156649/asp-net-core-register-controller-at-runtime
public class ActionDescriptorChangeProvider : IActionDescriptorChangeProvider
{
    private CancellationTokenSource TokenSource { get; set; } = new();

    public void NotifyChanges()
    {
        TokenSource.Cancel();
    }

    public IChangeToken GetChangeToken()
    {
        TokenSource = new CancellationTokenSource();
        return new CancellationChangeToken(TokenSource.Token);
    }
}
