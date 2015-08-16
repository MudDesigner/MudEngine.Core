using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine
{
    public sealed class ThreadContext<T>
    {
        private SynchronizationContext context;

        private Action<T> callback;

        public ThreadContext(SynchronizationContext syncContext, Action<T> callback)
        {
            this.callback = callback;
            this.context = syncContext;
        }

        public void Invoke(T item)
        {
            this.context.Post(this.ContextHandler, item);
        }

        private void ContextHandler(object item)
        {
            this.callback((T)item);
        }
    }
}
